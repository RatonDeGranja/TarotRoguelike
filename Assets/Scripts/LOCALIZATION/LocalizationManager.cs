using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    public enum Language { ES, EN , VAL}
    
    [Header("Configuración")]
    public Language currentLanguage = Language.ES;
    
    [SerializeField] private TextAsset[] localizationFiles; 

    private Dictionary<string, Dictionary<Language, string>> translationDatabase = new Dictionary<string, Dictionary<Language, string>>();

    private void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this; 
            DontDestroyOnLoad(gameObject); 
            LoadAllCSVs(); 
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void LoadAllCSVs()
    {
        // Recorremos todos los archivos CSV
        foreach (TextAsset csv in localizationFiles)
        {
            if (csv == null) continue;

            string[] rows = csv.text.Split('\n');
            
            for (int i = 1; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split(';');
                
                if (columns.Length >= 3)
                {
                    string key = columns[0].Trim();
                    string textES = columns[1].Trim();
                    string textEN = columns[2].Trim();
                    string textVAL = columns[2].Trim();

                    var languageData = new Dictionary<Language, string>();
                    languageData[Language.ES] = textES;
                    languageData[Language.EN] = textEN;
                    languageData[Language.VAL] = textVAL;

                    // Todos los textos van a la misma base de datos, vengan del CSV que vengan
                    translationDatabase[key] = languageData;
                }
            }
        }
        Debug.Log($"Traducciones cargadas: {translationDatabase.Count} claves.");
    }

    public string GetText(string key)
    {
        if (translationDatabase.ContainsKey(key) && translationDatabase[key].ContainsKey(currentLanguage))
        {
            return translationDatabase[key][currentLanguage];
        }
        return key; 
    }
}