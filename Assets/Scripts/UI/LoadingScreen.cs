using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private TextMeshProUGUI loadingText;
    private Image panel;
    
    public string LoadingText = "Loading...";
    public float FadeInDuration = 1f;
    public float LoadingDuration = 1f;
    public float FadeOutDuration = 1f;

    private void Awake()
    {
        loadingText = GetComponentInChildren<TextMeshProUGUI>();
        panel = GetComponent<Image>();
    }


    public IEnumerator ShowAndDisappear()
    {
        panel.color = Color.black;
        
        loadingText.text = LoadingText;
        
        // Fade in
        float elapsedTime = 0f;
        while (elapsedTime < FadeInDuration)
        {
            float alpha = Mathf.Clamp01(elapsedTime / FadeInDuration);
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, 1f);
        
        // Wait for the specified duration
        yield return new WaitForSeconds(LoadingDuration);
        
        // Fade out
        elapsedTime = 0f;
        while (elapsedTime < FadeOutDuration)
        {
            float alpha = Mathf.Clamp01(1f - (elapsedTime / FadeOutDuration));
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, alpha);
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0f);
        loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, 0f);
        
        Sequencer.Instance.StartIntro();
        
        gameObject.SetActive(false);
    }
}
