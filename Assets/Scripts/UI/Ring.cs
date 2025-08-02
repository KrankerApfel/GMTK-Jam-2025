using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ring : MonoBehaviour
{
    public static Ring Instance { get; private set; }

    private ActionBase[] actions;
    private int currentSlotIndex = 0;
    private int startAngle = 135;
    private float ringRadius = 5f - 5f * 0.1f / 2;

    [SerializeField] public Animator animator;
    [SerializeField] public GameObject SlotPrefab;
    [SerializeField] public Sprite QuestionIcon;
    [HideInInspector] public List<Image> icons = new List<Image>();
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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InitPositions();
    }

    private void InitPositions()
    {
        foreach (Transform child in animator.transform)
            Destroy(child.gameObject);

        gamePos = transform.position;
        gameScale = transform.localScale;
        introPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        introScale = gameScale * zoomScale;

        transform.position = introPos;
        transform.localScale = introScale;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ne vide plus les slots ici → le Sequencer gère ça
    }

    public void AddSlots(ActionBase[] actionSequence)
    {
        ResetSlots();

        actions = actionSequence;
        currentSlotIndex = actions.Length - 1;

        for (int i = 0; i < actions.Length; i++)
        {
            GameObject slot = Instantiate(SlotPrefab, animator.transform);
            slot.name = "Slot " + i;
            slot.transform.localScale = (i == currentSlotIndex) ? Vector3.one * 3 : Vector3.one * 2;

            float angle = -(i + 1) * (360f / actions.Length) + startAngle;
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);
            slot.transform.localPosition = new Vector3(x, y, 0) * ringRadius;

            Image image = slot.GetComponent<Image>();
            image.sprite = QuestionIcon;
            icons.Add(image);
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
        if (icons == null || icons.Count == 0 || actions == null || actions.Length == 0)
        {
            Debug.LogWarning("Rotate() appelé mais aucun slot n'est défini.");
            yield break;
        }

        bool isIntro = Sequencer.Instance.isIntro;
        float angle = 360f / icons.Count;
        float duration = Sequencer.Instance.tickInterval / 6f;
        float elapsed = 0f;

        Vector3 startRotation = transform.localEulerAngles;
        Vector3 endRotation = startRotation + new Vector3(0, 0, angle);

        while (elapsed < duration)
        {
            transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, elapsed / duration);
            foreach (var slot in icons)
                slot.transform.localEulerAngles = -Vector3.Lerp(startRotation, endRotation, elapsed / duration);
            if (isIntro) CenterImage.transform.rotation = Quaternion.Euler(0, 0, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        icons[currentSlotIndex].transform.localScale = Vector3.one * 2;
        currentSlotIndex = (currentSlotIndex + 1 + icons.Count) % icons.Count;
        icons[currentSlotIndex].transform.localScale = Vector3.one * 3;

        if (icons[currentSlotIndex].sprite == QuestionIcon)
            icons[currentSlotIndex].sprite = actions[currentSlotIndex].ActionIcon;

        transform.localEulerAngles = endRotation;
        foreach (var slot in icons)
            slot.transform.localEulerAngles = -endRotation;

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
            yield return null;
        }

        transform.position = gamePos;
        transform.localScale = gameScale;
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
            yield return null;
        }
    }

    public void ResetSlots()
    {
        foreach (Transform child in animator.transform)
            Destroy(child.gameObject);

        icons.Clear();
        actions = null;
        currentSlotIndex = 0;

        transform.position = introPos;
        transform.localScale = introScale;
        transform.localEulerAngles = Vector3.zero;

        if (CenterImage != null)
        {
            CenterImage.enabled = false;
            CenterImage.sprite = QuestionIcon;
            CenterImage.transform.rotation = Quaternion.identity;
        }
    }
}