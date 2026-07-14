using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    
    [SerializeField] private  string enemyName;
    [SerializeField] private int maxLife;
    [SerializeField] private Sprite artwork;


    public List<EnemyActionSO> attackPatterns;


    public string Name => enemyName;
    public int MaxLife => maxLife;
    public Sprite Artwork => artwork;

}