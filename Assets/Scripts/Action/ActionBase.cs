using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class ActionBase : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Duration in seconds")]
    protected float duration = 1f;

    public Action OnActionStarted;
    public Action OnActionFinished;
    
    public string ActionName;
    public Sprite ActionIcon;

    public abstract void HandleAction();
  
    public void TriggerAction()
    {
        OnActionStarted?.Invoke();
        HandleAction();
    }
}
