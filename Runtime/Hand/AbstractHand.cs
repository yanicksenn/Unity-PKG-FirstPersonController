using UnityEngine;
using YanickSenn.Utils;

namespace YanickSenn.Controller.FirstPerson.Hand
{
    public abstract class AbstractHand : MonoBehaviour
    {
        [SerializeField] private AbstractPlayerController playerController;
        public AbstractPlayerController PlayerController => playerController;

        [SerializeField] private float maxGrabbingDistance = 3;
        public float MaxGrabbingDistance => maxGrabbingDistance;

        [SerializeField] private float throwForce = 10;
        public float ThrowForce => throwForce;

        private Optional<Looker> _looker;
        
        private IState _currentState = new Idle();
        protected IState CurrentState
        {
            get => _currentState;
            set {
                var oldState = _currentState;
                _currentState = value;
                
                // TODO: Make state handling more generic.
                switch (_currentState) {
                    case Idle:
                        if (oldState is Holding h1) {
                            OnRelease(h1);
                        }
                        break;
                    case Holding currentHolding:
                        if (oldState is Holding h2) {
                            if (h2.Grabbable != currentHolding.Grabbable) {
                                OnRelease(h2);
                                OnGrab(currentHolding);
                            }
                        } else {
                            OnGrab(currentHolding);
                        }
                        break;
                }
            }
        }

        private void Awake() {
            _looker = playerController.Looker;
        }

        private void OnDrawGizmos() {
            if (_looker.IsAbsent) return;
            var looker = _looker.Value;
            Gizmos.color = Color.red;
            var origin = looker.LookOrigin;
            var direction = looker.LookDirection;
            Gizmos.DrawLine(origin, origin + direction * MaxGrabbingDistance);
        }


        public bool Grab() {
            if (_looker.IsAbsent) return false;
            var looker = _looker.Value;
    
            if (CurrentState is Holding) {
                return false;
            }

            var origin = looker.LookOrigin;
            var direction = looker.LookDirection;
    
            return Physics.Raycast(origin, direction, out var hit, maxGrabbingDistance)
                && Grab(hit.collider.gameObject);
        }

        public bool Release() {
            return Throw(Vector3.zero);
        }

        public bool Throw() {
            if (_looker.IsAbsent) return false;
            var looker = _looker.Value;
    
            return Throw(looker.LookDirection * throwForce);
        }
        
        public abstract bool Grab(GameObject gameObject);
        public abstract bool Throw(Vector3 force);

        protected virtual void OnGrab(Holding holding) { }
        protected virtual void OnRelease(Holding holding) { }
        

        protected interface IState {}
    
        protected class Idle : IState {}

        protected class Holding : IState {
            public Grabbable Grabbable { get; }
            public Quaternion RotationOffset { get; }
            public RigidbodySnapshot RigidbodySnapshot { get; }
            public ColliderSnapshot ColliderSnapshot { get; }
            public Holding(Grabbable grabbable, Quaternion rotationOffset) {
                Grabbable = grabbable;
                RotationOffset = rotationOffset;
                RigidbodySnapshot = RigidbodySnapshot.From(Grabbable.Rigidbody);
                ColliderSnapshot = ColliderSnapshot.From(Grabbable.Collider);
            }
        }
    }
}