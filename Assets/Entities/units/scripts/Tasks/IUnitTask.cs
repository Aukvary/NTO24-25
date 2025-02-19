public interface IUnitTask
{
    Unit Unit { get; }

    bool IsComplete { get; }

    void Enter() { }
    void Update() { }
    void FixedUpdate() { }
    void Exit() { }
}