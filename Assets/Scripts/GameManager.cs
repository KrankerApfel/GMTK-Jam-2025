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
        ActionBase[] selectedActions = actions.ToArray();
        actionSequencer.SetNewActions(selectedActions);
        player.OnPlayerDestroyed += OnPlayerDestroyed;
    }

    private void OnPlayerDestroyed() 
    {
        sequencer.Stop();
        player.OnPlayerDestroyed -= OnPlayerDestroyed;
    }

}
