using UnityEngine;

public interface IAnimationable : IEntity
{
    Animator Animator { get; }
}