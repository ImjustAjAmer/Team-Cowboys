using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectableBehaviors : MonoBehaviour
{
    //public float timeToAdd = 5f;

    public void Collect()
    {
        //TimerBehavior timer = FindAnyObjectByType<TimerBehavior>();
        //if (timer != null) timer.addToTimer(timeToAdd);

        Debug.Log("HIT!");
        Destroy(gameObject);
    }
}
