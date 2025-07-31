using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private AudioSource audioSource;
    public AudioClip BeatClip;
    public AudioClip BarClip;

    
    private ActionSequencer actionSequencer;
    
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
    }

    void FixedUpdate()
    {
        // if not on it return
        if (Time.time % tickInterval <= Time.fixedDeltaTime) StartAnimation();
        else if ((Time.time - latency) % tickInterval <= Time.fixedDeltaTime) Tick();
    }

    private void StartAnimation()
    {
        // Check which tick we are on
        int tickIndex = (int)(Time.time / tickInterval);
        
        if (BeatMap[tickIndex % ticksPerBar] == '0')
        {
            //if next is a bar
            if (BeatMap[(tickIndex+1)%ticksPerBar] == '1')
            {
                StartCoroutine(Ring.Instance.PlayAnimation("PreTransition"));
            }
            else StartCoroutine(Ring.Instance.PlayAnimation("Tick"));
        }
        
        if (BeatMap[tickIndex % ticksPerBar] == '1' && tickIndex != 0)
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
            audioSource.PlayOneShot(BeatClip);
        }
        
        if (BeatMap[tickIndex % ticksPerBar] == '1' && tickIndex != 0)
        {
            audioSource.PlayOneShot(BarClip);
            PlayAction();
        }
    }

    private void PlayAction()
    {
        actionSequencer.PlayCurrentAction();
        actionSequencer.NextAction();
    }
}
