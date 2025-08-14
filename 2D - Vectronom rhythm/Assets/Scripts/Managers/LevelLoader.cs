
using System.Collections;

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
    
}