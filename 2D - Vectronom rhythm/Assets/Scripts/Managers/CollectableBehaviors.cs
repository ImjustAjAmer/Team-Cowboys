using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectableBehaviors : MonoBehaviour
{
    //[ReadOnlyAtrribute] public static int collectedCount = 0;
    //public static int collectThreshold = 3;

    //public GameObject currentLevelManager;
    //public GameObject nextLevelManager;

    public float timeToAdd = 5f;

    //public LevelManager levelManager;

    //private bool collected = false;

    public void Collect()
    {
        TimerBehavior timer = FindAnyObjectByType<TimerBehavior>();
        if (timer != null)
        {
            timer.addToTimer(timeToAdd);
        }
        Destroy(gameObject);

        /*if (collected) return;
        collected = true;

        if (levelManager != null)
        {
            levelManager.NotifyCollectibleCollected(this);
        }

        Destroy(gameObject);

        /*if (collected) return;
        collected = true;

        LevelManager levelManager = FindActiveLevelManager();
        if (levelManager != null)
        {
            levelManager.NotifyCollectibleCollected(this);
        }

        Destroy(gameObject);*/

        /*collectedCount++;
        gameObject.SetActive(false); // Or Destroy(gameObject);

        if (collectedCount >= collectThreshold)
        {
            ProgressToNextLevel();
        }

        TimerBehavior timer = FindAnyObjectByType<TimerBehavior>();
        if (timer != null)
        {
            timer.addToTimer(timeToAdd);
        }*/
    }

    /*private LevelManager FindActiveLevelManager()
    {
        LevelManager[] managers = FindObjectsOfType<LevelManager>(true);
        foreach (var manager in managers)
        {
            if (manager.gameObject.activeInHierarchy) return manager;
        }
        return null;
    }

    void ProgressToNextLevel()
    {
        if (currentLevelManager != null) currentLevelManager.SetActive(false);
        if (nextLevelManager != null) nextLevelManager.SetActive(true);

        // Restart level with new manager active
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }*/

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            TimerBehavior timer = FindAnyObjectByType<TimerBehavior>();
            if(timer != null)
            {
                timer.addToTimer(timeToAdd);
            }
        }

        Destroy(gameObject);
    }*/
}
