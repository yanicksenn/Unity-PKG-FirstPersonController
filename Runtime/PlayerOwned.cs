using UnityEngine;

namespace YanickSenn.Controller.FirstPerson
{
    [DisallowMultipleComponent]
    public class PlayerOwned : MonoBehaviour {
        [SerializeField]
        private Player owner;
        public Player Owner => owner;
    }
}