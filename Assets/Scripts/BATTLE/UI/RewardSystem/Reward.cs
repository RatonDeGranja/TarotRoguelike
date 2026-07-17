using UnityEngine;
using UnityEngine.UI;


public abstract class Reward : ScriptableObject
{
    [SerializeField] private string text;
    public string Text => text;

    [SerializeField] private Sprite image;
    public Sprite Image => Image;

    public abstract void GetReward(GameObject button);
}
