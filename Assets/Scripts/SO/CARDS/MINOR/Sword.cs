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
            // targetEnemy.takeDamage(damageAmount); <-- Cuando tengas la clase EnemyController lista
            Debug.Log($"La carta {CardName} ha infligido {damage} de daño.");
        }
        else 
        {
            Debug.LogWarning("No hay enemigo seleccionado para "+CardName);
        }
    }
}
