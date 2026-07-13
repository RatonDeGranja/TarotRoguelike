using UnityEngine;

[CreateAssetMenu(fileName = "Hermit", menuName = "CARDS/MAJOR/Hermit")]
public class Hermit : Major
{

    public override void activeEffect()
    {
        Debug.Log("Hermit Active Effect");  
    }

    public override void SubscribePassive()
    {
        // Nos suscribimos al grito global de "¡Empieza el turno!"
        //GameEvents.onTurnStart += ApplyPassive;
    }

    public override void UnsubscribePassive()
    {
        // Obligatorio: nos desuscribimos para que no robe cartas si el Arcano se invierte
        //GameEvents.onTurnStart -= ApplyPassive;
    }

    private void ApplyPassive()
    {
        Debug.Log("Pasiva del Ermitaño: Robando carta extra...");
        // Aquí llamarás a tu Gestor de Cartas
        // DeckManager.Instance.DrawCard();
        // BattleManager.Instance.StartDiscardPhase();
    }
}
