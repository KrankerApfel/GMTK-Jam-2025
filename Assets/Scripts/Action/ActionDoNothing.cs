using System.Collections;
using UnityEngine;

public class ActionDoNothing : ActionBase
{
    [SerializeField]
    [Tooltip("Duration in seconds")]

    public override void HandleAction()
    {
        OnActionFinished?.Invoke();
        return;
    }

    public override void PreAction() { return; }
    public override void PostAction() { return; }
    
}
