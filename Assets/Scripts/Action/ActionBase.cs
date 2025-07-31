using System;
using System.Collections;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Duration in seconds")]
    private float duration = 1f;

    public Action OnActionStarted;
    public Action OnActionFinished;
    
    public abstract void HandleAction();

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
