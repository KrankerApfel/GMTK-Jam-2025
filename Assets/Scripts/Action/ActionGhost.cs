using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionGhost : ActionBase
{
    [SerializeField]
    public GameObject playerColliderObject;     
    [SerializeField]
    public GameObject player;

    private Collision2D foot; 

    private int playerLayer_num;
    private int ghostLayer_num;

    public Color ghostColor = Color.cyan;
    public Color normalColor = Color.white;
    private SpriteRenderer spriteRenderer;

    private List<int> physicalLayers_num;

    void Awake()
    {
        Init();
        if (player == null)
        {
            Debug.LogError("Player not assigned.");
            return;
        }


        // list of layers whith disable collisions 
        physicalLayers_num = new List<int>();
        physicalLayers_num.Add(LayerMask.NameToLayer("Enemy"));
        physicalLayers_num.Add(LayerMask.NameToLayer("CrossableWalls"));

        int ghostLayer_num = LayerMask.NameToLayer("GhostLayer");

        // disable collisions
        foreach (int physicalLayer_num in physicalLayers_num)
        {
            Physics2D.IgnoreLayerCollision(ghostLayer_num, physicalLayer_num, true);
        }

    }

    public override void Init()
    {
        player = GameManager.Instance.player.gameObject;
        playerColliderObject = player.GetComponentInChildren<BoxCollider2D>().gameObject;
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        
        ghostLayer_num = LayerMask.NameToLayer("GhostLayer");
        playerLayer_num = LayerMask.NameToLayer("Player");
    }

    public override void HandleAction()
    {
        DisableCollision();
        OnActionFinished?.Invoke();

    }

    private void DisableCollision()
    {
        // change player collider to the ghostlayer
        playerColliderObject.layer = ghostLayer_num;
        spriteRenderer.color = ghostColor;
    }
    
    public override void PreAction() { return; }

    public override void PostAction()
    {
        // change player collider back to the playerLayer
        playerColliderObject.layer = playerLayer_num;
        spriteRenderer.color = normalColor;
    }
}