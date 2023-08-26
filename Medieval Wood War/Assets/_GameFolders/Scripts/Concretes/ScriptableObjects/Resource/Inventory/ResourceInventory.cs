using UnityEngine;

namespace HappyHour.Abstract
{
    [CreateAssetMenu(menuName = "HappyHour/ResourceInventory")]
    public class ResourceInventory : ScriptableObject
    {
        public int wood;

        public void AddResource(int amount)
        {
            wood += amount;
        }

        public bool RemoveResource(int amount)
        {
            if(wood >= amount)
            {
                wood -= amount;
                return true;
            }
            return false;
        }
    }
}