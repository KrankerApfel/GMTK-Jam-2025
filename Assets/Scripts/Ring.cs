using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ring : MonoBehaviour
{
    public static Ring Instance { get; private set; }
    
    private List<GameObject> slots;
    private int currentSlotIndex = 0;
    private int startAngle = 135;
    private float ringRaduis = 5f - 5f * 0.1f / 2;
    
    
    public GameObject SlotPrefab;
    public int SlotCount;
    
    
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
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < SlotCount; i++)
        {
            GameObject slot = Instantiate(SlotPrefab, transform);
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
            float angle = i * (360f / SlotCount) + startAngle;
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);
            slot.transform.localPosition = new Vector3(x, y, 0) * ringRaduis;
            
            Image image = slot.GetComponent<Image>();
            if (image != null)
            {
                // generate pastel color
                image.color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 1f);
            }
            
            slots.Add(slot);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     MoveToPreviousSlot();
        // }
        // else if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     MoveToNextSlot();
        // }
    }
    
    public void MoveToNextSlot()
    {
        slots[currentSlotIndex].transform.localScale = Vector3.one * 2;
        currentSlotIndex = (currentSlotIndex + 1) % slots.Count;
        slots[currentSlotIndex].transform.localScale = Vector3.one * 3;
        
        RotateRing(-360f / SlotCount);
    }
    
    private void MoveToPreviousSlot()
    {
        slots[currentSlotIndex].transform.localScale = Vector3.one * 2;
        currentSlotIndex = (currentSlotIndex - 1 + slots.Count) % slots.Count;
        slots[currentSlotIndex].transform.localScale = Vector3.one * 3;
        
        RotateRing(360f / SlotCount);
    }

    private void RotateRing(float angle)
    {
        // transform.Rotate(0, 0, angle);
        // startAngle += (int)angle;
        for (int i = 0; i < slots.Count; i++)
        {
            float newAngle = (currentSlotIndex - i) * (360f / SlotCount) + startAngle;
            float radians = newAngle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);
            slots[i].transform.localPosition = new Vector3(x, y, 0) * ringRaduis;
        }
    }

}
