using UnityEngine;

namespace YanickSenn.Controller.FirstPerson.Hand
{
    [DisallowMultipleComponent]
    public class PhysicsHand : AbstractHand {
        private void FixedUpdate() {
            if (CurrentState is Holding holding) {
                Physics.SyncTransforms();
            }
        }

        public override bool Grab(GameObject gameObject) {
            if (PlayerController.Looker.IsAbsent) return false;
            var looker = PlayerController.Looker.Value;
            if (gameObject.TryGetComponent(out Grabbable grabbable) && grabbable.enabled) {
                CurrentState = new Holding(grabbable, Quaternion.Inverse(looker.transform.rotation) * grabbable.transform.rotation);
                grabbable.Grab(this);
                return true;
            }
            
            if (gameObject.TryGetComponent(out Interactable interactable) && interactable.enabled) {
                interactable.Interact(this);
                return true;
            }

            return false;
        }

        public override bool Throw(Vector3 force) {
            if (CurrentState is not Holding holding) return false;
            CurrentState = new Idle();
            holding.Grabbable.ReleaseWithForce(force);
            return true;
        }

        protected override void OnGrab(Holding holding) {
            var grabbable = holding.Grabbable;
            grabbable.transform.parent = transform;
            grabbable.transform.localPosition = Vector3.zero;
            grabbable.Rigidbody.linearVelocity = Vector3.zero;
            grabbable.Rigidbody.angularVelocity = Vector3.zero;
            grabbable.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            grabbable.Rigidbody.isKinematic = true;
            grabbable.Collider.excludeLayers |= 1 << LayerMask.NameToLayer("Player");
            grabbable.Collider.enabled = false;
        }

        protected override void OnRelease(Holding holding) {
            var grabbable = holding.Grabbable;
            grabbable.transform.parent = null;
            holding.RigidbodySnapshot.ApplyTo(grabbable.Rigidbody);
            holding.ColliderSnapshot.ApplyTo(grabbable.Collider);
        }
    }
}