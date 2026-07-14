using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    // --- Estadísticas Base ---
    public int playerHealth;
    public int playerMaxHealth;
    public int playerGold;

    // --- Construcción del Mazo ---
    public string equippedMajorArcanaID;
    
    // Lista de IDs de las cartas actuales. 
    // Reflejará si el jugador compró cartas nuevas en la Tienda
    // o si decidió mejorar/destruir una carta al Rezar.
    public List<string> currentDeckCardIDs; 

    // --- Progresión del Mapa ---
    // Hay 4 niveles en el juego, cada uno con un boss
    public int currentLevel; 
    public string currentEventNodeID; // El ID del evento (Combate, Hoguera, etc.) en el que se encuentra.
}