using UnityEngine;
using TMPro;

public class WisdomText : MonoBehaviour
{
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        GameEvents.onPlayerWisdomChanged += UpdateUI;
    }

    private void OnDisable()
    {
        GameEvents.onPlayerWisdomChanged -= UpdateUI;
    }
    
    private void UpdateUI(int wisdom)
    {
        text.text = $"{wisdom}/{PlayerController.Player.StartWisdom}";
    }
}
