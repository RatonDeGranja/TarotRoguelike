using System.Collections.Generic;
using UnityEngine;

public class CombatTesterManager : MonoBehaviour
{
    [Header("Oleadas de Prueba")]
    [SerializeField] private List<EncounterData> testEncounters;
    private int currentIndex = 0;

    private void Start()
    {
        // Al darle al Play, empezamos automáticamente la primera oleada
        if (testEncounters.Count > 0)
        {
            StartWave(0);
        }
    }

    private void OnEnable()
    {
        // Escucharemos un grito para pasar a la siguiente oleada
        GameEvents.onNextWaveTriggered += LoadNextWave;
    }

    private void OnDisable()
    {
        GameEvents.onNextWaveTriggered -= LoadNextWave;
    }

    public void LoadNextWave()
    {
        currentIndex++;

        if (currentIndex < testEncounters.Count)
        {
            Debug.Log($"<color=cyan>Iniciando oleada de prueba {currentIndex + 1}...</color>");
            
            // 1. Curamos al jugador al máximo para la siguiente prueba
            if (PlayerController.Player != null)
            {
                // Asumiendo que tienes un método heal. Si no, ponle la vida a tope directamente.
                PlayerController.Player.heal(); 
            }

            StartWave(currentIndex);
        }
        else
        {
            Debug.Log("<color=green>¡HAS COMPLETADO TODAS LAS OLEADAS DE PRUEBA!</color>");
        }
    }

    private void StartWave(int index)
    {
        EncounterData nextEncounter = testEncounters[index];
        
        // Le ordenamos a la fábrica que cree a los enemigos
        EncounterManager.Instance.SpawnEncounter(nextEncounter);
        
        // IMPORTANTE: Le decimos al BattleManager que empiece el combate de nuevo
        // (Robar cartas iniciales, reiniciar maná, etc.)
        BattleManager.Instance.StartBattle(); 
    }
}