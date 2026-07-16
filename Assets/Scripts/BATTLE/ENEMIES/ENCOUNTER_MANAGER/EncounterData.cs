using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "COMBAT/Encounter")]
public class EncounterData : ScriptableObject
{
    [SerializeField] private int victoryGold;
    public int VictoryGold => victoryGold;

    public Enemy[] leftEnemies;  // Enemigos que spawnean a la izquierda
    public Enemy[] rightEnemies; // Enemigos que spawnean a la derecha
}