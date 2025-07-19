using UnityEngine;
using TMPro;
using Mono.Cecil;

public class TimerBehavior : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float remaingTime;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(remaingTime > 0 )
        {
            remaingTime -= Time.deltaTime;
        }
        else
        {
            remaingTime = 0;
        }
       
        int minutes = Mathf.FloorToInt(remaingTime / 60);
        int seconds = Mathf.FloorToInt(remaingTime % 60);
        timerText.text = string.Format("{00:00}:{1:00}", minutes, seconds);
    }
}
