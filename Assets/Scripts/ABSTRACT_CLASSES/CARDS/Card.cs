using UnityEngine;

public abstract class Card : ScriptableObject
{
    [SerializeField] private string nameKey; 
    [SerializeField] private string descriptionKey; 

    [SerializeField] string ID;

    [SerializeField]
    Sprite cardArt;


    // Propiedades de C# (Solo lectura)
    public Sprite CardArt => cardArt;
    public string CardName => LocalizationManager.Instance.GetText(nameKey);
    public string CardDescription => LocalizationManager.Instance.GetText(descriptionKey);

    public virtual string GetFormattedDescription()
    {
        // Por defecto, devuelve el texto crudo del CSV, las cartas con variables la tendran que sobreescribir
        return LocalizationManager.Instance.GetText(descriptionKey);
    }
}
