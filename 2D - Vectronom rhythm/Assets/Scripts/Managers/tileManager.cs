using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    [ReadOnlyAtrribute] public bool isActive;
    [ReadOnlyAtrribute] public bool isAboutToBeActive = false;
    [ReadOnlyAtrribute] public bool isAboutToBeInactive = false;
    [ReadOnlyAtrribute] public bool isPlayerStanding;

    public SpriteRenderer tileVisual; // Child renderer
    public GameObject playerShadowPNG;
    public List<SpriteRenderer> fadeTargets = new List<SpriteRenderer>();

    private Vector3 originalScale;
    private Color originalColor;
    public float minScale = 0.3f;

    private float stateTimer = 0f;
    private float stateDuration = 1f;

    private SpriteRenderer sr;
    private LevelManager levelManager;
    private PlayerBehaviors playerBehaviors;


    [Header("Nice Timing")]
    public Color niceTimingColor = Color.yellow;
    public float niceTimingWindow = 0.15f;
    private bool wasPreviouslyActive = false;
    private bool isNiceTiming = false;
    private Coroutine niceTimingRoutine;

    public float transitionStartRatio = 0.1f;
    //private CollectableBehaviors collectible;

    private Color defaultColor;

    private void Awake()
    {
        originalScale = tileVisual.transform.localScale;
        originalColor = tileVisual.color;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        levelManager = LevelManager.Instance;
        playerBehaviors = FindFirstObjectByType<PlayerBehaviors>();

        if (sr != null)
            defaultColor = sr.color;

    }

    public void SetFadeInfo(float timeLeft, float totalDuration) //(float timer, float duration)
    {
        if (isActive && !isAboutToBeActive)
        {
            originalColor.a = LevelManager.Instance.globalMaxAlpha;
            originalScale = tileVisual.transform.localScale;
        }
        else if (!isActive && !isAboutToBeInactive)
        {
            originalColor.a = 0f;
            originalScale = Vector3.zero;
        }

        float t = Mathf.Clamp(transitionStartRatio - (timeLeft / totalDuration), 0.0f, 1.0f) / transitionStartRatio;

        float endingAlpha = 1.0f;
        Vector3 endingScale = Vector3.zero;

        if (isAboutToBeActive || isActive)
        {
            endingAlpha = LevelManager.Instance.aboutToBeActiveAlpha;
            endingScale = new Vector3(LevelManager.Instance.aboutToBeActiveScale, LevelManager.Instance.aboutToBeActiveScale, LevelManager.Instance.aboutToBeActiveScale);
        }
        else if (isAboutToBeInactive || !isActive)
        {
            endingAlpha = LevelManager.Instance.aboutToBeInactiveAlpha;
            endingScale = new Vector3(LevelManager.Instance.aboutToBeInactiveScale, LevelManager.Instance.aboutToBeInactiveScale, LevelManager.Instance.aboutToBeInactiveScale);
        }

        tileVisual.color = Color.Lerp(new Color(originalColor.r, originalColor.g, originalColor.b, LevelManager.Instance.globalMaxAlpha),
            new Color(originalColor.r, originalColor.g, originalColor.b, endingAlpha), t);

        tileVisual.transform.localScale = Vector3.Lerp(originalScale, endingScale, t);

        if (t >= 1f)
        {
            originalColor = tileVisual.color;
            originalScale = tileVisual.transform.localScale;
        }

        //return;
    }
}
