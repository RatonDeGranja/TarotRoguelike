using UnityEngine;
using System.IO;

public static class SaveManager
{
    // Ruta donde se guardará el archivo en el ordenador del jugador
    // persistentDataPath es una carpeta segura que Unity crea automáticamente (en Windows suele estar en AppData)
    private static string runSavePath = Application.persistentDataPath + "/run_save.json";
    private static string profileSavePath = Application.persistentDataPath + "/profile_save.json";

    // --- GUARDAR ---
    public static void SaveRun(GameSaveData dataToSave)
    {
        // 1. Convertimos los datos a un texto JSON
        string json = JsonUtility.ToJson(dataToSave, true); // El 'true' lo hace bonito y legible

        // 2. Escribimos ese texto en el disco duro
        File.WriteAllText(runSavePath, json);

        Debug.Log("Partida guardada con éxito en: " + runSavePath);
    }
    public static void SaveProfile(ProfileSaveData dataToSave)
    {
        // 1. Convertimos los datos a un texto JSON
        string json = JsonUtility.ToJson(dataToSave, true); // El 'true' lo hace bonito y legible

        // 2. Escribimos ese texto en el disco duro
        File.WriteAllText(profileSavePath, json);

        Debug.Log("Partida guardada completa con éxito en: " + profileSavePath);
    }

    // --- CARGAR ---
    public static GameSaveData LoadRun()
    {
        // 1. Comprobamos si el archivo existe antes de intentar leerlo
        if (File.Exists(runSavePath))
        {
            // 2. Leemos el texto del archivo
            string json = File.ReadAllText(runSavePath);

            // 3. Convertimos el JSON de vuelta a nuestro objeto GameSaveData
            GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(json);

            Debug.Log("Partida cargada con éxito.");
            return loadedData;
        }
        else
        {
            Debug.LogWarning("No se encontró ningún archivo de guardado.");
            return null; // Devolvemos nulo si es una partida nueva
        }
    }

    public static ProfileSaveData LoadProfile()
    {
        // 1. Comprobamos si el archivo existe antes de intentar leerlo
        if (File.Exists(profileSavePath))
        {
            // 2. Leemos el texto del archivo
            string json = File.ReadAllText(profileSavePath);

            // 3. Convertimos el JSON de vuelta a nuestro objeto ProfileSaveData
            ProfileSaveData loadedData = JsonUtility.FromJson<ProfileSaveData>(json);

            Debug.Log("Partida cargada con éxito.");
            return loadedData;
        }
        else
        {
            Debug.LogWarning("No se encontró ningún archivo de guardado.");
            return null; // Devolvemos nulo si es una partida nueva
        }
    }

    // --- COMPROBAR SI HAY PARTIDA --- (Útil para el Menú Principal)
    public static bool HasSaveRun()
    {
        return File.Exists(runSavePath);
    }

    public static bool HasSaveProfile()
    {
        return File.Exists(profileSavePath);
    }


    // --- BORRAR PARTIDA --- (Útil para cuando el jugador muere en el Roguelike)
    public static void DeleteSaveFile()
    {
        if (File.Exists(runSavePath))
        {
            File.Delete(runSavePath);
            Debug.Log("Archivo de guardado borrado (Fin de la run).");
        }
    }
}