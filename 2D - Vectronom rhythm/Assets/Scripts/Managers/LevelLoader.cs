
/*using System.Collections;

using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelLoader : MonoBehaviour
{

    public GameObject loadingScreen;
    public Slider slider;


    public void LoadLevel(int levelIndex)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsynchronously(levelIndex));
        Time.timeScale = 1f;
    }

    IEnumerator LoadAsynchronously(int levelIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            yield return null;
        }
    }

}*/


using System.Collections;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    public int buildINDEX;

    [SerializeField] public Animator animator;

    public GameObject loadingPanel;

    public void Awake()
    {
        instance = this;
    }

    public void LoadScene()
    {
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        loadingPanel.SetActive(true);
        animator.SetTrigger("End");

        yield return new WaitForSeconds(1);

        SceneManager.LoadSceneAsync(buildINDEX);
        animator.SetTrigger("Start");

        yield return null;
    }

    /*IEnumerator LoadAsynchronously(int LevelIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(LevelIndex);

        while (!operation.isDone)
        {
            loadingPanel.SetActive(true);
            animator.SetTrigger("Start");
            yield return new WaitForSeconds(1);
            animator.SetTrigger("End");
            yield return null;
        }
    }*/

    /*IEnumerator LoadLevel()
    {
        
        yield return new WaitForSeconds(1);
        animator.SetTrigger("Start");
        
    }*/



}