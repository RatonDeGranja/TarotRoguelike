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

    public override string GetFormattedDescription()
    {
        string rawText = LocalizationManager.Instance.GetText(CardDescription);
        // Le pasamos las variables en el mismo orden que queremos que aparezcan en los huecos {0} y {1}
        return string.Format(rawText, gainedWisdom); 
    }
}
