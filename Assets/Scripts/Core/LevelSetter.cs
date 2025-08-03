using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetter : MonoBehaviour
{
    [SerializeField] private string BeatMap;
    [SerializeField] private int BPM = 60;
    [SerializeField] private int BarCount = 6;
    [SerializeField] private AudioClip Music;
    [SerializeField] private float MusicOffset = 0f;
    [SerializeField] private List<String> actionNames;
    [SerializeField] private string loadingMessage;
    [SerializeField] private float messageTime = 4f;

    public void Start()
    {
        // if (UIManager.Instance == null) SceneManager.LoadScene(0);
        
        Sequencer.Instance.BeatMap = BeatMap;
        Sequencer.Instance.BPM = BPM;
        Sequencer.Instance.BarCount = BarCount;
        Sequencer.Instance.SetMusic(Music);
        Sequencer.Instance.MusicOffset = MusicOffset;

        if(actionNames.Count>0)
            GameManager.Instance.SetFixedSequence(actionNames);
        else 
            GameManager.Instance.ClearFixedSequence();

        if (loadingMessage.Length > 0)
        {
            UIManager.Instance.Loading.GetComponentInChildren<TextMeshProUGUI>().text = loadingMessage;
            UIManager.Instance.Loading.GetComponent<LoadingScreen>().LoadingDuration = messageTime;
        }
    }
}
