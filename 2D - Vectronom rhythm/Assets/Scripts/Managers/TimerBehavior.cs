/*using UnityEngine;
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

    //private bool isDead = false;
    //public float deathFreezeDuration = 0.5f;

    private Quaternion originalRotation;
    public bool isTimeRunning = true;

    public SpriteRenderer playerSprite;

    //public int secondsAdded;

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
        //if (isDead) return;

        if (isTimeRunning && remaingTime > 0)
        {
            remaingTime -= Time.deltaTime;
        }
        else
        {
            remaingTime = 0;

            isTimeRunning = false;

            //HandleGameOver();
            //StartCoroutine(HandleGameOver());
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


}*/

using UnityEngine;
using TMPro;

using System.Collections;
using UnityEngine.SceneManagement;

public class TimerBehavior : MonoBehaviour
{
    [Header("Componets")]
    public TextMeshProUGUI timerText;
    
    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;

    void Start()
    {
        
    }

    void Update()
    {
        currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
        timerText.text = currentTime.ToString("[00:00]");
    }

}

