using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager Instance;



    [Header("Datos y Prefabs")]
    [SerializeField] private EncounterData encounterData; 
    [SerializeField] private GameObject enemyDisplay; 
    [SerializeField] private List<Reward> rewards = new List<Reward>();
    public List<Reward> Rewards => rewards; 

    private int winGold;
    public int WinGold => winGold;

    [Header("Posiciones en la escena")]
    [SerializeField] private Transform[] leftSlots;  // Los puntos vacíos a la izquierda
    [SerializeField] private Transform[] rightSlots; // Los puntos vacíos a la derecha
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        winGold = encounterData.VictoryGold;

        for (int i = 0; i < encounterData.leftEnemies.Length; i++)
        {
            Enemy enemyData = encounterData.leftEnemies[i];

            if (enemyData != null && i < leftSlots.Length)
            {
                // 1. Instanciamos el PREFAB, en la posición del slot
                GameObject newEnemyObj = Instantiate(enemyDisplay, leftSlots[i].position, Quaternion.identity, leftSlots[i]);

                // 2. Cogemos su EnemyController
                EnemyController controller = newEnemyObj.GetComponent<EnemyController>();

                // 3. Le inyectamos los DATOS y le decimos su lado
                if (controller != null)
                {
                    controller.Initialize(enemyData, FieldSide.Left);
                }
            }
        }

        for (int i = 0; i < encounterData.rightEnemies.Length; i++)
        {
            Enemy enemyData = encounterData.rightEnemies[i];

            if (enemyData != null && i < rightSlots.Length)
            {
                GameObject newEnemyObj = Instantiate(enemyDisplay, rightSlots[i].position, Quaternion.identity, rightSlots[i]);
                EnemyController controller = newEnemyObj.GetComponent<EnemyController>();

                if (controller != null)
                {
                    controller.Initialize(enemyData, FieldSide.Right);
                }
            }
        }
    }
    public void AddWinGold(int gold)
    {
        winGold += gold;
    }
}
