using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemTransition : MonoBehaviour
{
    public static ItemTransition instance;
    [SerializeField] Animator transitionController;
    public GameObject loadingPanel;
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
        loadingPanel.SetActive(true);

        transitionController.SetTrigger("End");

        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        transitionController.SetTrigger("Start");
        
    }
}
