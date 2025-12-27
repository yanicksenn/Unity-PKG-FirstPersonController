using System.Collections.Generic;
using UnityEngine;
using YanickSenn.Utils.Control;

namespace YanickSenn.Controller.FirstPerson
{
    [DisallowMultipleComponent]
    public abstract class AbstractPlayerController<TMover, TMoverConfig> : MonoBehaviour 
            where TMover : AbstractMover<TMoverConfig> 
            where TMoverConfig : IMoverConfig {
        private readonly Stack<IPlayerState> _stateStack = new();

        private IPlayerState _defaultPlayerState;
        private IPlayerState _currentPlayerState;

        public Optional<Looker> Looker => GetLooker();
        public Optional<Hand.Hand> Hand => GetHand();
        public Optional<TMover> Mover => GetMover();
        public IPlayerState CurrentPlayerState => _currentPlayerState;

        private void Awake() {
            _defaultPlayerState = GetDefaultPlayerState();
            _currentPlayerState = _defaultPlayerState;
        }

        protected abstract Optional<Looker> GetLooker();
        protected abstract Optional<Hand.Hand> GetHand();
        protected abstract Optional<TMover> GetMover();
        protected abstract IPlayerState GetDefaultPlayerState();

        private void OnEnable() {
            _currentPlayerState.Enable();
        }

        private void OnDisable() {
            _currentPlayerState.Disable();
        }

        private void Update() {
            _currentPlayerState.Update();
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
