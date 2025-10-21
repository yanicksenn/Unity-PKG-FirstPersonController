namespace YanickSenn.Controller.FirstPerson
{
    public interface IPlayerState
    {
        void Enable(AbstractPlayerController playerController, Looker looker, Hand hand, AbstractMover abstractMover);
        void Disable();
    }
}