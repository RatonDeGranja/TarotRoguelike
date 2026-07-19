using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "COMBAT/Encounter")]
public class EncounterData : ScriptableObject
{
    [SerializeField] private List<Reward> rewards = new List<Reward>();
    public List<Reward> Rewards => rewards; 

    public Enemy[] leftEnemies;  // Enemigos que spawnean a la izquierda
    public Enemy[] rightEnemies; // Enemigos que spawnean a la derecha

    public int getGold()
    {
        foreach(Reward reward in rewards)
        {
            if(reward is Reward_Gold gold)
            {
                return gold.Gold;
            
            }
        }

        return 10;
    }

    public int VictoryGold => getGold();
}