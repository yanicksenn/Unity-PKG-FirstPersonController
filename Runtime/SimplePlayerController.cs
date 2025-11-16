using UnityEngine;

namespace YanickSenn.Controller.FirstPerson
{
    [DisallowMultipleComponent]
    public class SimplePlayerController : AbstractPlayerController {
        protected override IPlayerState GetDefaultPlayerState() {
            return new SimplePlayerState(this);
        }
    }
}
