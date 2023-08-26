using BatuhanSevinc.Helpers;
using BatuhanSevinc.ScriptableObjects;
using UnityEngine;

namespace PizzaGuy.Uis
{
    public class CanvasGroupController : MonoBehaviour
    {
        [SerializeField] CanvasGroup _canvasGroup;

        public bool IsOpen { get; private set; }

        void OnValidate()
        {
            this.GetReference(ref _canvasGroup);
        }

        public void UpdateCanvasGroup()
        {
            _canvasGroup.alpha = (_canvasGroup.alpha == 0) ? 1f : 0f;
            _canvasGroup.interactable = !_canvasGroup.interactable;
            _canvasGroup.blocksRaycasts = !_canvasGroup.blocksRaycasts;
        }
    }
}