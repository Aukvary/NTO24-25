using System;
using UnityEngine;

public class BehaviourAnimation : MonoBehaviour
{
    public event Action OnPunchAnimationEvent;

    public void StepSound()
    {
        
    }

    public void Punch()
    {
        OnPunchAnimationEvent?.Invoke();
    }
}