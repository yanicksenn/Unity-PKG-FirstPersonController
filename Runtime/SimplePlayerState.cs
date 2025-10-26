
using UnityEngine;
using UnityEngine.InputSystem;

namespace YanickSenn.Controller.FirstPerson
{
    public class SimplePlayerState : IPlayerState, InputSystemActions.IPlayerActions {
        private InputSystemActions _actions;
        private Looker _looker;
        private Hand _hand;
        private AbstractMover _mover;

        public SimplePlayerState(Looker looker,  Hand hand, AbstractMover mover) {
            _actions = new InputSystemActions();
            _looker = looker;
            _hand = hand;
            _mover = mover;
        }

        public void Enable() {
            _actions.Player.SetCallbacks(this);
            _actions.Player.Enable();
        }

        public void Disable() {
            _actions.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context) {
            _mover.MoveInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context) {
            _looker.LookInput = context.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext context) {
            _mover.IsRunning = context.ReadValueAsButton();
        }

        public void OnJump(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Started) {
                _mover.Jump();
            }
        }

        public void OnGrab(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Started) {
                _hand.Grab();
            }
        }

        public void OnRelease(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Started) {
                _hand.Release();
            }
        }

        public void OnThrow(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Started) {
                _hand.Throw();
            }
        }
    }
}