using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    [SerializeField]
    private ActionSequencer actionSequencer;

    private float beatInterval;
    private float barInterval;
    private AudioSource audioSource;

    public int BPM = 60;
    public int BeatsPerBar = 4;
    public int BarCount = 4;

    public AudioClip BeatClip;
    public AudioClip BarClip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beatInterval = 60f / BPM;
        barInterval = beatInterval * BeatsPerBar;
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // Check if it's time to play a beat or bar
        if (Time.time % beatInterval < Time.fixedDeltaTime && Time.time % barInterval >= Time.fixedDeltaTime)
        {
            //play backwards
            // audioSource.clip = BeatClip;
            // audioSource.pitch = -1f;
            // audioSource.timeSamples = audioSource.clip.samples - 1;
            // audioSource.PlayCurrentAction();

            audioSource.PlayOneShot(BeatClip);
        }

        if (Time.time % barInterval < Time.fixedDeltaTime && Time.time > 0)
        {
            audioSource.PlayOneShot(BarClip);
            PlayAction();
        }
    }

    private void PlayAction()
    {
        Ring.Instance.MoveToNextSlot();
        actionSequencer.PlayCurrentAction();
        actionSequencer.NextAction();
    }
}
