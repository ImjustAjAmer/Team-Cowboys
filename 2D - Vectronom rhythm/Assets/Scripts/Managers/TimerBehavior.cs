using UnityEngine;
using TMPro;

using System.Collections;
using UnityEngine.SceneManagement;

public class TimerBehavior : MonoBehaviour
{

    public static TimerBehavior Instance;

    [SerializeField] TextMeshProUGUI timerText;
    public float remaingTime;
    private PlayerBehaviors playerBehaviors;
    private CollectableBehaviors collectableBehaviors;
    private bool isDead = false;
    public float deathFreezeDuration = 0.5f;
    private Quaternion originalRotation;
    public bool isTimeRunning = true;

    public SpriteRenderer playerSprite;

    public int secondsAdded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        playerBehaviors = GetComponent<PlayerBehaviors>();
        collectableBehaviors = GetComponent<CollectableBehaviors>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        if (isTimeRunning && remaingTime > 0)
        {
            remaingTime -= Time.deltaTime;
        }
        else
        {
            remaingTime = 0;

            isTimeRunning = false;

            HandleGameOver();
            StartCoroutine(HandleGameOver());
            UpdateTimerDisplay();
        }

        int minutes = Mathf.FloorToInt(remaingTime / 60);
        int seconds = Mathf.FloorToInt(remaingTime % 60);
        timerText.text = string.Format("{00:00}:{1:00}", minutes, seconds);
        
        
    }

    private void UpdateTimerDisplay()
    {
        if(timerText != null)
        {
            timerText.text = "time: " + Mathf.Ceil(remaingTime).ToString();
        }
    }

    public void addToTimer(float timeToAdd)
    {
        remaingTime += timeToAdd;
        UpdateTimerDisplay();
    }


    public IEnumerator HandleGameOver()
    {
        isDead = true;
        originalRotation = playerSprite.transform.rotation;

        // Flip the sprite upside down
        playerSprite.transform.rotation = Quaternion.Euler(0, 0, 180);

        // Darken the sprite
        //Color c = playerSprite.color;
        //c = Color.black;
        //playerSprite.color = c;

        // Freeze input
        yield return new WaitForSeconds(deathFreezeDuration);

        // Reset rotation (optional if you're reloading scene anyway)
        //playerSprite.transform.rotation = originalRotation;

        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
