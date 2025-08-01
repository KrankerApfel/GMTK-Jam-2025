using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Ring : MonoBehaviour
{
    public static Ring Instance { get; private set; }
    
    private List<GameObject> slots;
    private int currentSlotIndex = 0;
    private int startAngle = 135;
    private float ringRaduis = 5f - 5f * 0.1f / 2;
    
    public Animator animator;
    
    public GameObject SlotPrefab;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        //clean children
        slots = new List<GameObject>();
        foreach (Transform child in animator.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddSlots(ActionBase[] actions)
    {
        currentSlotIndex = actions.Length - 1;
        
        // create slots
        for (int i = 0; i < actions.Length; i++)
        {
            GameObject slot = Instantiate(SlotPrefab, animator.transform);
            slot.name = "Slot " + i;

            if (i == currentSlotIndex)
            {
                slot.transform.localScale = Vector3.one * 3;
            }
            else
            {
                slot.transform.localScale = Vector3.one * 2;
            }

            // put slots equally spaced in a circle
            float angle = - (i+1) * (360f / actions.Length) + startAngle;
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);
            slot.transform.localPosition = new Vector3(x, y, 0) * ringRaduis;

            Image image = slot.GetComponent<Image>();
            if (image != null)
            {
                // generate pastel color
                // image.color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 1f);
                image.sprite = actions[i].ActionIcon;
            }

            slots.Add(slot);
            // slots.Insert(0, slot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     transform.Rotate(0, 0, -360f / SlotCount);
        // }
        // else if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     transform.Rotate(0, 0, 360f / SlotCount);
        // }
    }

    public IEnumerator PlayAnimation(string animationName)
    {

        if (animator != null)
        {
            animator.speed = Sequencer.Instance.BPM / 60f;
            animator.Play(animationName);
        }
        yield return null;
    }

    public IEnumerator Rotate()
    {
        // Rotate the ring to 360f / SlotCount degrees in tickInterval *0.1
        float angle = 360f / slots.Count;
        float duration = Sequencer.Instance.tickInterval / 10f;
        float elapsed = 0f;
        
        Vector3 startRotation = transform.localEulerAngles;
        Vector3 endRotation = startRotation + new Vector3(0, 0, angle);
        
        
        while (elapsed < duration)
        {
            transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, elapsed / duration);

            foreach (var slot in slots)
            {
                slot.transform.localEulerAngles = -Vector3.Lerp(startRotation, endRotation, elapsed / duration);
            }
            
            elapsed += Time.deltaTime;
            //wait delta time
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        slots[currentSlotIndex].transform.localScale = Vector3.one * 2;
        currentSlotIndex = (currentSlotIndex + 1 + slots.Count) % slots.Count;
        slots[currentSlotIndex].transform.localScale = Vector3.one * 3;
        
        transform.localEulerAngles = endRotation;
        foreach (var slot in slots)
        {
            slot.transform.localEulerAngles = -endRotation;
        }
    }

}
