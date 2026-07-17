using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardScreenManager : MonoBehaviour
{

    [Header("Referencias")]
    [SerializeField] private GameObject rewardButtonPrefab; // Arrastra tu prefab del botón aquí
    private GameObject visualPanel;
    private Transform rewardsContainer;

    private void Start()
    {
        visualPanel = gameObject.transform.GetChild(0).gameObject;
        rewardsContainer = visualPanel.transform;
        visualPanel.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.onBattleWon += OnBattleWon;
    }
    private void OnDisable()
    {
        GameEvents.onBattleWon -= OnBattleWon;
    }

    private void OnBattleWon()
    {
        visualPanel.SetActive(true);

        foreach(Reward rew in EncounterManager.Instance.Rewards)
        {
            GameObject nuevoBoton = Instantiate(rewardButtonPrefab, rewardsContainer);
            nuevoBoton.GetComponent<RewardButtonUI>().Setup(rew);
        }

    }

}
