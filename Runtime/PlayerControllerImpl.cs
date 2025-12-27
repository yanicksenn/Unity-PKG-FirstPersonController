using System.Collections.Generic;
using UnityEngine;

namespace YanickSenn.Controller.FirstPerson
{
    public class PlayerControllerImpl<TMover, TMoverConfig> 
            where TMover : AbstractMover<TMoverConfig> 
            where TMoverConfig : IMoverConfig {
        private readonly Stack<IPlayerState> _stateStack = new();

        private readonly Looker _looker;
        private readonly Hand.Hand _hand;
        private readonly TMover _mover;
        private readonly IPlayerState _defaultPlayerState;

        private IPlayerState _currentPlayerState;
        public IPlayerState CurrentPlayerState => _currentPlayerState;
        public Looker Looker => _looker;
        public Hand.Hand Hand => _hand;
        public TMover Mover => _mover;

        public PlayerControllerImpl(Looker looker, Hand.Hand hand, TMover mover, IPlayerState defaultPlayerState) {
            _looker = looker;
            _hand = hand;
            _mover = mover;
            _defaultPlayerState = defaultPlayerState;
            _currentPlayerState = _defaultPlayerState;
        }

        public void Start() {
            _currentPlayerState.Enable();
        }

        public void Update() {
            _currentPlayerState.Update();
        }

        public void FixedUpdate() {
            _currentPlayerState.FixedUpdate();
        }

        public void OnDrawGizmos() {
            _currentPlayerState?.OnDrawGizmos();
        }

        public void OnDrawGizmosSelected() {
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
