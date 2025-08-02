using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ActionSequencer actionSequencer;
    [SerializeField]
    private PlayerPhysics player;

    [SerializeField]
    private List<ActionBase> actionPool;
    [SerializeField] 
    private List<ActionBase> fixedSequence;
    
    private void Start()
    {
        Sequencer.Instance.CreateSequence(actionPool.ToArray(), fixedSequence.ToArray());
        player.OnPlayerDestroyed += OnPlayerDestroyed;
    }

    private void OnPlayerDestroyed() 
    {
        Sequencer.Instance.Stop();
        player.OnPlayerDestroyed -= OnPlayerDestroyed;
    }

}
