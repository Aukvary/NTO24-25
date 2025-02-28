namespace NTO24
{
    public interface IUnitTask
    {
        bool IsComplete { get; }
        Entity Entity { get; }

        void Enter() { }
        void Update() { }
        void FixedUpdate() { }
        void Exit() { }
    }
}