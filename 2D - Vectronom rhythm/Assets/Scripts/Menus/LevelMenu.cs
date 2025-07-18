using System.Collections;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class LevelMenu : MonoBehaviour
{
    public void OpenLevel(int levelID)
    {
        string levelName = "Level " + levelID;
        SceneManager.LoadScene(levelName); 
    }

}


