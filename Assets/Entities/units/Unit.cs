using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitList hui;

    private UnitiesMoveController _moveController;

    public UnitStates UnitState { get; private set; }

    private void Awake()
    {
        _moveController = GetComponent<UnitiesMoveController>();
        hui.Add(this);
    }

    public void MoveTo(Vector3 newPostion)
    {
        _moveController.MoveTo(newPostion);
        UnitState = UnitStates.Walk;
    }
}