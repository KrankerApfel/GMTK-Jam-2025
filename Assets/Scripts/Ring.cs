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
        
        // animator = GetComponent<Animator>();
        
        //clean children
        slots = new List<GameObject>();
        foreach (Transform child in animator.transform)
        {
            Destroy(child.gameObject);
        }

        // create slots
        for (int i = 0; i < SlotCount; i++)
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
        //     transform.Rotate(0, 0, -360f / SlotCount);
        // }
        // else if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     transform.Rotate(0, 0, 360f / SlotCount);
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

    public IEnumerator PlayAnimation(string animationName)
    {
        if (animator != null) animator.Play(animationName);
        return null;
    }

    public IEnumerator Rotate()
    {
        // Rotate the ring to 360f / SlotCount degrees in tickInterval *0.1
        float angle = 360f / SlotCount;
        float duration = Sequencer.Instance.tickInterval / 10f;
        float elapsed = 0f;
        
        Vector3 startRotation = transform.localEulerAngles;
        Vector3 endRotation = startRotation + new Vector3(0, 0, angle);
        
        
        while (elapsed < duration)
        {
            print(Vector3.Lerp(startRotation, endRotation, elapsed / duration));
            transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, elapsed / duration);

            foreach (var slot in slots)
            {
                slot.transform.localEulerAngles = -Vector3.Lerp(startRotation, endRotation, elapsed / duration);
            }
            
            elapsed += Time.deltaTime;
            //wait delta time
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.localEulerAngles = endRotation;
        foreach (var slot in slots)
        {
            slot.transform.localEulerAngles = -endRotation;
        }
        
        slots[currentSlotIndex].transform.localScale = Vector3.one * 2;
        currentSlotIndex = (currentSlotIndex - 1 + slots.Count) % slots.Count;
        slots[currentSlotIndex].transform.localScale = Vector3.one * 3;
    }

}
