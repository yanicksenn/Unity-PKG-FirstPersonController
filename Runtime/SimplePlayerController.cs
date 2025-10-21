using UnityEngine;

namespace YanickSenn.Controller.FirstPerson
{
    [DisallowMultipleComponent]
    public class SimplePlayerController : AbstractPlayerController {
        public override IPlayerState GetDefaultPlayerState() {
            return new SimplePlayerState();
        }
    }
}
