namespace NTO24
{
    public interface IUnitTask
    {
        bool IsComplete { get; }
        Entity Entity { get; }

        AnimationController.Animations Animation { get; }

        void Enter() { }
        void Update() { }
        void FixedUpdate() { }
        void Exit() { }
    }
}