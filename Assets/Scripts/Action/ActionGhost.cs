using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionGhost : ActionBase
{
    [SerializeField]
    public GameObject playerColliderObject;     
    [SerializeField]
    public GameObject player;     

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
        player = GameObject.FindGameObjectWithTag("Player");
        playerColliderObject = player;
        spriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    public override void HandleAction()
    {


        StartCoroutine(DisableCollision());
        OnActionFinished?.Invoke();

    }

    private IEnumerator DisableCollision()
    {
        ghostLayer_num = LayerMask.NameToLayer("GhostLayer");

        playerLayer_num = LayerMask.NameToLayer("Player");



        // change player collider to the ghostlayer
        playerColliderObject.layer = ghostLayer_num;
        spriteRenderer.color = ghostColor;


        yield return new WaitForSeconds(0.7f * duration);
        // change player collider back to the playerLayer
        playerColliderObject.layer = playerLayer_num;
        spriteRenderer.color = normalColor;
        yield return new WaitForSeconds(0.3f*duration); // bug let some time for moving player to a different layer


    }
    
    public override void PreAction() { return; }
    public override void PostAction() { return; }
}