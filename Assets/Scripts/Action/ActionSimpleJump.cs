using System.Collections;
using UnityEngine;
public class ActionSimpleJump : ActionBase
{
    [Header("Jump Settings")]
    [SerializeField] private PlayerPhysics playerPhysics;
    [SerializeField] private float jumpForce = 25f;

    [Tooltip("Bloque temporairement les mouvements horizontaux du joueur")]
    [SerializeField] private bool disableMovementDuringJump = true;

    public override void HandleAction()
    {
        if (disableMovementDuringJump)
            playerPhysics.DisableMovement(true);

        playerPhysics.SetVelocity(new Vector2(playerPhysics.Velocity.x, jumpForce));
        StartCoroutine(FinishAfterDuration());
    }

    private IEnumerator FinishAfterDuration()
    {
        yield return new WaitForSeconds(duration);

        if (disableMovementDuringJump)
            playerPhysics.DisableMovement(false);

        OnActionFinished?.Invoke();
    }
}