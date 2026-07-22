using UnityEngine;

[CreateAssetMenu(fileName = "EchoesOfTime", menuName = "CARDS/MINOR/HERMIT/Echoes Of Time")]
public class EchoesTime : Minor
{
    public override void PlayCard(PlayerController player, EnemyController targetEnemy = null)
    {
        if(player != null)
        {
            if(BattleManager.Instance.LastCardPlayer != null)
            {
                DeckManager.Instance.AddGeneratedCardToHand(BattleManager.Instance.LastCardPlayer);
            }
        }
    }
}
