using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemTransition : MonoBehaviour
{

    public static ItemTransition instance;
    public Animator playerAnimationController;

    private void Awake()
    {
        instance = this;
    }

    public void LoadScreen()
    {
        StartCoroutine(LoadPanel());
    }

    IEnumerator LoadPanel()
    {
        playerAnimationController.Play("360Pose");

        yield return new WaitForSeconds(1);

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
