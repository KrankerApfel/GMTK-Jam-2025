using System.Collections.Generic;
using UnityEngine;

public class ActionSequencer : MonoBehaviour
{
    private Queue<ActionBase> actionStack;
    private ActionBase lastAction;
    private ActionBase currentAction;
    private ActionBase nextAction;

    private void Awake()
    {
        actionStack = new Queue<ActionBase>();
    }

    
    public void PlayLastPostAction()
    {
        lastAction?.PostAction();
    }
    
    public void PlayCurrentAction()
    {
        currentAction?.TriggerAction();
    }
    
    public void PlayNextPreAction()
    {
        nextAction?.PreAction();
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
        {
            lastAction = currentAction;
            actionStack.Enqueue(currentAction);
            currentAction = actionStack.Dequeue();
            nextAction = actionStack.Count > 0 ? actionStack.Peek() : null;
        }
        else
        {
            nextAction = actionStack.Peek();
            currentAction = actionStack.Dequeue();
        }
    }

    private void OnActionStarted()
    {
        // play sound
    }

    private void OnActionFinished()
    {
    }


}
