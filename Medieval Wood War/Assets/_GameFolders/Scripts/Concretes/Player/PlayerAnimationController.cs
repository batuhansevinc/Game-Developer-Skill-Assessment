using HappyHour.Concretes.Managers;
using UnityEngine;

namespace HappyHour.Concretes.Controllers
{
    public class PlayerAnimationController : MonoSingleton<PlayerAnimationController>
    {
        public Animator _animator;
        Vector3 _moveDirection;
        Vector3 _lastMoveDirection;
        private void Awake()
        {
            _animator = GetComponent<Animator>();

        }

        void Idle()
        {
            PlayerAnimationController.Instance._animator.SetBool("isMoving", false);
        }
        private void Moving()
        {
            if (PlayerStateManager.Instance.playerState == PlayerStateManager.PlayerState.Moving)
            {
                PlayerAnimationController.Instance._animator.SetFloat("horizontalMovement", _moveDirection.x);
                PlayerAnimationController.Instance._animator.SetFloat("verticalMovement", _moveDirection.y);
                PlayerAnimationController.Instance._animator.SetBool("isMoving", true);

            }
            
        }
    }
}
