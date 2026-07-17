using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardButtonUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;

    public void Setup(Reward rewardData)
    {
        nameText.text = rewardData.Text;
        iconImage.sprite = rewardData.Image;
        button.onClick.AddListener(() => rewardData.GetReward(gameObject));
    }
}