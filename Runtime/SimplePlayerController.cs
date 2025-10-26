using UnityEngine;

namespace YanickSenn.Controller.FirstPerson
{
    [DisallowMultipleComponent]
    public class SimplePlayerController : AbstractPlayerController {
        
        [SerializeField]
        private Looker looker;
        
        [SerializeField]
        private Hand hand;
        
        [SerializeField]
        private AbstractMover mover;
        
        protected override IPlayerState GetDefaultPlayerState() {
            return new SimplePlayerState(looker, hand, mover);
        }
    }
}
