using System.Collections;
using UnityEngine;

public class ActionBerserker : ActionBase
{
    private SpriteRenderer playerSprite;
    private Transform foots;
    private Vector3 originalScale;

    public override void Init()
    {
        playerSprite = GameManager.Instance.player.GetComponent<SpriteRenderer>();
        foots = GameManager.Instance.player.transform.GetChild(0);
        originalScale = playerSprite.transform.localScale;
    }
    
    public override void HandleAction()
    {
        playerSprite.color = Color.red;
        playerSprite.transform.localScale *= 2f;
        playerSprite.GetComponent<Collider2D>().enabled = true;

        //change layer
        playerSprite.gameObject.layer = LayerMask.NameToLayer("Default");
        foots.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public override void PreAction() { return; }

    public override void PostAction()
    {
        playerSprite.color = Color.white;
        playerSprite.transform.localScale = originalScale;
        playerSprite.GetComponent<Collider2D>().enabled = false;
        playerSprite.gameObject.layer = LayerMask.NameToLayer("Player");
        foots.gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
