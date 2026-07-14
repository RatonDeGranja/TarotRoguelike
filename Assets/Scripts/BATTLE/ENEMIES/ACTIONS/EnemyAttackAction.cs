using UnityEngine;

[CreateAssetMenu(fileName = "NewSimpleAttack", menuName = "ENEMY/ACTIONS/Simple Attack")]
public class EnemyAttackAction : EnemyActionSO
{
    [Header("Configuración del Ataque")]
    [SerializeField] private int damage;

    public override void Execute(EnemyController enemy, PlayerController player)
    {
        Debug.Log($"{enemy.name} usa {ActionName} y ataca por {damage} de daño.");
        
        // Llamamos al método exacto que definiste en tu nuevo PlayerController
        player.takeDamage(damage); 
    }
}