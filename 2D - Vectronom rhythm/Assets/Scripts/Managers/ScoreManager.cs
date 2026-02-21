using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI donutCounter;

    public bool hasCollected = false;

    public int currentDonutCount = 0; //text counter

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentDonutCount = 0;
        UpdateDonutCounter();
    }

    public void AddToCounter(int amount)
    {
        currentDonutCount += amount;
        UpdateDonutCounter();
    }

    void UpdateDonutCounter()
    {
        donutCounter.text = " " + currentDonutCount.ToString();
    }

   
}
