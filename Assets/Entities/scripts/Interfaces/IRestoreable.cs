using UnityEngine;

public interface IRestoreable : IEntity
{
    public float RestoreTime { get; }

    public EntityHealth HealthComponent { get; }

    Coroutine StartRestoring() => EntityReference.StartCoroutine(Restore());


    private System.Collections.IEnumerator Restore()
    {
        yield return new WaitForSeconds(RestoreTime);
        HealthComponent.Alive = true;
    }
}