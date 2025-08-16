using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;

    private Playercontrols playerControls;
    public bool GameIsPaused = false;

    private void Awake()
    {
        playerControls = new Playercontrols();

        playerControls.UI.Pause.performed += ctx => Pause();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void Pause()
    {
        GameIsPaused = true;

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

    }

    public void Resume()
    {
        GameIsPaused = false;

        pauseMenu.SetActive(false);

        Time.timeScale = 1f;
    }

    public void Restart()
    {

    }


}
