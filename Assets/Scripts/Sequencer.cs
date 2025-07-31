using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    public static Sequencer Instance { get; private set; }
    
    [SerializeField]
    private ActionSequencer actionSequencer;
    
    public float tickInterval;
    private float barInterval;
    private float latency;
    private AudioSource audioSource;

    public int BPM = 60;
    public int TicksPerBar = 4;
    public int BarCount = 4;

    public AudioClip BeatClip;
    public AudioClip BarClip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        tickInterval = 60f / BPM;
        barInterval = tickInterval * TicksPerBar;
        latency = tickInterval / 6f;
        audioSource = GetComponent<AudioSource>();
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
        
        if (tickIndex % TicksPerBar != 0)
        {
            //if next is a bar
            if ((tickIndex+1)%TicksPerBar == 0)
            {
                StartCoroutine(Ring.Instance.PlayAnimation("PreTransition"));
            }
            else StartCoroutine(Ring.Instance.PlayAnimation("Tick"));
        }
        
        if (tickIndex % TicksPerBar == 0 && tickIndex != 0)
        {
            StartCoroutine(Ring.Instance.Rotate());
            StartCoroutine(Ring.Instance.PlayAnimation("PostTransition"));
        }
    }

    private void Tick()
    {
        int tickIndex = (int)(Time.time / tickInterval);
        
        if (tickIndex % TicksPerBar != 0)
        {
            audioSource.PlayOneShot(BeatClip);
        }
        
        if (tickIndex % TicksPerBar == 0 && tickIndex != 0)
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
