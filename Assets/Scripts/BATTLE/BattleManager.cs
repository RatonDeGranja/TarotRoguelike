using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public enum BattleState
    {
        START,
        PLAYER_TURN,
        WAITING_TARGET,
        ENEMY_TURN,
        WIN,
        LOSE
    }

    [Header("Estado Actual")]
    private BattleState state;

    // Lista para llevar el control de los enemigos vivos
    private List<EnemyController> activeEnemies = new List<EnemyController>();
    
    // Variables temporales para cuando el jugador va a usar una carta sobre un enemigo
    [Header("Gestión De Cartas")]
    private Minor pendingCard; 
    public Minor PendingCard => pendingCard;
    private Major equippedMajor;
    private bool isMajorInverted = false;
    private Minor lastCardPlayed;
    public Minor LastCardPlayer => lastCardPlayed;
    private int wandExtraDamage;
    public int WandExtraDamage => wandExtraDamage;

    private int hasExtraTurn = 0;
    public int HasExtraTurn => hasExtraTurn;

    public void AddExtraTurn()
    {
        hasExtraTurn+=1;
    }

    public void AddWandExtraDamage()
    {
        wandExtraDamage+=1;
    }




    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // Clic derecho para cancelar si estábamos esperando objetivo
        if (state == BattleState.WAITING_TARGET && Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            pendingCard = null;
            state = BattleState.PLAYER_TURN;
            Debug.Log("Lanzamiento cancelado.");
        }
    }

    private void OnEnable()
    {
        // Nos suscribimos a los nacimientos y muertes para mantener la lista actualizada
        GameEvents.onEnemySpawned += AddEnemy;
        GameEvents.onEnemyDied += RemoveEnemy;
    }

    private void OnDisable()
    {
        GameEvents.onEnemySpawned -= AddEnemy;
        GameEvents.onEnemyDied -= RemoveEnemy;
    }

    private void Start()
    {
        // Aqui se cargaran los datos del SaveManager
        // Por ahora, simulamos el inicio del combate
        //StartBattle();
    }

    public void StartBattle()
    {
        state = BattleState.START;
        
        // 1. Inicializamos el mazo
        DeckManager.Instance.InitializeDeck();

        // 2. Activamos la pasiva del Arcano Mayor
        equippedMajor = PlayerController.Player.EquippedMajor;
        equippedMajor?.SubscribePassive();
        isMajorInverted = false;
        hasExtraTurn = 0;
        wandExtraDamage = 0;


        // 3. Empezamos el turno
        StartCoroutine(SetupBattleAndStartPlayerTurn());
    }

    private IEnumerator SetupBattleAndStartPlayerTurn()
    {
        // Pequeña pausa para que las barras de vida y animaciones iniciales terminen
        yield return new WaitForSeconds(1f); 
        
        StartPlayerTurn();
    }

    // --- FLUJO DEL TURNO DEL JUGADOR ---

    public void StartPlayerTurn()
    {
        state = BattleState.PLAYER_TURN;
        
        // El grito global activa las pasivas
        GameEvents.onTurnStart?.Invoke(); 

        // Robamos la mano inicial de este turno
        DeckManager.Instance.DrawStartingHand(); 
        
        Debug.Log("Turno del Jugador. Esperando acciones...");
    }

    // Este método lo llamará el HandManager cuando el jugador arrastre una carta
    public void PlayCard(Minor cardToPlay)
    {
        if (state != BattleState.PLAYER_TURN) return;

        if (PlayerController.Player.Wisdom < cardToPlay.WisdomCost || PlayerController.Player.Wisdom < cardToPlay.MinWisdomRequired) return;

        if (cardToPlay.Target == TargetType.ToPlayer)
        {
            // Ejecución inmediata
            cardToPlay.PlayCard(PlayerController.Player, null);
            GameEvents.onCardPlayed?.Invoke(cardToPlay);

            if ((cardToPlay.CanBeSavedAsLast)) lastCardPlayed = cardToPlay;
        }
        else if (cardToPlay.Target == TargetType.ToEnemy)
        {
            // Esperamos a que el jugador haga clic en un enemigo
            pendingCard = cardToPlay;
            state = BattleState.WAITING_TARGET;
            Debug.Log($"Esperando objetivo para {cardToPlay.CardName}...");
        }

        
    }

    public void PlayCardOnTarget(Minor playedCard, EnemyController target)
    {
        if (state != BattleState.PLAYER_TURN) return;

        if (PlayerController.Player.Wisdom < playedCard.WisdomCost || PlayerController.Player.Wisdom < playedCard.MinWisdomRequired) return;

        // Ejecutamos la matemática
        playedCard.PlayCard(PlayerController.Player, target);
        
        // Lanzamos el grito global para descartarla y limpiar memoria
        GameEvents.onCardPlayed?.Invoke(playedCard);
    }

    // Este método lo llama el EnemyController al hacerle click
    public void OnEnemyClicked(EnemyController clickedEnemy)
    {
        if (state == BattleState.WAITING_TARGET && pendingCard != null && !PlayerController.Player.CheckState(States.NO_ATTACK))
        {
            if (PlayerController.Player.CheckBackwards(clickedEnemy.Side))
            {
                PlayerController.Player.ChangeDirection();
            }

            if ((pendingCard.CanBeSavedAsLast)) lastCardPlayed = pendingCard;
            pendingCard.PlayCard(PlayerController.Player, clickedEnemy);
            GameEvents.onCardPlayed?.Invoke(pendingCard);

            pendingCard = null;
            state = BattleState.PLAYER_TURN;
        }
    }

    // Puedes llamar a este método si el jugador cancela el lanzamiento de la carta 
    public void CancelTargeting()
    {
        if (state == BattleState.WAITING_TARGET)
        {
            pendingCard = null;
            state = BattleState.PLAYER_TURN;
            Debug.Log("Lanzamiento cancelado.");
        }
    }

    // Este método lo llamará el botón de "Terminar Turno" de la UI
    public void EndPlayerTurn()
    {
        if (state != BattleState.PLAYER_TURN && state != BattleState.WAITING_TARGET) return;

        DeckManager.Instance.DiscardHand();
        
        GameEvents.onTurnEnd?.Invoke();

        if (hasExtraTurn>0)
        {
            hasExtraTurn -= 1; 
            StartPlayerTurn();
        }
        else
        {
            StartCoroutine(EnemyTurnRoutine());
        }
    }

    // --- FLUJO DEL TURNO DEL ENEMIGO ---

    private IEnumerator EnemyTurnRoutine()
    {
        state = BattleState.ENEMY_TURN;
        Debug.Log("Turno de los Enemigos...");

        // Usamos una copia de la lista por si un enemigo muere por efecto de veneno durante su propio turno (ejemplo)
        List<EnemyController> enemiesToAct = new List<EnemyController>(activeEnemies);

        foreach (EnemyController enemy in enemiesToAct)
        {
            // Si el enemigo sigue vivo, ataca
            if (enemy.Health > 0)
            {
                enemy.Act();
                yield return new WaitForSeconds(1f); // Ritmo visual entre ataques
            }
        }

        // Al terminar, vuelve a ser el turno del jugador
        StartPlayerTurn();
    }

    // --- ARCANO MAYOR ---

    // Este método lo llama el botón de UI de "Invertir"
    public void InvertMajorArcana()
    {
        if (isMajorInverted || equippedMajor == null) return;

        isMajorInverted = true;
        equippedMajor.UnsubscribePassive(); // Quitamos la pasiva
        equippedMajor.ActiveEffect(); 

        // Avisamos a la UI para que haga la animación de girar la carta
        GameEvents.onMajorInverted?.Invoke(); 
    }

    // --- GESTIÓN DE LISTA DE ENEMIGOS ---

    private void AddEnemy(EnemyController enemy)
    {
        if (!activeEnemies.Contains(enemy)) activeEnemies.Add(enemy);
    }

    private void RemoveEnemy(EnemyController enemy)
    {
        if (activeEnemies.Contains(enemy)) activeEnemies.Remove(enemy);
        
        // Comprobar victoria
        if (activeEnemies.Count == 0)
        {
            state = BattleState.WIN;

            if (EncounterManager.Instance != null)
            {
                PlayerController.Player.GainGold(EncounterManager.Instance.WinGold);
                Debug.Log($"¡Has ganado {EncounterManager.Instance.WinGold} de oro!");
            }
            
            Debug.Log("¡Batalla Ganada!");
            GameEvents.onBattleWon?.Invoke();
        }
    }

    public BattleState State => state;
}