using System.Collections.Generic;
using UnityEngine;

public class ActionSequencer : MonoBehaviour
{
    private Queue<ActionBase> actionStack;
    private ActionBase currentAction;

    private void Awake()
    {
        actionStack = new Queue<ActionBase>();
    }

    
    public void PlayPreAction()
    {
        // currentAction.PreAction();
    }
    
    public void PlayCurrentAction()
    {
        currentAction?.TriggerAction();
    }

    public void SetNewActions(ActionBase[] actions)
    {
        foreach (ActionBase action in actionStack)
        {
            action.OnActionStarted -= OnActionStarted;
            action.OnActionFinished -= OnActionFinished;
        }
        
        actionStack.Clear();

        foreach (ActionBase action in actions)
        {
            actionStack.Enqueue(action);

            action.OnActionStarted += OnActionStarted;
            action.OnActionFinished += OnActionFinished;
        }

        NextAction();
    }
    public void NextAction()
    {
        if (currentAction != null)
            actionStack.Enqueue(currentAction);

        currentAction = actionStack.Dequeue();
    }

    private void OnActionStarted()
    {
        // play sound
    }

    private void OnActionFinished()
    {
    }


}
