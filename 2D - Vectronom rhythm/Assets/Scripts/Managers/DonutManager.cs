using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections;


public class DonutManager : MonoBehaviour
{
    public static DonutManager instance;

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

    public void RNGTransforms()
    {
        int randomPICK = UnityEngine.Random.Range(0, tileTRMS.Length);

        Transform randomINDEXPICK = tileTRMS[randomPICK];

        currentTRNSPICK = randomPICK;

        Instantiate(donutPREFAB, randomINDEXPICK.position, randomINDEXPICK.rotation);

        Debug.Log("random pick: " + randomINDEXPICK.name);
    }


}
