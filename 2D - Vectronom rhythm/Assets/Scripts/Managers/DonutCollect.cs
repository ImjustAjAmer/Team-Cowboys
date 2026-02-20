using UnityEngine;

public class DonutCollect : MonoBehaviour
{
    public static DonutCollect instance;

    public int pointValue = 1;

    private DonutManager DM;

    void Awake()
    {
        instance = this;

        DM = FindFirstObjectByType<DonutManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            DM.AddToCounter(pointValue);

            DM.hasCollected = true;

            DM.RNGTransforms();

            Invoke("ResetCollection", 0.1f);

            Destroy(gameObject);

            Debug.Log("this code has been hit --donut collect--");
        }
    }

    void ResetCollection()
    {
        DM.hasCollected = false;
    }


}
