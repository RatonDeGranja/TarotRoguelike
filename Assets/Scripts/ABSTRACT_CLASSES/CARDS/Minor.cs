using UnityEngine;

public abstract class Minor : Card
{    
    [Header("Reglas de la Carta")]
    [SerializeField] private int wisdomCost;
    [SerializeField] private int minWisdomRequired;
    [SerializeField] private TargetType target;
    

    // Propiedades de lectura
    public int WisdomCost => wisdomCost;
    public int MinWisdomRequired => minWisdomRequired;
    public TargetType Target => target;

    // AL inyectar los controladores como parámetros, la carta sabe exactamente a quién afectar
    // targetEnemy puede ser nulo si la carta es de curación y solo afecta al jugador.
    public abstract void PlayCard(PlayerController player, EnemyController targetEnemy = null);
}

public enum TargetType
{
    ToPlayer,
    ToEnemy
}