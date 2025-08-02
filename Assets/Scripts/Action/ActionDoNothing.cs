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

    public override void PreAction() { return; }
    public override void PostAction() { return; }

    private IEnumerator Waitforduration()
    {
        yield return new WaitForSeconds(duration);
    }
}
