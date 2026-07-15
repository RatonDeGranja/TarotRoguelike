using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Player { get; private set; }

    [Header("BASE STATS")]
    [SerializeField]
    private int startWisdom;
    private int currentWisdom;

    [SerializeField]
    private int maxHealth;
    private int currentHealth;

    [Header("TEMPORAL STATS")]
    private int armor;

    [Header("ARCANO MAYOR")]
    // 1. Variable privada para el Inspector
    [SerializeField] private Major equippedMajor;

    [Header("Referencias Visuales")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Interfaz")]
    private HealthBar healthBar;
    private RectTransform healthBarRect;

    public int Wisdom => currentWisdom;
    public int Health => currentHealth;
    public int MaxHealth => maxHealth;
    public int Armor => armor;
    public Major EquippedMajor => equippedMajor;

    private void Awake()
    {
        // Inicialización del Singleton
        if (Player == null) Player = this;
        else Destroy(gameObject);

        healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            Debug.Log("Hay health bar");
            healthBarRect = healthBar.GetComponent<RectTransform>();
            Debug.Log(healthBarRect);
        }
    }

    private void Start()
    {
        // Valores iniciales por defecto al empezar el combate
        currentWisdom = startWisdom;
        armor = 0;

        //QUITAR CUANDO YA HAYA BUCLE DE JUEGO PARA NO CURARTE ENTRE COMBATES
        currentHealth = maxHealth;


        UpdateHealthBar();
        PositionHealthBar();

        // Avisamos a la UI de los valores iniciales
        GameEvents.onPlayerWisdomChanged?.Invoke(currentWisdom);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentHealth -= 10;
            
            UpdateHealthBar();
            
            Debug.Log($"¡Ouch! Vida actual: {currentHealth}");
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null) 
        {
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }
    }

    private void PositionHealthBar()
    {
        if (healthBarRect != null && spriteRenderer.sprite != null)
        {
            Debug.Log("Entra a posicionar la barra del jugador");
            float spriteHalfHeight = spriteRenderer.sprite.bounds.extents.y;
            float topOffset = 0.5f; 
            
            // Movemos el RectTransform hacia arriba
            healthBarRect.localPosition = new Vector3(0, spriteHalfHeight + topOffset, 0);
        }
    }


    public void takeDamage(int damage)
    {
        int penDamage = damage-armor;
        armor -= damage;
        if (armor < 0) armor = 0;
        
        if(penDamage > 0) {StartCoroutine(TakeDamageSteps(penDamage));}
    }

    public void heal(int life_gained)
    {  
        StartCoroutine(GainLifeSteps(life_gained));
    }

    IEnumerator TakeDamageSteps(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            currentHealth -= 1;
            if (currentHealth < 0) currentHealth = 0;

            UpdateHealthBar();

            // Mandamos el grito a la UI con la información actualizada
            GameEvents.onPlayerHealthChanged?.Invoke(currentHealth, maxHealth);

            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator  GainLifeSteps(int life_gained)
    {
        for (int i = 0; i < life_gained; i++)
        {
            currentHealth += 1;
            if (currentHealth > maxHealth) currentHealth = maxHealth;

            // Mandamos el grito a la UI con la información actualizada
            GameEvents.onPlayerHealthChanged?.Invoke(currentHealth, maxHealth);

            yield return new WaitForEndOfFrame();
        }
    }
}