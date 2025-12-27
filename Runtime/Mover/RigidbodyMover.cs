using System;
using UnityEngine;
using YanickSenn.Utils.Variables;

namespace YanickSenn.Controller.FirstPerson.Mover
{
    [DisallowMultipleComponent,
     RequireComponent(typeof(Rigidbody)),
     RequireComponent(typeof(Groundable)),
     RequireComponent(typeof(Looker))]
    public class RigidbodyMover : AbstractMover<RigidbodyMover.RigidbodyMoverConfig> {
        
        private Rigidbody _rigidbody;
        private Groundable _groundable;
        private Looker _looker;
        private bool _isRunning;

        public override bool IsGrounded => _groundable.IsGrounded;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
            _groundable = GetComponent<Groundable>();
            _looker = GetComponent<Looker>();

            _rigidbody.linearDamping = 10f;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        private void FixedUpdate() {
            switch (MoverConfig) {
                case WalkingConfig walkingStrategy:
                    ApplyWalkingStrategy(walkingStrategy);
                    break;
            }
        }

        private void ApplyWalkingStrategy(WalkingConfig walkingConfig) {
            if (IsGrounded) {
                _rigidbody.linearDamping = 10f;
                ApplyMovementInDirection(walkingConfig);
            } else {
                _rigidbody.linearDamping = 0f;
            }
        }

        public override void Jump() {
            switch (MoverConfig) {
                case WalkingConfig walkingStrategy:
                    ApplyJump(walkingStrategy);
                    break;
            }
        }

        private void ApplyJump(WalkingConfig walkingConfig) {
            if (!IsGrounded) return;
            _rigidbody.AddRelativeForce(Vector3.up * walkingConfig.jumpForce.Value);
        }

        private void ApplyMovementInDirection(WalkingConfig walkingConfig) {
            var lookDirection = _looker.LookDirection;
            var forward = new Vector3(lookDirection.x, 0f, lookDirection.z).normalized;
            var right = new Vector3(forward.z, 0, -forward.x);
            var moveDirection = (forward * MoveInput.y + right * MoveInput.x).normalized;
            var translatedMoveDirection = transform.InverseTransformDirection(moveDirection);
            var moveForce = IsRunning ? walkingConfig.runningForce.Value : walkingConfig.walkingForce.Value;
            _rigidbody.AddRelativeForce(moveForce * translatedMoveDirection);
        }

        public interface RigidbodyMoverConfig : IMoverConfig {
            
        }
        
        [Serializable]
        public class WalkingConfig : RigidbodyMoverConfig {
            public FloatReference walkingForce = new(3500f);
            public FloatReference runningForce = new(5000f);
            public FloatReference jumpForce = new(15000f);
        }
    }
}