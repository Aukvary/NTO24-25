using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class UnitiesMoveController : MonoBehaviour
{
    [SerializeField]
    private float _unitSpeed;


    private NavMeshAgent _navMeshAgent;

    public Vector3 UnitPosition { get; private set; }

    private void Awake()
    {
        UnitPosition = transform.position;
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _navMeshAgent.speed = _unitSpeed;
    }

    public void MoveTo(Vector3 newPosition)
    {
        UnitPosition = newPosition;
        _navMeshAgent.destination = newPosition;
    }
}
