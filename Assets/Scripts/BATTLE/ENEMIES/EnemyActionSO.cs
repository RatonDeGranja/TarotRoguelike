using UnityEngine;

public abstract class EnemyActionSO : ScriptableObject
{
    [Header("Información de la Acción (Intent)")]
    [SerializeField] private string actionName;
    [SerializeField] private Sprite intentIcon; // ¡El icono que verá el jugador antes de que ocurra!

    public string ActionName => actionName;
    public Sprite IntentIcon => intentIcon;

    // La función que ejecuta la matemática real
    public abstract void Execute(EnemyController enemy, PlayerController player);
}