using UnityEngine;

[CreateAssetMenu(fileName = "Cup", menuName = "CARDS/MINOR/Cup")]
public class Cup : Minor
{
    [SerializeField] int lifeGained;
    public override void PlayCard(PlayerController player, EnemyController targetEnemy = null)
    {
        if(player != null)
        {
            player.heal(lifeGained);
        }
    }
}
