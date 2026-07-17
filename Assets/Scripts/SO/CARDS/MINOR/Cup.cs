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

    //Para inyectar la variable en la traduccion
    public override string GetFormattedDescription()
    {
        string rawText = LocalizationManager.Instance.GetText(CardDescription); // "Cura {0} de vida."
        return string.Format(rawText, lifeGained); // Resultado: "Cura 10 de vida."
    }
}
