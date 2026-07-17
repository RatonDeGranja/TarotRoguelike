using UnityEngine;

[CreateAssetMenu(fileName = "Gold", menuName = "REWARDS/Gold")]
public class Reward_Gold : Reward
{
    [SerializeField] private int gold;
    public int Gold => gold;

    public override void GetReward(GameObject button)
    {
        PlayerController.Player.GainGold(gold);

        button.SetActive(false);
        Destroy(button);
    }
}