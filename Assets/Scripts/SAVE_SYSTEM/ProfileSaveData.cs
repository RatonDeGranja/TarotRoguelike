using System.Collections.Generic;

[System.Serializable]
public class ProfileSaveData
{
    // Arcanos Mayores que has desbloqueado para elegir al inicio 
    // (Tu objetivo secundario del juego es desbloquearlos todos)
    public List<string> unlockedMajorArcanaIDs; 
    
    // Cartas bendecidas de los Arcanos que has ido desbloqueando al avanzar
    public List<string> unlockedBlessedCardIDs;

    // Logros del juego
    public bool hasBeatenGameOnce; 
    
    // Opciones del jugador para que no tenga que reconfigurarlas
    public float masterVolume;
    public bool isFullScreen;
}