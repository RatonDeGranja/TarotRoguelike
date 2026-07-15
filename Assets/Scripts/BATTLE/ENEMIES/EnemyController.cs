using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldSide 
{ 
    Left, 
    Right 
}

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Datos (ScriptableObject)")]
    public Enemy enemyData;
    
    // Referencia al sprite para asignarle el dibujo del ScriptableObject
    [SerializeField] private SpriteRenderer spriteRenderer; 

    [SerializeField] private int health;
    private int maxHealth;
    private int currentPatternIndex = 0;

    [SerializeField] private bool atras;

    public int Health => health;
    public int MaxHealth => maxHealth;

    FieldSide enemySide;


    [Header("Interfaz")]
    private HealthBar healthBar;
    private RectTransform healthBarRect;

    private void Awake()
    {
        // 1. Busca automáticamente en todos sus hijos el script HealthBar
        healthBar = GetComponentInChildren<HealthBar>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        //Initialize();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null) 
        {
            healthBar.UpdateHealth(health, maxHealth);
        }
    }

    private void PositionHealthBar()
    {
        if (healthBarRect != null && spriteRenderer.sprite != null)
        {
            Debug.Log("Entra a posicionar la barra");
            float spriteHalfHeight = spriteRenderer.sprite.bounds.extents.y;
            float topOffset = 0.2f; 
            
            // Movemos el RectTransform hacia arriba
            healthBarRect.localPosition = new Vector3(0, -(spriteHalfHeight + topOffset), 0);
        }
    }

    public void Initialize(Enemy data, FieldSide side)
    {
        enemyData = data;
        enemySide = side;

        // 1. Cargamos los datos del SO
        health = enemyData.MaxLife;
        maxHealth = enemyData.MaxLife;
        
        if (spriteRenderer != null && enemyData.Artwork != null)
        {
            spriteRenderer.sprite = enemyData.Artwork;

            if (enemySide == FieldSide.Right)
            {
                spriteRenderer.flipX = true; 
            }
            else 
            {
                spriteRenderer.flipX = false; 
            }

            BoxCollider2D col = GetComponent<BoxCollider2D>();
            if (col != null && spriteRenderer.sprite != null)
            {
                col.size = spriteRenderer.sprite.bounds.size;
            }
        }

        PositionHealthBar();

        UpdateHealthBar();


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
        Debug.Log("Entra en take damage");
        StartCoroutine(TakeDamageOneByOne(damage));
    }

    private IEnumerator TakeDamageOneByOne(int damage)
    {
        Debug.Log("Entra en TakeDamageOneByOne");
        for (int i = 0; i < damage; i++)
        {
            health -= 1;
            if (health < 0) health = 0;

            UpdateHealthBar();
            
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
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        // 1. Guardamos el color original (ej: blanco puro)
        Color startColor = spriteRenderer.color;
        
        // 2. Creamos el color final: El mismo RGB original, pero con Alfa a 0 (transparente)
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        
        float timeElapsed = 0f;
        float fadeDuration = 1f; // ¿Cuántos segundos quieres que tarde en desaparecer?

        // 3. El bucle que se ejecuta fotograma a fotograma
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime; // Sumamos el tiempo que ha pasado
            
            // Calculamos el porcentaje de la animación (de 0.0 a 1.0)
            float t = timeElapsed / fadeDuration;
            
            // Hacemos el Lerp perfecto
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            
            // Le decimos a Unity: "Pausa esto aquí y vuelve en el siguiente fotograma"
            yield return null; 
        }

        // 4. Aseguramos que acabe 100% transparente por si hubo algún salto de frames
        spriteRenderer.color = endColor;

        // 5. Destruimos al enemigo, lo devolvemos a una Pool, o le decimos al EncounterManager que ha muerto
        GameEvents.onEnemyDied?.Invoke(this); // Avisar a la UI que borre los elementos asociados
        gameObject.SetActive(false);
    }


    public void setAtras(bool set) => atras = set;
    public bool getAtras() => atras;
}