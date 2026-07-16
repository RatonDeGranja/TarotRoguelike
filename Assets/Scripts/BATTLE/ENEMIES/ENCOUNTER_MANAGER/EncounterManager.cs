using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager Instance;



    [Header("Datos y Prefabs")]
    [SerializeField] private EncounterData encounterData; 
    [SerializeField] private GameObject enemyDisplay; 
    private int winGold;
    public int WinGold => winGold;

    [Header("Posiciones en la escena")]
    [SerializeField] private Transform[] leftSlots;  // Los puntos vacíos a la izquierda
    [SerializeField] private Transform[] rightSlots; // Los puntos vacíos a la derecha

    private List<EnemyController> activeEnemies = new List<EnemyController>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void OnEnable()
    {
        GameEvents.onEnemyDied += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        GameEvents.onEnemyDied -= HandleEnemyDeath;
    }

    private void Start()
    {
        winGold = encounterData.VictoryGold;

        for (int i = 0; i < encounterData.leftEnemies.Length; i++)
        {
            Enemy enemyData = encounterData.leftEnemies[i];

            if (enemyData != null && i < leftSlots.Length)
            {
                // 1. Instanciamos el PREFAB, en la posición del slot
                GameObject newEnemyObj = Instantiate(enemyDisplay, leftSlots[i].position, Quaternion.identity, leftSlots[i]);

                // 2. Cogemos su EnemyController
                EnemyController controller = newEnemyObj.GetComponent<EnemyController>();

                // 3. Le inyectamos los DATOS y le decimos su lado
                if (controller != null)
                {
                    controller.Initialize(enemyData, FieldSide.Left);
                }
            }
        }

        for (int i = 0; i < encounterData.rightEnemies.Length; i++)
        {
            Enemy enemyData = encounterData.rightEnemies[i];

            if (enemyData != null && i < rightSlots.Length)
            {
                GameObject newEnemyObj = Instantiate(enemyDisplay, rightSlots[i].position, Quaternion.identity, rightSlots[i]);
                EnemyController controller = newEnemyObj.GetComponent<EnemyController>();

                if (controller != null)
                {
                    controller.Initialize(enemyData, FieldSide.Right);
                }
            }
        }
    }

    private void HandleEnemyDeath(EnemyController deadEnemy)
    {
        if (activeEnemies.Contains(deadEnemy))
        {
            activeEnemies.Remove(deadEnemy);
            Debug.Log($"Un enemigo ha caído. Quedan {activeEnemies.Count} enemigos vivos.");

            // Comprobamos si la batalla ha terminado
            CheckWinCondition();
        }
    }

    private void CheckWinCondition()
    {
        if (activeEnemies.Count == 0)
        {
            Debug.Log("¡VICTORIA! Todos los enemigos han sido derrotados.");
            // Aquí en el futuro llamaremos a la pantalla de recompensas, dar oro, etc.
        }
    }

    public void AddWinGold(int gold)
    {
        winGold += gold;
    }
}
