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
        currentAction?.PostAction();
    }
    
    public void PlayCurrentAction()
    {
        currentAction?.TriggerAction();
    }
    
    public void PlayNextPreAction()
    {
        currentAction?.PreAction();
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
        }
        else
        { //make sure the PreAction of the first action is called
            lastAction = actionStack.Peek();
            actionStack.Enqueue(lastAction);
        }
        
        currentAction = actionStack.Dequeue();
        nextAction = actionStack.Count > 0 ? actionStack.Peek() : null;
    }

    private void OnActionStarted()
    {
        // play sound
    }

    private void OnActionFinished()
    {
    }


}
