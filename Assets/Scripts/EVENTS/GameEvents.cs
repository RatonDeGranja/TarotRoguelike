using System;

public static class GameEvents
{
    //BATTLE EVENTS
    public static Action onTurnStart;
    public static Action<Card> onCardPlayed;
    public static Action<Card> onCardDrawn;
    public static Action onBattleWon;

    //MAJORES
    public static Action onMajorInverted;

    //PLAYER STATS CHANGED
    public static Action<int, int> onPlayerHealthChanged; // (vidaActual, vidaMaxima)
    public static Action<int> onPlayerWisdomChanged;      // (sabiduriaActual)

    //ENEMY EVENTS
    public static Action<EnemyController, int, int> onEnemyHealthChanged; // (enemigo, vidaAct, vidaMax)
    public static Action<EnemyController> onEnemySpawned; // Para avisar a la UI que cree su barra
    public static Action<EnemyController> onEnemyDied;    // Para avisar a la UI que limpie su barra
}
