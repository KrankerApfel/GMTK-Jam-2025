using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Sequencer : MonoBehaviour
{
    public static Sequencer Instance { get; private set; }

    [SerializeField] private ActionSequencer actionSequencer;
    [SerializeField] public ActionBase[] actionPool;
    private List<ActionBase> actionSequence;
    private int introActionIndex = 0;

    public string BeatMap = "1000";
    private BeatMap beatMapUI;
    public int BPM = 60;
    public int BarCount = 6;
    private int SlotCount;

    private int ticksPerBar;
    [HideInInspector] public float tickInterval;
    private float latency;
    private float tickStamp;
    private float introStartTime, gameStartTime, elapsedTime;
    [SerializeField] private float gachaInterval = 10f;

    [HideInInspector] public AudioSource audioSource;
    public AudioClip TickClip;
    public AudioClip BarClip;

    private AudioSource musicSource;
    [SerializeField] public AudioClip MusicClip;
    public float MusicOffset = 0f;

    [HideInInspector] public bool isPlaying = false;
    [HideInInspector] public bool isIntro = false;
    private bool gachaing = false;
    
    private bool loaded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SlotCount = 0;
        foreach (var c in BeatMap)
            if (c == '1') SlotCount++;
        SlotCount *= BarCount;
    }

    private void Start()
    {
        tickInterval = 60f / BPM;
        latency = tickInterval / 6f;
        audioSource = GetComponent<AudioSource>();
        ticksPerBar = BeatMap.Length;
        beatMapUI = UIManager.Instance.BeatMap.GetComponent<BeatMap>();

        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        musicSource.clip = MusicClip;
        musicSource.loop = true;

        UIManager.Instance.ShowLoadingScreen();
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (loaded) StartCoroutine(InitAfterSceneLoad());
    }

    private IEnumerator InitAfterSceneLoad()
    {
        yield return null;
        StartIntro();
        // CreateSequence(actionPool, new ActionBase[0]);
    }

    public void StartIntro()
    {
        introStartTime = Time.time;
        isIntro = true;
        introActionIndex = 0;
        loaded = true;
    }

    public void SetMusic(AudioClip clip) 
    {
        MusicClip = clip;
        musicSource.clip = MusicClip;
    }
    public void CreateSequence(ActionBase[] actionPool, ActionBase[] fixedSequence)
    {
        Ring.Instance.ResetSlots();
        actionSequencer = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionSequencer>();

        this.actionPool = actionPool;
        actionSequence = new List<ActionBase>();

        for (int i = 0; i < SlotCount; i++)
        {
            if (fixedSequence.Length > 0)
            {
                ActionBase tmpSeq = fixedSequence[i % fixedSequence.Length];
                if (tmpSeq == null)
                    actionSequence.Add(actionPool[Random.Range(0, actionPool.Length)]);
                else
                    actionSequence.Add(tmpSeq);
            }
            else
            {
                actionSequence.Add(actionPool[Random.Range(0, actionPool.Length)]);
            }
        }

        actionSequencer.SetNewActions(actionSequence.ToArray());
        Ring.Instance.AddSlots(actionSequence.ToArray());
    }

    public void Stop()
    {
        isPlaying = false;
        StopMusic();
    }

    private void FixedUpdate()
    {
        if (!isPlaying && !isIntro)
            return;

        elapsedTime = Time.time - Math.Max(gameStartTime, introStartTime);

        if (elapsedTime % tickInterval < Time.fixedDeltaTime)
        {
            TickAnimation();
            tickStamp = elapsedTime + latency;
        }
        else if (elapsedTime > tickStamp && elapsedTime - tickStamp < Time.fixedDeltaTime)
        {
            TickSound();
        }
    }

    private void TickAnimation()
    {
        int tickIndex = (int)((Time.time - Math.Max(gameStartTime, introStartTime)) / tickInterval);

        if (BeatMap[tickIndex % ticksPerBar] == '0')
        {
            if (BeatMap[(tickIndex + 1) % ticksPerBar] == '1')
                StartCoroutine(Ring.Instance.PlayAnimation("PreTransition"));
            else
                StartCoroutine(Ring.Instance.PlayAnimation("Tick"));

            if (isIntro && introActionIndex == actionSequence.Count)
            { //finish intro
                introActionIndex++;
                gameStartTime = introStartTime;
                UIManager.Instance.FinishIntro();
                StartCoroutine(PlayMusic());
                // actionSequencer.PlayNextPreAction();
            }
        }
        else if (BeatMap[tickIndex % ticksPerBar] == '1' && tickIndex != 0)
        {
            StartCoroutine(Ring.Instance.Rotate());
            StartCoroutine(Ring.Instance.PlayAnimation("PostTransition"));
            if (isIntro) introActionIndex++;
        }
    }

    private void TickSound()
    {
        int tickIndex = (int)((Time.time - Math.Max(gameStartTime, introStartTime)) / tickInterval);

        if (isIntro) beatMapUI.Advance();

        if (BeatMap[tickIndex % ticksPerBar] == '0')
        {
            audioSource.clip = TickClip;
            if (isIntro && introActionIndex > actionSequence.Count)
            {
                isIntro = false;
                isPlaying = true;
                actionSequencer.PlayNextPreAction();
            }

            if (isIntro && !gachaing)
            {
                gachaing = true;
                StartCoroutine(Gacha());
            }
            audioSource.Play();
        }
        else if (BeatMap[tickIndex % ticksPerBar] == '1' && tickIndex != 0)
        {
            audioSource.clip = BarClip;
            audioSource.Play();
            if (isPlaying)
            {
                PlayAction();
            }
            gachaing = false;

            beatMapUI.SetActionTitle(actionSequence[(introActionIndex - 1 + actionSequence.Count) % actionSequence.Count].ActionName);
        }
    }

    private IEnumerator Gacha()
    {
        if (Ring.Instance != null)
        {
            Image centerImage = Ring.Instance.CenterImage;
            centerImage.enabled = true;
            while (Sequencer.Instance.audioSource.clip.name == Sequencer.Instance.TickClip.name)
            {
                centerImage.sprite = actionPool[Random.Range(0, actionPool.Length)].ActionIcon;
                centerImage.transform.rotation = Quaternion.Euler(0, 0, 0);
                yield return new WaitForSeconds(tickInterval / 100f * gachaInterval);
            }
        }
    }

    private IEnumerator PlayMusic()
    {
        yield return new WaitForSeconds(MusicOffset);
        musicSource.Play();
    }
    private IEnumerator StopMusic()
    {
        yield return new WaitForSeconds(MusicOffset);
        musicSource.Stop();
    }

    private void PlayAction()
    {
        actionSequencer.PlayLastPostAction();
        actionSequencer.PlayCurrentAction();
        actionSequencer.PlayNextPreAction();
        actionSequencer.NextAction();
    }
    
}
