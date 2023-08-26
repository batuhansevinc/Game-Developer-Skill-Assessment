using BatuhanSevinc.ScriptableObjects;
using UnityEngine;

namespace BatuhanSevinc.Uis
{
    public class BaseButtonWithGameEvents : BaseButton
    {
        [SerializeField] protected GameEvent _buttonEvent;

        protected override void HandleOnButtonClicked()
        {
            _buttonEvent.InvokeEvents();
        }
    }
}