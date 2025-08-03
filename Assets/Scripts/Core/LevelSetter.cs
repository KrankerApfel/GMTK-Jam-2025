using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetter : MonoBehaviour
{
    [SerializeField] private string BeatMap = "1000";
    [SerializeField] private int BPM = 60;
    [SerializeField] private int BarCount = 6;
    [SerializeField] private AudioClip Music;
    [SerializeField] private List<String> actionNames;
    [SerializeField] private string loadingMessage;

    public void Start()
    {
        // if (UIManager.Instance == null) SceneManager.LoadScene(0);
        
        Sequencer.Instance.BeatMap = BeatMap;
        Sequencer.Instance.BPM = BPM;
        Sequencer.Instance.BarCount = BarCount;
        Sequencer.Instance.SetMusic(Music);

        if(actionNames.Count>0)
            GameManager.Instance.SetFixedSequence(actionNames);

        if (loadingMessage.Length > 0)
        {
            UIManager.Instance.Loading.GetComponentInChildren<TextMeshProUGUI>().text = loadingMessage;
        }
    }
}
