using UnityEngine;

namespace YanickSenn.Controller.FirstPerson {
    [DisallowMultipleComponent, RequireComponent(typeof(Rigidbody))]
    public class Groundable : MonoBehaviour {
        public GroundedCheckType groundedCheckType = GroundedCheckType.Raycast;
        
        private Collider _collider;
        private Rigidbody _rigidbody;
        
        private bool _isGrounded;
        private Rigidbody _currentPlatform;

        public bool IsGrounded => _isGrounded;

        private void Awake() {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            UpdateGroundedState();
        }

        private void UpdateGroundedState() {
            _isGrounded = groundedCheckType switch {
                GroundedCheckType.Velocity => IsGroundedVelocity(),
                GroundedCheckType.Raycast => IsGroundedRaycast(),
                _ => _isGrounded
            };
        }

        private bool IsGroundedRaycast() {
            float height = _collider.bounds.size.y;
            Vector3 rayStart = transform.position;
            return Physics.Raycast(rayStart, Vector3.down, height / 2 + 0.05f);
        }

        private bool IsGroundedVelocity() {
            return Mathf.Abs(_rigidbody.linearVelocity.y) < Mathf.Epsilon;
        }

        private bool IsGroundedCollision() {
            return _currentPlatform != null;
        }

        public enum GroundedCheckType {
            Velocity,
            Raycast,
        }
    }
}