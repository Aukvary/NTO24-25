using System;
using UnityEngine;

public class BehaviourAnimation : MonoBehaviour
{
    [SerializeField]
    private AudioSource _stepSound;

    public event Action OnPunchAnimationEvent;

    private void Awake()
    {
        _stepSound = GetComponent<AudioSource>();
    }

    public void StepSound()
    {
        _stepSound?.Play();
    }

    public void Punch()
    {
        OnPunchAnimationEvent?.Invoke();
    }
}