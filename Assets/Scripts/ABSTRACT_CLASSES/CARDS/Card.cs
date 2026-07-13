using UnityEngine;

public abstract class Card : ScriptableObject
{
    [SerializeField] string cardName;

    [SerializeField]
    [TextArea(3,30)]
    string cardDescription;

    [SerializeField]
    Sprite cardArt;


    // Propiedades de C# (Solo lectura)
    public string CardName => cardName;
    public string CardDescription => cardDescription;
    public Sprite CardArt => cardArt;

}
