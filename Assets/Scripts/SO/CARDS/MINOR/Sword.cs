using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "CARDS/MINOR/Sword")]
public class Sword : Minor
{
    [SerializeField] int damage;
    public override void PlayCard(PlayerController player, EnemyController targetEnemy = null)
    {
        // Verificamos que se haya seleccionado un enemigo (puesto que su TargetType será ToEnemy)
        if (targetEnemy != null)
        {
            targetEnemy.takeDamage(damage);
            Debug.Log($"La carta {CardName} ha infligido {damage} de daño.");
        }
        else 
        {
            Debug.LogWarning("No hay enemigo seleccionado para "+CardName);
        }
    }

    public override string GetFormattedDescription()
    {
        string rawText = LocalizationManager.Instance.GetText(CardDescription);
        // Le pasamos las variables en el mismo orden que queremos que aparezcan en los huecos {0} y {1}
        return string.Format(rawText, damage); 
    }
}
