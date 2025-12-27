using UnityEngine;

namespace YanickSenn.Controller.FirstPerson {
    public abstract class AbstractMover<T> : PlayerOwned where T : IMoverConfig {
        public Vector2 MoveInput { get; set; }
        public bool IsRunning { get; set; }
        
        public IMoverConfig MoverConfig { get; set; }
        
        public abstract bool IsGrounded { get; }

        public abstract void Jump();
    }
    
    public interface IMoverConfig { }
}