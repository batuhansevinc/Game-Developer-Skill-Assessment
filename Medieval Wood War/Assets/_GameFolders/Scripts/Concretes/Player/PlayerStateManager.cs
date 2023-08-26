using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HappyHour.Concretes.Managers
{
    public class PlayerStateManager : MonoSingleton<PlayerStateManager>
    {

        public enum PlayerState
        {
            Idle,
            Moving,
            Digging
        };
        public enum Event
        {
            Enter,
            Update,
            Exit
        };

        public PlayerState playerState;
        protected Event _event;

        public PlayerStateManager()
        {
            _event = Event.Enter;
        }

        public virtual void Enter() { _event = Event.Update; }
        public virtual void Update() { _event = Event.Update; }
        public virtual void Exit() { _event = Event.Exit; }

        public PlayerStateManager Process()
        {
            if (_event == Event.Enter)
            {
                Enter();
            }
            if (_event == Event.Update)
            {
                Update();
            }
            if (_event == Event.Exit)
            {
                Exit();
                return this;
            }

            return this;
        }
    }
}
