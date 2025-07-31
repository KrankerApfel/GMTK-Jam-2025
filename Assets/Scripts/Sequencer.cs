using UnityEngine;

public class Sequencer : MonoBehaviour
{
    public static Sequencer Instance { get; private set; }
    
    public float tickInterval;
    private float barInterval;
    private AudioSource audioSource;
    
    public int BPM = 60;
    public int TicksPerBar = 4;
    public int BarCount = 4;
   
    public AudioClip BeatClip;
    public AudioClip BarClip;

    private ActionSequencer actionSequencer;
    
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
        audioSource = GetComponent<AudioSource>();
    }

    public void Init(ActionSequencer s)
    {
        actionSequencer = s;
    }

    void FixedUpdate()
    {
        // if not on it return
        if (Time.time % tickInterval > Time.fixedDeltaTime) return;
        
        // Check which tick we are on
        int tickIndex = (int)(Time.time / tickInterval);
        
        if (tickIndex % TicksPerBar != 0)
        {
            //play backwards
            // audioSource.clip = BeatClip;
            // audioSource.pitch = -1f;
            // audioSource.timeSamples = audioSource.clip.samples - 1;
            // audioSource.PlayCurrentAction();
            
            audioSource.PlayOneShot(BeatClip);
            
            //if next is a bar
            if ((tickIndex+1)%TicksPerBar == 0)
            {
                StartCoroutine(Ring.Instance.PlayAnimation("PreTransition"));
            }
            else StartCoroutine(Ring.Instance.PlayAnimation("Tick"));
        }
        
        if (tickIndex % TicksPerBar == 0 && tickIndex != 0)
        {
            audioSource.PlayOneShot(BarClip);
            // Ring.Instance.MoveToNextSlot();
            StartCoroutine(Ring.Instance.Rotate());
            StartCoroutine(Ring.Instance.PlayAnimation("PostTransition"));
        }
    }
}
