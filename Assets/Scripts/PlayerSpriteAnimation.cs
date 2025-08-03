using UnityEngine;

public class PlayerSpriteAnimator : MonoBehaviour
{
    [Header("Animations")]
    public Sprite[] idleFrames;
    public Sprite[] runFrames;
    public Sprite[] jumpFrames;

    [Header("Frame Rates")]
    public float idleFrameRate = 0.1f; 
    public float runFrameRate = 0.01f; 
    public float jumpFrameRate = 0;

    private SpriteRenderer sr;
    private PlayerInputs inputs;
    private Rigidbody2D rb;

    private Sprite[] currentFrames;
    private float currentFrameRate;
    private int currentFrame;
    private float timer;

    private enum State { Idle, Run, Jump }
    private State currentState;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        inputs = GetComponent<PlayerInputs>();
        rb = GetComponent<Rigidbody2D>();

        ChangeAnimation(idleFrames, idleFrameRate); // Par défaut, Idle
    }

    void Update()
    {
        // Déterminer l'état
        if (Mathf.Abs(rb.linearVelocity.y) > 0.01f)
        {
            SetState(State.Jump);
        }
        else if (Mathf.Abs(inputs.Horizontal) > 0.01f)
        {
            SetState(State.Run);
        }
        else
        {
            SetState(State.Idle);
        }

        // Avance de l'animation
        timer += Time.deltaTime;
        if (timer >= currentFrameRate && currentFrames.Length > 0)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % currentFrames.Length;
            sr.sprite = currentFrames[currentFrame];
        }

        // Flip horizontal
        if (inputs.Horizontal != 0)
        {
            sr.flipX = inputs.Horizontal < 0;
        }
    }

    private void SetState(State newState)
    {
        if (newState != currentState)
        {
            currentState = newState;
            switch (currentState)
            {
                case State.Idle:
                    ChangeAnimation(idleFrames, idleFrameRate);
                    break;
                case State.Run:
                    ChangeAnimation(runFrames, runFrameRate);
                    break;
                case State.Jump:
                    ChangeAnimation(jumpFrames, jumpFrameRate);
                    break;
            }
        }
    }

    private void ChangeAnimation(Sprite[] frames, float frameRate)
    {
        if (frames == null || frames.Length == 0) return;

        currentFrames = frames;
        currentFrameRate = frameRate;
        currentFrame = 0;
        timer = 0f;
        sr.sprite = currentFrames[0];
    }
}
