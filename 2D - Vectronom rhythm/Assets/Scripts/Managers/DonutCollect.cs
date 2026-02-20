using UnityEngine;

public class DonutCollect : MonoBehaviour
{
    public static DonutCollect instance;

    public int pointValue = 1;

    private DonutManager DM;

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            Debug.Log("this code has been hit --donut collect--");
        }
    }*/

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

            Debug.Log("this code has been hit --donut collect--");
        }
    }
}
