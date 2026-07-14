using System.Collections;
using UnityEngine;

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
    }

    private void Start()
    {
        // Valores iniciales por defecto al empezar el combate
        currentWisdom = startWisdom;
        armor = 0;

        // Avisamos a la UI de los valores iniciales
        GameEvents.onPlayerWisdomChanged?.Invoke(currentWisdom);
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