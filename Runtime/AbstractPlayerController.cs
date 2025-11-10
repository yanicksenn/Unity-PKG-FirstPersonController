using System;
using System.Collections.Generic;
using UnityEngine;

namespace YanickSenn.Controller.FirstPerson
{
    [DisallowMultipleComponent, 
     RequireComponent(typeof(Looker)),
     RequireComponent(typeof(Hand)),
     RequireComponent(typeof(AbstractMover))]
    public abstract class AbstractPlayerController : MonoBehaviour {
        private readonly Stack<IPlayerState> _stateStack = new();

        private Looker _looker;
        private Hand _hand;
        private AbstractMover _abstractMover;
        private IPlayerState _defaultPlayerState;
        private IPlayerState _currentPlayerState;

        private void Awake() {
            _looker = GetComponent<Looker>();
            _hand = GetComponent<Hand>();
            _abstractMover = GetComponent<AbstractMover>();
            _defaultPlayerState = GetDefaultPlayerState();
            _currentPlayerState = _defaultPlayerState;
        }

        protected abstract IPlayerState GetDefaultPlayerState();

        private void OnEnable() {
            _currentPlayerState.Enable();
        }

        private void OnDisable() {
            _currentPlayerState.Disable();
        }

        private void FixedUpdate() {
            _currentPlayerState.FixedUpdate();
        }

        private void OnDrawGizmos() {
            _currentPlayerState?.OnDrawGizmos();
        }

        private void OnDrawGizmosSelected() {
            _currentPlayerState?.OnDrawGizmosSelected();
        }

        public void PushState(IPlayerState newState) {
            if (newState == null) return;
            _stateStack.Push(newState);
            _currentPlayerState.Disable();
            _currentPlayerState = newState;
            newState.Enable();
        }

        public void PopState() {
            if (_stateStack.Count == 0) return;
            var previousState = _stateStack.Pop();
            previousState.Disable();
            _currentPlayerState = _stateStack.Count == 0 ? _defaultPlayerState : _stateStack.Peek();
            _currentPlayerState.Enable();
        }
    }
}
