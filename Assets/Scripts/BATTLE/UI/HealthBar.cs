using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [Header("Referencias Visuales")]
    [SerializeField] private Image fillImage;     // La barra roja que se vacía
    [SerializeField] private TMP_Text healthText; // El texto numérico (ej. 15/15)
    [SerializeField] private float animationSpeed = 10f; // Velocidad a la que baja la sangre

    private float targetFill = 1f;

    // Se llama al nacer para configurarla
    public void Setup(int currentHealth, int maxHealth)
    {
        UpdateHealth(currentHealth, maxHealth);
        fillImage.fillAmount = targetFill; // Se ajusta al instante en el frame 1
    }

    // Se llama cada vez que alguien recibe daño o se cura
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        // Calculamos el porcentaje (de 0.0 a 1.0) para la imagen
        targetFill = (float)currentHealth / maxHealth;
        
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }

    private void Update()
    {
        // Animación fluida de la barra
        if (fillImage != null && fillImage.fillAmount != targetFill)
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, Time.deltaTime * animationSpeed);
        }
    }
}