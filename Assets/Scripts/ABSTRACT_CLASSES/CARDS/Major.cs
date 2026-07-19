using UnityEngine;

public abstract class Major : Card
{
    // Conecta la pasiva a los eventos globales del juego
    public void SubscribePassive()
    {
        // Se des
        GameEvents.onTurnStart -= ApplyPassive;
        GameEvents.onTurnStart += ApplyPassive;
    }
    
    // Desconecta la pasiva (Vital para evitar memory leaks o al Invertir el Arcano)
    public void UnsubscribePassive()
    {
        GameEvents.onTurnStart -= ApplyPassive;
    }


    public abstract void ActiveEffect();
    public abstract void ApplyPassive();
}
