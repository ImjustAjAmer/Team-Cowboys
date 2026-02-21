using UnityEngine;
using TMPro;
public class LiveManager : MonoBehaviour
{
    public static LiveManager instance;

    public Transform[] tileTRMS;

    public TextMeshProUGUI donutCounter;

    public bool hasCollected = false;

    public int currentTRNSPICK = 0; //transform current pick

    public int currentDonutCount = 0; //text counter

    public GameObject donutPREFAB;

    public bool canSPAWN = false;

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
