using UnityEngine;

namespace YanickSenn.Controller.FirstPerson {
    public abstract class AbstractMover : MonoBehaviour {
        public Vector2 MoveInput { get; set; }
        public bool IsRunning { get; set; }
        public abstract bool IsGrounded { get; }

        public abstract void Jump();
    }
}