using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    public GameObject BeatMap;

    public GameObject Ring;

    public GameObject Menu;
    public GameObject Loading;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        BeatMap.SetActive(true);
    }

    public void ShowLoadingScreen()
    {
        Loading.SetActive(true);
        StartCoroutine(Loading.GetComponent<LoadingScreen>().ShowAndDisappear());
    }

    public void ShowIntro()
    {
        BeatMap.SetActive(true);
        // Ring.SetActive(true);
    }
    
    public void FinishIntro()
    {
        BeatMap.SetActive(false);
        StartCoroutine(Ring.GetComponent<Ring>().IntroToGamePos());
    }

    // public void Show(string name)
    // {
    //     switch (name)
    //     {
    //         case "BeatMap": BeatMap.SetActive(true); break;
    //         case "Ring": Ring.SetActive(true); break;
    //         case "Menu": Menu.SetActive(true); break;
    //         case "Loading": Loading.SetActive(true); break;
    //     }
    // }
    //
    // public void Hide(string name)
    // {
    //     switch (name)
    //     {
    //         case "BeatMap": BeatMap.SetActive(false); break;
    //         case "Ring": Ring.SetActive(false); break;
    //         case "Menu": Menu.SetActive(false); break;
    //         case "Loading": Loading.SetActive(false); break;
    //     }
    // }
}
