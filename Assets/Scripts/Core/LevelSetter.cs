using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetter : MonoBehaviour
{
    [SerializeField] private string BeatMap = "1000";
    [SerializeField] private int BPM = 60;
    [SerializeField] private int BarCount = 6;
    [SerializeField] private AudioClip Music;
    [SerializeField] private List<String> actionNames;

    private void Start()
    {
        Sequencer.Instance.BeatMap = BeatMap;
        Sequencer.Instance.BPM = BPM;
        Sequencer.Instance.BarCount = BarCount;
        Sequencer.Instance.SetMusic(Music);

        if(actionNames.Count>0)
            GameManager.Instance.SetFixedSequence(actionNames);
    }
}
