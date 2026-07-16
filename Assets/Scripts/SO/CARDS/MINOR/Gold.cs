using UnityEngine;

[CreateAssetMenu(fileName = "Gold", menuName = "CARDS/MINOR/Gold")]
public class Gold : Minor
{
    [SerializeField] int goldGained;
    [SerializeField] int armorGained;
    public override void PlayCard(PlayerController player, EnemyController targetEnemy = null)
    {
        if(player != null)
        {
            EncounterManager.Instance.AddWinGold(goldGained);
            player.AddArmor(armorGained);
        }
    }
}
