public interface UnitTask
{
    public Unit Unit { get; }

    bool IsComplete { get; }

    void Enter() { }
    void Update() { }
    void FixedUpdate() { }
    void Exit() { }
}