using System;
using UnityEngine;
using UnityEngine.Events;

namespace YanickSenn.Controller.FirstPerson.Hand {

    [DisallowMultipleComponent]
    public class Interactable : MonoBehaviour {
    
        [SerializeField]
        private InteractionEvent onInteractEvent = new();
        public InteractionEvent OnInteractEvent => onInteractEvent;

        public void Interact(Hand hand) {
            onInteractEvent.Invoke(hand);
        }

        [Serializable]
        public class InteractionEvent : UnityEvent<Hand> {
            
        }
    }
}