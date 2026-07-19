using UnityEngine;

[CreateAssetMenu(fileName = "Meditation", menuName = "CARDS/MINOR/HERMIT/Meditation")]
public class Meditation : Minor
{
    [SerializeField] int cardsToDraw;
    public override void PlayCard(PlayerController player, EnemyController targetEnemy = null)
    {
        for(int i=0; i<cardsToDraw; i++)
        {
            DeckManager.Instance.DrawCard();
        }
        
        BattleManager.Instance.AddExtraTurn();
    }

    public override string GetFormattedDescription()
    {
        string rawText = LocalizationManager.Instance.GetText(CardDescription);
        // Le pasamos las variables en el mismo orden que queremos que aparezcan en los huecos {0} y {1}
        return string.Format(rawText, cardsToDraw); 
    }
}
