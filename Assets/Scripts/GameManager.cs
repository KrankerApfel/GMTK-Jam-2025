using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private ActionSequencer actionSequencer;
    public PlayerPhysics player;

    [Header("Sequences")]
    [SerializeField] private List<ActionBase> actionPool;
    [SerializeField] private List<ActionBase> fixedSequence;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winAudio;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Init()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        player = playerObj.GetComponent<PlayerPhysics>();
        actionSequencer = playerObj.GetComponent<ActionSequencer>();

        if (actionSequencer != null && actionPool != null && fixedSequence != null)
        {
            Sequencer.Instance.CreateSequence(actionPool.ToArray(), fixedSequence.ToArray());
        }

        if (player != null)
        {
            player.OnPlayerDestroyed += OnPlayerDestroyed;
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnPlayerDestroyed -= OnPlayerDestroyed;
        }
    }

    private void OnPlayerDestroyed()
    {
        Sequencer.Instance.Stop();
        if (player != null)
        {
            player.OnPlayerDestroyed -= OnPlayerDestroyed;
        }
    }


    public void OnLevelFinished()
    {
        Sequencer.Instance.Stop();

        if (audioSource && winAudio)
        {
            audioSource.PlayOneShot(winAudio);
            StartCoroutine(WaitAndGoToNextLevel(winAudio.length));
        }
        else
        {
            if (LevelManager.Instance != null)
                LevelManager.Instance.FadeToNextLevel();
        }
    }

    private System.Collections.IEnumerator WaitAndGoToNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (LevelManager.Instance != null)
            LevelManager.Instance.FadeToNextLevel();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Init();

        foreach (ActionBase action in actionPool)
        {
            action.Init();
        }
        foreach (ActionBase action in fixedSequence)
        {
            action.Init();
        }
    }
}
