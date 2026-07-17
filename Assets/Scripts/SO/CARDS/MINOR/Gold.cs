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
    public override string GetFormattedDescription()
    {
        string rawText = LocalizationManager.Instance.GetText(CardDescription);
        // Le pasamos las variables en el mismo orden que queremos que aparezcan en los huecos {0} y {1}
        return string.Format(rawText, goldGained, armorGained); 
    }
}
