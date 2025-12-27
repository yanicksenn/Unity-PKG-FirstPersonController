using UnityEngine;
using YanickSenn.Utils.Snapshots;

namespace YanickSenn.Controller.FirstPerson.Hand
{
    public abstract class AbstractHand : PlayerOwned {
        [SerializeField] private float maxGrabbingDistance = 3;
        [SerializeField] private float throwForce = 10;

        public float MaxGrabbingDistance => maxGrabbingDistance;
        public float ThrowForce => throwForce;

        private IHandState _currentHandState = new Idle();
        public IHandState CurrentHandState
        {
            get => _currentHandState;
            set {
                var oldState = _currentHandState;
                _currentHandState = value;
                
                // TODO: Make state handling more generic.
                switch (_currentHandState) {
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

        public bool Use() {
            if (CurrentHandState is not Holding holding) {
                return false;
            }

            if (!holding.Grabbable.TryGetComponent<Usable>(out var usable)) {
                return false;
            }
            
            usable.Use(this);
            return true;
        }

        public bool Release(Vector3 direction, float forceMultiplier = 0.0f) {
            return Throw(throwForce * Mathf.Clamp01(forceMultiplier) * direction);
        }
        
        public abstract bool Interact(GameObject gameObject);
        public abstract bool Throw(Vector3 force);

        protected virtual void OnGrab(Holding holding) { }
        protected virtual void OnRelease(Holding holding) { }
        

        public interface IHandState {}
    
        public class Idle : IHandState {}

        public class Holding : IHandState {
            public Grabbable Grabbable { get; }
            public RigidbodySnapshot RigidbodySnapshot { get; }
            public ColliderSnapshot ColliderSnapshot { get; }
            public Holding(Grabbable grabbable) {
                Grabbable = grabbable;
                RigidbodySnapshot = RigidbodySnapshot.From(Grabbable.Rigidbody);
                ColliderSnapshot = ColliderSnapshot.From(Grabbable.Collider);
            }
        }
    }
}