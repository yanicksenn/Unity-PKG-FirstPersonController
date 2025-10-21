using System;
using UnityEngine;
using UnityEngine.Events;

namespace YanickSenn.Controller.FirstPerson
{
    [DisallowMultipleComponent]
    public class Interactable : MonoBehaviour {
        [SerializeField]
        private InteractionEvent onInteractEvent = new();
        public InteractionEvent OnInteractEvent => onInteractEvent;

        public void Interact(Hand hand) {
            onInteractEvent.Invoke(hand.gameObject);
        }

        [Serializable]
        public class InteractionEvent : UnityEvent<GameObject> {
            
        }
    }
}