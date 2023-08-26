using UnityEngine;

[CreateAssetMenu(menuName = "HappyHour/PlayerData")]
public class PlayerData : ScriptableObject
{
    public GameObject workerPrefab;
    public PlayerBase playerBase;
    public PlayerInventory playerInventory;
    public PlayerStats playerStats;
}