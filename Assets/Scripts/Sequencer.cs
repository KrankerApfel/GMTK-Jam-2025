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
    [HideInInspector] public ActionBase[] actionPool;
    [HideInInspector] public List<ActionBase> actionSequence;
    private int introActionIndex = 0;

    public string BeatMap;
    private BeatMap beatMapUI;
    public int BPM = 60;
    public int BarCount = 6;
    private int SlotCount;

    private int ticksPerBar;
    [HideInInspector] public float tickInterval;
    private float offset;
    private float tickStamp;
    private float gameStartTime, elapsedTime;
    [SerializeField] private float gachaInterval = 10f;

    [HideInInspector] public AudioSource audioSource;
    public AudioClip TickClip;
    public AudioClip BarClip;

    private AudioSource musicSource;
    [SerializeField] public AudioClip MusicClip;
    public float MusicOffset = 0f;

    [HideInInspector] public bool isPlaying = false;
    [HideInInspector] public bool isIntro = false;
    [HideInInspector] public bool animPlaying = false;
    private bool soundPlaying = false;
    private int tickAnimIndex = 0;
    private int tickSoundIndex = 0;
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
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        isPlaying = false;
        isIntro = false;
        animPlaying = false;
        soundPlaying = false;
        UIManager.Instance.ShowLoadingScreen();
        yield return null;
    }
    
    public void CreateSequence(ActionBase[] actionPool, ActionBase[] fixedSequence)
    {
        SlotCount = 0;
        foreach (var c in BeatMap)
            if (c == '1') SlotCount++;
        SlotCount *= BarCount;
        
        tickInterval = 60f / BPM;
        offset = tickInterval / 6f;
        ticksPerBar = BeatMap.Length;
        
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
        
        //print every elements
        string actionNames = "";
        foreach (ActionBase action in actionSequence)
        {
            actionNames += action.ActionName + ", ";
        }
        Debug.Log("Action Sequence: " + actionNames);
    }

    public void Stop()
    {
        isPlaying = false;
        StopMusic();
    }
    
    public void StartIntro()
    {
        UIManager.Instance.BeatMap.SetActive(true);
        UIManager.Instance.BeatMap.GetComponent<BeatMap>().Reset();
        // UIManager.Instance.Ring.
        
        tickStamp = 0;
        gameStartTime = Time.time + offset;
        
        isPlaying = false;
        isIntro = true;
        
        introActionIndex = 0;
        tickSoundIndex = -1;
        tickAnimIndex = -1;
        
        
        loaded = true;
    }

    private void Update()
    {
        if (!isPlaying && !isIntro)
            return;

        elapsedTime = Time.time - gameStartTime;

        if (elapsedTime % tickInterval < Time.deltaTime && !animPlaying)
        {
            TickAnimation();
            tickStamp = elapsedTime + offset;
        }
        else if (elapsedTime > tickStamp && elapsedTime - tickStamp < Time.deltaTime)
        {
            TickSound();
        }
    }

    private void TickAnimation()
    {
        int tmpIndex = (int)((Time.time - gameStartTime) / tickInterval);
        if (tmpIndex <= tickAnimIndex) return;
        
        animPlaying = true;
        tickAnimIndex = tmpIndex;

        if (BeatMap[tickAnimIndex % ticksPerBar] == '0')
        {
            if (BeatMap[(tickAnimIndex + 1) % ticksPerBar] == '1')
                StartCoroutine(Ring.Instance.PlayAnimation("PreTransition"));
            else
                StartCoroutine(Ring.Instance.PlayAnimation("Tick"));

            if (isIntro && introActionIndex == actionSequence.Count)
            { //finish intro
                introActionIndex++;
                UIManager.Instance.FinishIntro();
                StartCoroutine(PlayMusic());
            }
        }
        else if (BeatMap[tickAnimIndex % ticksPerBar] == '1' && tickAnimIndex != 0)
        {
            StartCoroutine(Ring.Instance.Rotate());
            StartCoroutine(Ring.Instance.PlayAnimation("PostTransition"));
            if (isIntro) introActionIndex++;
        }
        animPlaying = false;
    }

    private void TickSound()
    {
        int tmpIndex = (int)((Time.time - gameStartTime - offset) / tickInterval);
        if (tmpIndex <= tickSoundIndex) return;
        
        soundPlaying = true;
        tickSoundIndex = tmpIndex;

        if (isIntro) beatMapUI.Advance();

        if (BeatMap[tickSoundIndex % ticksPerBar] == '0')
        {
            audioSource.clip = TickClip;
            if (isIntro && introActionIndex > actionSequence.Count)
            {
                isIntro = false;
                isPlaying = true;
                actionSequencer.PlayNextPreAction();
                actionSequencer.ResetNextAction(); 
            }

            if (isIntro && !gachaing)
            {
                gachaing = true;
                StartCoroutine(Gacha());
            }
            audioSource.Play();
        }
        else if (BeatMap[tickSoundIndex % ticksPerBar] == '1' && tickSoundIndex != 0)
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
        soundPlaying = false;
    }

    private IEnumerator Gacha()
    {
        if (Ring.Instance != null && GameManager.Instance.fixedSequence.Count == 0)
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

    public void SetMusic(AudioClip clip) 
    {
        MusicClip = clip;
        musicSource.clip = MusicClip;
    }
    private IEnumerator PlayMusic()
    {
        yield return new WaitForSeconds(MusicOffset);
        musicSource.Play();
    }
    private IEnumerator StopMusic()
    {
        yield return null;
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
