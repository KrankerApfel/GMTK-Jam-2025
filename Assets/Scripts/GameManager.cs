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
        // ActionBase[] selectedActions = actions.GetRange(0,3).ToArray();
        // actionSequencer.SetNewActions(selectedActions);
        
        Sequencer.Instance.CreateSequence(actions.ToArray());
    }

}
