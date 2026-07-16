using UnityEngine;

[CreateAssetMenu(fileName = "Patience", menuName = "CARDS/MINOR/HERMIT/Patience")]
public class Patience : Minor
{
    [SerializeField] int gainedWisdom;
    public override void PlayCard(PlayerController player, EnemyController targetEnemy = null)
    {
        if(player != null)
        {
            player.GainWisdom(gainedWisdom);
            BattleManager.Instance.EndPlayerTurn();
        }
    }
}
