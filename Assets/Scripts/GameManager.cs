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
        ActionBase[] selectedActions = actions.ToArray();
        actionSequencer.SetNewActions(selectedActions);
    }

}
