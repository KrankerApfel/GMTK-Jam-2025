using System;
using System.Collections;
using UnityEngine;

public class ActionDoNothing : ActionBase
{
    [SerializeField]
    [Tooltip("Duration in seconds")]

    public override void HandleAction()
    {
        StartCoroutine(Waitforduration());
        OnActionFinished?.Invoke();
    }
    private IEnumerator Waitforduration()
    {
        yield return new WaitForSeconds(duration);
    }
}
