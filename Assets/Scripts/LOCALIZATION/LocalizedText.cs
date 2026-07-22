using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    [Header("Clave del Diccionario")]
    [SerializeField] private string translationKey; //"BTN_CONTINUE" o "UI_HEALTH"

    private TMP_Text myText;

    private void Awake()
    {
        myText = GetComponent<TMP_Text>();
        
    }

    private void Start()
    {
        UpdateTranslation();
    }


    private void OnEnable()
    {
        GameEvents.onLanguageChanged += UpdateTranslation;
        if (LocalizationManager.Instance != null)
        {
            UpdateTranslation();
        }
    }

    private void OnDisable()
    {
        GameEvents.onLanguageChanged -= UpdateTranslation;
    }

    private void UpdateTranslation()
    {
        Debug.Log("Cambiando Idioma");
        myText.text = LocalizationManager.Instance.GetText(translationKey);
    }
}