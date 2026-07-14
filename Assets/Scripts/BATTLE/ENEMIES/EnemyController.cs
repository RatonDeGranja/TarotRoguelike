using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Datos (ScriptableObject)")]
    public Enemy enemyData;
    
    // Referencia al sprite para asignarle el dibujo del ScriptableObject
    [SerializeField] private SpriteRenderer spriteRenderer; 

    private int health;
    private int maxHealth;
    private int currentPatternIndex = 0;

    [SerializeField] private bool atras;

    public int Health => health;
    public int MaxHealth => maxHealth;

    [Header("Interfaz")]
    private HealthBar healthBar;
    private RectTransform healthBarRect;

    private void Awake()
    {
        // 1. Busca automáticamente en todos sus hijos el script HealthBar
        healthBar = GetComponentInChildren<HealthBar>();

        // 2. Si lo encuentra, extrae automáticamente su RectTransform (para moverlo luego)
        if (healthBar != null)
        {
            Debug.Log("Hay health bar");
            healthBarRect = healthBar.GetComponent<RectTransform>();
            Debug.Log(healthBarRect);
        }
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        // 1. Cargamos los datos del SO
        health = enemyData.MaxLife;
        maxHealth = enemyData.MaxLife;
        
        if (spriteRenderer != null && enemyData.Artwork != null)
        {
            spriteRenderer.sprite = enemyData.Artwork;

            BoxCollider2D col = GetComponent<BoxCollider2D>();
            if (col != null && spriteRenderer.sprite != null)
            {
                col.size = spriteRenderer.sprite.bounds.size;
            }
        }

        if (healthBarRect != null && spriteRenderer.sprite != null)
        {
            Debug.Log("Entra a posicionar la barra");
            float spriteHalfHeight = spriteRenderer.sprite.bounds.extents.y;
            float topOffset = 0.5f; 
            
            // Movemos el RectTransform hacia arriba
            healthBarRect.localPosition = new Vector3(0, spriteHalfHeight + topOffset, 0);
        }

        if (healthBar != null) 
        {
            healthBar.UpdateHealth(health, maxHealth);
        }

        // 2. Avisamos a la UI que cree su barra
        GameEvents.onEnemySpawned?.Invoke(this);
    }

    public void Act()
    {
        if (enemyData.attackPatterns.Count == 0) return;

        // Leemos la acción directamente del ScriptableObject
        EnemyActionSO action = enemyData.attackPatterns[currentPatternIndex];
        action.Execute(this, PlayerController.Player);

        currentPatternIndex = (currentPatternIndex + 1) % enemyData.attackPatterns.Count;
    }

    public void takeDamage(int damage)
    {
        // Matemáticas puras. La lógica visual del "flip" la moveremos al BattleManager o a un script de animaciones.
        StartCoroutine(TakeDamageOneByOne(damage));
    }

    private IEnumerator TakeDamageOneByOne(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            health -= 1;
            if (health < 0) health = 0;

            if (healthBar != null) 
            {
                healthBar.UpdateHealth(health, maxHealth);
            }
            
            // Mandamos el grito a la UI con la información actualizada de ESTE enemigo
            GameEvents.onEnemyHealthChanged?.Invoke(this, health, maxHealth);
            

            yield return new WaitForEndOfFrame();

            if (health <= 0)
            {
                Die();
                yield break;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // El BattleManager comprobará si el jugador estaba intentando lanzar una carta con objetivo.
        if (BattleManager.Instance.State == BattleManager.BattleState.WAITING_TARGET)
        {
            // ...le mandamos este enemigo a tu método clásico
            BattleManager.Instance.OnEnemyClicked(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) 
    { 
        // Mostrar icono de Intención o iluminar silueta
    }

    public void OnPointerExit(PointerEventData eventData) 
    { 
        // Apagar iluminación
    }

    private void Die()
    {
        GameEvents.onEnemyDied?.Invoke(this); // Avisar a la UI que borre los elementos asociados
        gameObject.SetActive(false);
    }

    public void setAtras(bool set) => atras = set;
    public bool getAtras() => atras;
}