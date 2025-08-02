using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private ActionSequencer actionSequencer;
    [SerializeField] private PlayerPhysics player;

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
    }

    private void Start()
    {
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
        Debug.Log("Level finished !");
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
}
