using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Sequencer : MonoBehaviour
{
    public static Sequencer Instance { get; private set; }
    
    [SerializeField]
    private ActionSequencer actionSequencer;

    [SerializeField] public ActionBase[] actionPool;
    private List<ActionBase> actionSequence;
    private int introActionIndex = 0;

    public string BeatMap = "1000";
    private BeatMap beatMapUI;
    public int BPM = 60;
    public int BarCount = 6;
    private int SlotCount;
    
    
    private int ticksPerBar;
    [HideInInspector]
    public float tickInterval;
    // [HideInInspector] public int SlotCount;
    private float latency;
    private float tickStamp;
    private float titleStartTime, introStartTime, gameStartTime, elapsedTime;
    [SerializeField] private float gachaInterval = 10f;


    [HideInInspector] public AudioSource audioSource;
    public AudioClip TickClip;
    public AudioClip BarClip;
    
    private AudioSource musicSource;
    [SerializeField] private AudioClip MusicClip;
    public float MusicOffset = 0f;

    [HideInInspector] public bool isPlaying = false;
    [HideInInspector] public bool isIntro = false;
    private bool gachaing = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        SlotCount = 0;
        foreach (var c in BeatMap)
        { if (c == '1') SlotCount++; }
        SlotCount *= BarCount;
    }

    void Start()
    {
        tickInterval = 60f / BPM;
        latency = tickInterval / 6f;
        audioSource = GetComponent<AudioSource>();
        ticksPerBar = BeatMap.Length;
        beatMapUI = UIManager.Instance.BeatMap.GetComponent<BeatMap>();

        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        musicSource.clip = MusicClip;
        musicSource.loop = true;
        // isPlaying = true;

        // StartCoroutine(Ring.Instance.IntroToGamePos());
        // StartCoroutine(Ring.Instance.GameToIntroPos());
        
        // gameStartTime = Time.time;
        UIManager.Instance.ShowLoadingScreen();
    }

    public void StartIntro()
    {
        introStartTime = Time.time;
        isIntro = true;
        introActionIndex = 0;
    }
    
    public void CreateSequence(ActionBase[] actionPool, ActionBase[] fixedSequence)
    {
        this.actionPool = actionPool;
        actionSequence = new List<ActionBase>();
        for (int i = 0; i < SlotCount; i++)
        {
            if(fixedSequence.Length > 0)
            {
                ActionBase tmpSeq = fixedSequence[i%fixedSequence.Length]; 
                
                if(tmpSeq == null)
                {
                    actionSequence.Add(actionPool[Random.Range(0, actionPool.Length)]);
                }
                else actionSequence.Add(fixedSequence[i% fixedSequence.Length]);
            }
            else actionSequence.Add(actionPool[Random.Range(0, actionPool.Length)]);
        }
        
        actionSequencer.SetNewActions(actionSequence.ToArray());
        Ring.Instance.AddSlots(actionSequence.ToArray());
    }

    public void Stop()
    {
        isPlaying = false;
    }

    void FixedUpdate()
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
        // if (!musicSource.isPlaying) StartCoroutine(PlayMusic());
        // Check which tick we are on
        int tickIndex = (int)((Time.time - Math.Max(gameStartTime, introStartTime)) / tickInterval);
        
        //Tick
        if (BeatMap[tickIndex % ticksPerBar] == '0')
        {
            //if next is a bar
            if (BeatMap[(tickIndex+1)%ticksPerBar] == '1')
                StartCoroutine(Ring.Instance.PlayAnimation("PreTransition"));
            else StartCoroutine(Ring.Instance.PlayAnimation("Tick"));

            if (isIntro && introActionIndex == actionSequence.Count)
            {
                introActionIndex++;
                gameStartTime = introStartTime;
                UIManager.Instance.FinishIntro();
                StartCoroutine(PlayMusic());
            }
        }
        
        //Bar
        else if (BeatMap[tickIndex % ticksPerBar] == '1' && tickIndex != 0)
        {
            StartCoroutine(Ring.Instance.Rotate());
            StartCoroutine(Ring.Instance.PlayAnimation("PostTransition"));
            if(isIntro) introActionIndex++;
        }
    }

    private void TickSound()
    {
        int tickIndex = (int)((Time.time - Math.Max(gameStartTime, introStartTime)) / tickInterval);
        
        if(isIntro) beatMapUI.Advance();
        
        //Tick
        if (BeatMap[tickIndex % ticksPerBar] == '0')
        {
            audioSource.clip = TickClip;
            if (isIntro && introActionIndex > actionSequence.Count)
            {
                isIntro = false;
                isPlaying = true;
            }

            if (isIntro && !gachaing)
            {
                gachaing = true;
                StartCoroutine(Gacha());
            }
            audioSource.Play();
        }
        
        //Bar
        else if (BeatMap[tickIndex % ticksPerBar] == '1' && tickIndex != 0)
        {
            audioSource.clip = BarClip;
            audioSource.Play();
            if(isPlaying) PlayAction();
            // if(isIntro) Ring.Instance.ShowIconOnebyOne();
            gachaing = false;
            
            beatMapUI.SetActionTitle(actionSequence[(introActionIndex-1+actionSequence.Count)%actionSequence.Count].ActionName);
        }
    }
    
    private IEnumerator Gacha()
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
    
    private IEnumerator PlayMusic()
    {
        yield return new WaitForSeconds(MusicOffset);
        musicSource.Play();
    }

    private void PlayAction()
    {
        actionSequencer.PlayCurrentAction();
        actionSequencer.NextAction();
    }
}
