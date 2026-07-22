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

    public override string GetFormattedDescription()
    {
        string rawText = LocalizationManager.Instance.GetText(CardDescription);
        // Le pasamos las variables en el mismo orden que queremos que aparezcan en los huecos {0} y {1}
        return string.Format(rawText, gainedWisdom); 
    }
}
