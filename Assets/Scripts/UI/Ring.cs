using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Ring : MonoBehaviour
{
    public static Ring Instance { get; private set; }

    private ActionBase[] actions;
    // private List<GameObject> slots;
    private int currentSlotIndex = 0;
    private int startAngle = 135;
    private float ringRaduis = 5f - 5f * 0.1f / 2;

    [SerializeField] public Animator animator;

    [SerializeField] public GameObject SlotPrefab;
    [SerializeField] public Sprite QuestionIcon;
    [HideInInspector] public List<Image> icons;
    public Image CenterImage;

    private Vector3 gamePos;
    private Vector3 gameScale;
    private Vector3 introPos;
    private Vector3 introScale;
    public float zoomScale = 4f;


    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Awake call destroy ! parent :  " + gameObject.transform.parent.name);
            Debug.Log("of " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        //clean children
        // slots = new List<GameObject>();
        foreach (Transform child in animator.transform)
        {
            Destroy(child.gameObject);
        }

        gamePos = transform.position;
        gameScale = transform.localScale;
        introPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        introScale = gameScale * zoomScale;

        transform.position = introPos;
        transform.localScale = introScale;
    }

    public void AddSlots(ActionBase[] actionSequence)
    {
        actions = actionSequence;

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
            float angle = -(i + 1) * (360f / actions.Length) + startAngle;
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);
            slot.transform.localPosition = new Vector3(x, y, 0) * ringRaduis;

            Image image = slot.GetComponent<Image>();
            image.sprite = QuestionIcon;
            icons.Add(image);

            // slots.Add(slot);
        }
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
        bool isIntro = Sequencer.Instance.isIntro;
        // Rotate the ring to 360f / SlotCount degrees in tickInterval *0.1
        float angle = 360f / icons.Count;
        float duration = Sequencer.Instance.tickInterval / 6f;
        float elapsed = 0f;

        Vector3 startRotation = transform.localEulerAngles;
        Vector3 endRotation = startRotation + new Vector3(0, 0, angle);

        while (elapsed < duration)
        {
            transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, elapsed / duration);
            foreach (var slot in icons)
                slot.transform.transform.localEulerAngles = -Vector3.Lerp(startRotation, endRotation, elapsed / duration);
            if (isIntro) CenterImage.transform.rotation = Quaternion.Euler(0, 0, 0);

            elapsed += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        icons[currentSlotIndex].transform.localScale = Vector3.one * 2;

        currentSlotIndex = (currentSlotIndex + 1 + icons.Count) % icons.Count;

        icons[currentSlotIndex].transform.localScale = Vector3.one * 3;
        //add border
        // icons[currentSlotIndex]

        if (icons[currentSlotIndex].sprite == QuestionIcon)
            icons[currentSlotIndex].sprite = actions[currentSlotIndex].ActionIcon;

        transform.localEulerAngles = endRotation;
        foreach (var slot in icons)
            slot.transform.transform.localEulerAngles = -endRotation;
        if (isIntro)
        {
            CenterImage.enabled = true;
            CenterImage.sprite = actions[currentSlotIndex].ActionIcon;
            CenterImage.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public IEnumerator IntroToGamePos()
    {
        CenterImage.enabled = false;
        float duration = Sequencer.Instance.tickInterval / 6f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(introPos, gamePos, elapsed / duration);
            transform.localScale = Vector3.Lerp(introScale, gameScale, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        transform.position = gamePos;
        transform.localScale = gameScale;

        // currentSlotIndex = 0;
    }

    public IEnumerator GameToIntroPos()
    {
        float duration = Sequencer.Instance.tickInterval / 6f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(gamePos, introPos, elapsed / duration);
            transform.localScale = Vector3.Lerp(gameScale, introScale, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    // public IEnumerator ShowIconOnebyOne()
    // { 
    //     icons[currentSlotIndex].sprite = actions[currentSlotIndex].ActionIcon;
    //     yield return null;
    // }
}
