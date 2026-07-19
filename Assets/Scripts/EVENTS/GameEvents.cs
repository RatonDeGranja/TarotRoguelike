using System;

public static class GameEvents
{
    //BATTLE EVENTS
    public static Action onTurnStart;
    public static Action onTurnEnd;
    public static Action<Card> onCardPlayed;
    public static Action<Card> onCardDrawn;
    public static Action<Card> onCardDiscarded;
    public static Action onBattleWon;
    public static Action onNextWaveTriggered;
    
    public static Action onLanguageChanged;

    //MAJORES
    public static Action onMajorInverted;

    //PLAYER STATS CHANGED
    public static Action<int, int> onPlayerHealthChanged; // (vidaActual, vidaMaxima)
    public static Action<int> onPlayerWisdomChanged;      // (sabiduriaActual)
    public static Action<int> onPlayerGoldChanged;        // (oroActual)
    public static Action<int> onPlayerArmorChanged;       // (armaduraActual)



    //ENEMY EVENTS
    public static Action<EnemyController, int, int> onEnemyHealthChanged; // (enemigo, vidaAct, vidaMax)
    public static Action<EnemyController> onEnemySpawned; // Para avisar a la UI que cree su barra
    public static Action<EnemyController> onEnemyDied;    // Para avisar a la UI que limpie su barra
}
