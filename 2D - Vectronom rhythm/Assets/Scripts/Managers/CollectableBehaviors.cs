using Unity.VisualScripting;
using UnityEngine;

public class CollectableBehaviors : MonoBehaviour
{
    public float timeToAdd = 5f;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            TimerBehavior timer = FindAnyObjectByType<TimerBehavior>();
            if(timer != null)
            {
                timer.addToTimer(timeToAdd);
                addToScore();
            }
        }

        Destroy(gameObject);
    }

    public void addToScore()
    {


    }

}
