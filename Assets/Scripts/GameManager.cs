using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ActionSequencer actionSequencer;
    [SerializeField]
    private Sequencer sequencer;
    [SerializeField]
    private List<ActionBase> actions;
    [SerializeField]
    private PlayerPhysics player;

    private void Start()
    {
        // ActionBase[] selectedActions = actions.GetRange(0,3).ToArray();
        // actionSequencer.SetNewActions(selectedActions);
        Sequencer.Instance.CreateSequence(actions.ToArray());

        player.OnPlayerDestroyed += OnPlayerDestroyed;
    }

    private void OnPlayerDestroyed() 
    {
        sequencer.Stop();
        player.OnPlayerDestroyed -= OnPlayerDestroyed;
    }

}
