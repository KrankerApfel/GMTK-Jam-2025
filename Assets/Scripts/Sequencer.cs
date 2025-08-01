using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Sequencer : MonoBehaviour
{
    public static Sequencer Instance { get; private set; }
    
    [SerializeField]
    private ActionSequencer actionSequencer;

    public string BeatMap = "1000";
    public int BPM = 60;
    public int BarCount = 6;
    
    private int ticksPerBar;
    [HideInInspector]
    public float tickInterval;
    // [HideInInspector] public int SlotCount;
    private float latency;
    private float tickStamp;

    private AudioSource audioSource;
    public AudioClip TickClip;
    public AudioClip BarClip;
    
    private AudioSource musicSource;
    public AudioClip MusicClip;
    public float MusicOffset = 0f;

    private bool isPlaying;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        tickInterval = 60f / BPM;
        latency = tickInterval / 6f;
        audioSource = GetComponent<AudioSource>();
        ticksPerBar = BeatMap.Length;

        musicSource = GetComponentInChildren<AudioSource>();
        musicSource.clip = MusicClip;
        musicSource.loop = true;
        isPlaying = true;

    }

    public void Stop()
    {
        isPlaying = false;
    }

    void FixedUpdate()
    {
        if (!isPlaying)
            return;

        // if not on it return
        if (Time.time % tickInterval <= Time.fixedDeltaTime)
        {
            StartAnimation();
            tickStamp = Time.time + latency;
        }
        else if (Time.time > tickStamp && Time.time - tickStamp <= Time.fixedDeltaTime) Tick();
    }

    private void StartAnimation()
    {
        if (!musicSource.isPlaying) StartCoroutine(PlayMusic());
        // Check which tick we are on
        int tickIndex = (int)(Time.time / tickInterval);
        
        if (BeatMap[tickIndex % ticksPerBar] == '0')
        {
            //if next is a bar
            if (BeatMap[(tickIndex+1)%ticksPerBar] == '1')
                StartCoroutine(Ring.Instance.PlayAnimation("PreTransition"));
            else StartCoroutine(Ring.Instance.PlayAnimation("Tick"));
        }
        
        else if (BeatMap[tickIndex % ticksPerBar] == '1' && tickIndex != 0)
        {
            StartCoroutine(Ring.Instance.Rotate());
            StartCoroutine(Ring.Instance.PlayAnimation("PostTransition"));
        }
    }

    private void Tick()
    {
        int tickIndex = (int)(Time.time / tickInterval);
        
        if (BeatMap[tickIndex % ticksPerBar] == '0')
        {
            audioSource.PlayOneShot(TickClip);
        }
        
        else if (BeatMap[tickIndex % ticksPerBar] == '1' && tickIndex != 0)
        {
            audioSource.PlayOneShot(BarClip);
            PlayAction();
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
