using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CollectableBehaviors : MonoBehaviour
{
    public ItemTransition itemTransition;

    //Scene currentScene;
    //int currentSceneIndex = currentScene.buildIndex;

    /*public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger hit by: " + collision.gameObject.name + " with tag: " + collision.tag);

        currentScene = SceneManager.GetActiveScene();
        int currentSceneIndex = currentScene.buildIndex;

        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(++currentSceneIndex);
            Debug.Log("hit player");
        }
    }*/
    
    //make it work with the donut//

    public void LoadScene()
    {
        itemTransition.LoadScreen();
    }

    /*public void Collect()
    {
        currentScene = SceneManager.GetActiveScene();
        int currentSceneIndex = currentScene.buildIndex;

        SceneManager.LoadScene(++ currentSceneIndex);
        Debug.Log("hit player");
    }*/


    /*public void Collect()
    {
        currentScene = SceneManager.GetActiveScene();
        int currentSceneIndex = currentScene.buildIndex;

        SceneManager.LoadScene(currentSceneIndex++);
        Debug.Log("hit player");
    }*/

    /*public bool hasBeenCollected = false;

    public void Collect()
    {
        if (hasBeenCollected) return;

        hasBeenCollected = true;
        gameObject.SetActive(false);
        LevelManager.Instance.OnCollectibleCollected(this.gameObject);
    }

    public void ResetCollectible()
    {
        hasBeenCollected = false;
        //gameObject.SetActive(false);
        TryActivate(false);
    }

    public void TryActivate(bool tileIsActive)
    {
        // Show if this hasn't been collected and its tile is active
        gameObject.SetActive(tileIsActive && !hasBeenCollected);
    }*/
}
