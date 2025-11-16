using UnityEngine;

namespace YanickSenn.Controller.FirstPerson
{
    [DisallowMultipleComponent, 
        RequireComponent(typeof(CharacterController)),
        RequireComponent(typeof(Looker))]
    public class CharacterControllerMover : AbstractMover
    {
        [SerializeField]
        private float walkingSpeed = 5f;

        [SerializeField]
        private float runningSpeed = 8f;

        [SerializeField]
        private float jumpHeight = 1.5f;
        
        [SerializeField]
        private float mass = 80f;

        private CharacterController _characterController;
        private Looker _looker;
        
        private Vector3 _horizontalVelocity;
        private Vector3 _velocity;

        public override bool IsGrounded => _characterController.isGrounded;

        private void Awake() {
            _characterController = GetComponent<CharacterController>();
            _looker = GetComponent<Looker>();
        }

        private void FixedUpdate() {
            var isGrounded = _characterController.isGrounded;
            if (isGrounded && _velocity.y < 0) {
                _velocity.y = -2f;
            }

            if (isGrounded) {
                var currentSpeed = IsRunning ? runningSpeed : walkingSpeed;
                var lookDirection = _looker.LookDirection;
                var forward = new Vector3(lookDirection.x, 0f, lookDirection.z).normalized;
                var right = new Vector3(forward.z, 0, -forward.x);
                var moveDirection = (forward * MoveInput.y + right * MoveInput.x).normalized;
                _horizontalVelocity = moveDirection * currentSpeed;
            }
    
            _velocity.y += Physics.gravity.y * Time.fixedDeltaTime;

            var finalVelocity = _horizontalVelocity + new Vector3(0, _velocity.y, 0);
            _characterController.Move(finalVelocity * Time.fixedDeltaTime);
        }
    
        private void OnControllerColliderHit(ControllerColliderHit hit) {
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic) {
                return;
            }

            // We don't want to push objects below us
            //if (hit.moveDirection.y < -0.3f) {
            //    return;
            //}

            // Apply the push force, scaled by our mass and the Rigidbody's mass
            // This makes the push feel more realistic
            Debug.DrawRay(hit.point, hit.moveDirection * mass, Color.red);
            body.AddForceAtPosition(hit.moveDirection * mass * hit.moveLength, hit.point, ForceMode.Force);
        }

        public override void Jump() {
            if (!IsGrounded) return;
            
            // Calculate the required upward velocity to reach the desired jumpHeight
            // This is derived from the physics formula: v = sqrt(h * -2 * g)
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }
    }
}