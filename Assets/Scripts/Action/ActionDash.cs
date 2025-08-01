using UnityEngine;
using System.Collections;

public class ActionDash : ActionBase
{
    [Header("Physics")]
    [SerializeField] private PlayerPhysics playerPhysics;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private Vector2 direction = Vector2.right;

    [SerializeField]
    private AnimationCurve dashCurve = new AnimationCurve(
    new Keyframe(0f, 1f, 0f, 0f),
    new Keyframe(0.05f, 1f),
    new Keyframe(0.12f, 0.6f),
    new Keyframe(0.18f, 0f, -10f, -10f)
);


    private Coroutine dashCoroutine;

    public override void HandleAction()
    {
        if (dashCoroutine != null)
            StopCoroutine(dashCoroutine);

        playerPhysics.DisableMovement(true);
        dashCoroutine = StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float speedFactor = dashCurve.Evaluate(t);
            Vector2 velocity = direction.normalized * dashSpeed * speedFactor;

            playerPhysics.SetVelocity(velocity);

            elapsed += Time.deltaTime;
            yield return null;
        }

        playerPhysics.SetVelocity(Vector2.zero);
        playerPhysics.DisableMovement(false);
        OnActionFinished?.Invoke();
        dashCoroutine = null;
    }
}
