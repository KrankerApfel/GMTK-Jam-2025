using System;
using System.Collections;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Duration in seconds")]
    protected float duration = 1f;

    public Action OnActionStarted;
    public Action OnActionFinished;

    private float frequency;
    
    public abstract void HandleAction();
    

    public void SetFrequency(float f)
    {
        frequency = f;
        duration *= frequency;

    }
    public void TriggerAction() 
    {
        OnActionStarted?.Invoke();
        StartCoroutine(PerformAction());
        OnActionFinished?.Invoke();
    }
    protected IEnumerator PerformAction() 
    {
        HandleAction();
        yield return new WaitForSeconds(duration);
    }
}
