using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections;


public class DonutManager : MonoBehaviour
{
    public static DonutManager instance;

    public Transform[] tileTRMS;

    public bool hasCollected = false;

    public int currentTRNSPICK = 0; //transform current pick

    public GameObject donutPREFAB;

    private void Awake()
    {
        instance = this;
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
