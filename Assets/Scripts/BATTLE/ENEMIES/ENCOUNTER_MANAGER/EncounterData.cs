using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "COMBAT/Encounter")]
public class EncounterData : ScriptableObject
{
    public Enemy[] leftEnemies;  // Enemigos que spawnean a la izquierda
    public Enemy[] rightEnemies; // Enemigos que spawnean a la derecha
}