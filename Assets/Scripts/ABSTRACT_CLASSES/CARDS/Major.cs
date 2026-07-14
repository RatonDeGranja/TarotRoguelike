using UnityEngine;

public abstract class Major : Card
{
    // Conecta la pasiva a los eventos globales del juego
    public abstract void SubscribePassive(); 
    
    // Desconecta la pasiva (Vital para evitar memory leaks o al Invertir el Arcano)
    public abstract void UnsubscribePassive();


    public abstract void ActiveEffect();
}
