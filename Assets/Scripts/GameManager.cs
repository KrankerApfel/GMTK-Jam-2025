using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ActionSequencer actionSequencer;
    [SerializeField]
    private List<ActionBase> actions;

    private void Start()
    {
        ActionBase[] selectedActions = actions.GetRange(0,2).ToArray();
        actionSequencer.SetNewActions(selectedActions);
    }

    private void Update()
    {
        actionSequencer.PlayCurrentAction();
    }

}
