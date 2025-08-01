using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatMap : MonoBehaviour
{
    public GameObject TickDot;

    public GameObject BarDot;
    
    public List<Image> dots;

    private int currentDot = -1;
    private int lastDot = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string map = Sequencer.Instance.BeatMap;
        for(int i = 0; i < map.Length; i++)
        {
            GameObject dot;
            if(map[i] == '0') dot = Instantiate(TickDot, transform);
            else dot = Instantiate(BarDot, transform);
            
            dots.Add(dot.GetComponent<Image>());
        }
        
    }

    public void Advance()
    {
        if(currentDot >= 0)
        {
            lastDot = currentDot;
        }
        currentDot = (currentDot + 1) % dots.Count;
        
        dots[currentDot].color = Color.black;
        if (lastDot >= 0)
        {
            dots[lastDot].color = Color.white;
        }
    }
}
