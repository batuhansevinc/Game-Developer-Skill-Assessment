using UnityEngine;

[CreateAssetMenu(menuName = "HappyHour/PlayerInventory")]
public class PlayerInventory : ScriptableObject
{
    public int wood;

    public void AddWood(int amount)
    {
        wood += amount;
    }

    public void RemoveWood(int amount)
    {
        wood -= amount;
        if(wood < 0)
        {
            wood = 0;
        }
    }
}