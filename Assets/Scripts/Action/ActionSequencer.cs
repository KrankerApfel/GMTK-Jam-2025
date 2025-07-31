using System.Collections.Generic;
using UnityEngine;

public class ActionSequencer : MonoBehaviour
{
    [SerializeField]
    private float frequency = 5f;

    private Queue<ActionBase> actionStack;
    private ActionBase currentAction;

    private void Awake()
    {
        actionStack = new Queue<ActionBase>();
    }

    public void Play()
    {
        currentAction.TriggerAction();
    }

    public void SetFrequency(float f) 
    {
        frequency = f;
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
            action.SetFrequency(frequency);
            actionStack.Enqueue(action);
        
            action.OnActionStarted += OnActionStarted;
            action.OnActionFinished += OnActionFinished;
        }

        SetNextAction();
    }

    private void OnActionStarted()
    {
        // play sound
        Debug.Log("OnActionStarted");

    }

    private void OnActionFinished()
    {
        Debug.Log("OnActionFinished");
        SetNextAction();
    }

    private void SetNextAction()
    {
        if (currentAction != null) 
        {
            actionStack.Enqueue(currentAction);
            Debug.Log("curr " + currentAction);
        }
        currentAction = actionStack.Dequeue();

    }
}
