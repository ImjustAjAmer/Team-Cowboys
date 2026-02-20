using UnityEngine;
using TMPro;


public class DonutManager : MonoBehaviour
{
    public static DonutManager instance;

    public Transform[] tileTRMS;

    public TextMeshProUGUI donutCounter;

    public int currentDonutCount = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentDonutCount = 0;
        UpdateDonutCounter();
    }

    void Update()
    {
        
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
