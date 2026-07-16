using Unity.VisualScripting;
using UnityEngine;

public abstract class Minor : Card
{    
    [Header("Reglas de la Carta")]
    [SerializeField] private int wisdomCost;
    [SerializeField] private int minWisdomRequired;
    [SerializeField] private TargetType target;
    [SerializeField] private CardType type;

    [Header("Propiedades Especiales")]
    [Tooltip("Si es falso, esta carta no se guardará como la última jugada (Ej: Ecos del Tiempo)")]
    [SerializeField] private bool canBeSavedAsLast = true; 

    // Propiedad pública para leerla
    public bool CanBeSavedAsLast => canBeSavedAsLast;
    

    // Propiedades de lectura
    public int WisdomCost => wisdomCost;
    public int MinWisdomRequired => minWisdomRequired;
    public TargetType Target => target;
    public CardType Type => type;

    // AL inyectar los controladores como parámetros, la carta sabe exactamente a quién afectar
    // targetEnemy puede ser nulo si la carta es de curación y solo afecta al jugador.
    public abstract void PlayCard(PlayerController player, EnemyController targetEnemy = null);
}

public enum TargetType
{
    ToPlayer,
    ToEnemy
}

public enum CardType
{
    Attack,
    Buff,
    Debuff
}
