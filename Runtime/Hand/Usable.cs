
using System;
using UnityEngine;
using UnityEngine.Events;
using YanickSenn.Controller.FirstPerson.Hand;

[DisallowMultipleComponent, 
    RequireComponent(typeof(Grabbable))]
public class Usable : MonoBehaviour {
    
    [SerializeField]
    private UseEvent onUseEvent = new();
    
    public void Use(AbstractHand hand) {
        onUseEvent.Invoke(hand.gameObject);
    }

    [Serializable]
    public class UseEvent : UnityEvent<GameObject> {
            
    }
}