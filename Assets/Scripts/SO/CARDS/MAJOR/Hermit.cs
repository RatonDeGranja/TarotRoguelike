using UnityEngine;

[CreateAssetMenu(fileName = "Hermit", menuName = "CARDS/MAJOR/Hermit")]
public class Hermit : Major
{

    public override void ActiveEffect()
    {
        Debug.Log("Hermit Active Effect");  
    }

    public override void ApplyPassive()
    {
        Debug.Log("Pasiva del Ermitaño: Robando carta extra...");
        DeckManager.Instance.DrawCard();
        // BattleManager.Instance.StartDiscardPhase();
    }
}
