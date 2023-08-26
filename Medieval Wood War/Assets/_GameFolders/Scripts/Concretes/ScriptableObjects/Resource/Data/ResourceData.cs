using UnityEngine;

namespace HappyHour.Abstract
{
    [CreateAssetMenu(menuName = "HappyHour/ResourceData")]
    public class ResourceData : ScriptableObject
    {
        public ResourceInventory resourceInventory;
    }
}