using System;
using UnityEngine;

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
    
    public abstract void PreAction();
    public abstract void PostAction();
  

    public virtual void Init() { }

    private void Start()
    {
        Init();
    }
    public void TriggerAction()
    {
        OnActionStarted?.Invoke();
        HandleAction();
    }
}
