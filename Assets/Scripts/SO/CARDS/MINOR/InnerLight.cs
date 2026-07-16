using UnityEngine;

[CreateAssetMenu(fileName = "InnerLight", menuName = "CARDS/MINOR/HERMIT/InnerLight")]
public class InnerLight : Minor
{
    [SerializeField] int gainedWisdom;
    public override void PlayCard(PlayerController player, EnemyController targetEnemy = null)
    {
        if(player != null)
        {
            player.GainWisdom(gainedWisdom);
        }
    }
}
