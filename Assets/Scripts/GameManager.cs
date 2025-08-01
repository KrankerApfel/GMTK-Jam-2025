using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ActionSequencer actionSequencer;
    [SerializeField]
    private Sequencer sequencer;
    [FormerlySerializedAs("actions")] [SerializeField]
    private List<ActionBase> actionPool;
    [SerializeField]
    private PlayerPhysics player;

    private void Start()
    {
        // ActionBase[] selectedActions = actionPool.GetRange(0,3).ToArray();
        // actionSequencer.SetNewActions(selectedActions);
        Sequencer.Instance.CreateSequence(actionPool.ToArray());

        player.OnPlayerDestroyed += OnPlayerDestroyed;
    }

    private void OnPlayerDestroyed() 
    {
        sequencer.Stop();
        player.OnPlayerDestroyed -= OnPlayerDestroyed;
    }

}
