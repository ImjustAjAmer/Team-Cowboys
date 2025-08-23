using UnityEngine;

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

    //private Vector3 originalScale;
    //private Color originalColor;
    public float minScale = 0.3f;
    public float maxScale = 0.5f;
    public float minAlpha = 0.5f;
    public float maxAlpha = 0.2f;

    private float stateTimer = 0f;
    private float stateDuration = 1f;

    private SpriteRenderer sr;
    private LevelManager levelManager;
    private PlayerBehaviors playerBehaviors;

    public Color aboutToBeInactiveColor = Color.red;
    public Color ogColor = Color.yellow;

    public float transitionOffset = 0.5f;

    //[Header("Nice Timing")]
    //public Color niceTimingColor = Color.yellow;
    //public float niceTimingWindow = 0.15f;
    //private bool wasPreviouslyActive = false;
    //private bool isNiceTiming = false;
    //private Coroutine niceTimingRoutine;

    //public float transitionStartRatio = 0.1f;
    //private CollectableBehaviors collectible;

    //private Color defaultColor;

    /*private void Awake()
    {
        originalScale = tileVisual.transform.localScale;
        originalColor = tileVisual.color;
    }*/

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        levelManager = LevelManager.Instance;
        playerBehaviors = FindFirstObjectByType<PlayerBehaviors>();

        //if (sr != null)
            //defaultColor = sr.color;
    }

    public void RefreshVisual()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();

        if (levelManager == null)
        {
            levelManager = FindFirstObjectByType<LevelManager>();
            if (levelManager == null)
            {
                return; // Skip this frame; it will retry next frame
            }
        }

        float max = levelManager.globalMaxAlpha;
        float min = levelManager.globalMinAlpha;

        float alpha = 1f;
        float scale = 1f;

        if (!isActive)
        {
            if (isAboutToBeActive)
            {
                float t = 1f - (stateTimer / stateDuration);
                //float t = transitionOffset - (stateTimer / stateDuration);
                
                //alpha = Mathf.Lerp(0f, max, t);
                alpha = Mathf.Lerp(0f, maxAlpha, t);

                scale = Mathf.Lerp(minScale, maxScale, t);

                //scale = Mathf.Lerp(0f, 1f, t);
                //scale = Mathf.Lerp(0f, maxScale, t);
            }
            else
            {
                alpha = 0f;
                scale = minScale;
            }
        }
        else
        {
            if (isAboutToBeInactive)
            {
                float t = 1f - (stateTimer / stateDuration);
                //alpha = Mathf.Lerp(1f, min, t);
                alpha = Mathf.Lerp(1f, minAlpha, t);
                //scale = Mathf.Lerp(1f, minScale, t);

                //sr.color = aboutToBeInactiveColor;
                sr.color = Color.Lerp(ogColor, aboutToBeInactiveColor, t);
            }
            else
            {
                alpha = 1f;
                scale = 1f;

                sr.color = ogColor;
            }
        }

        //just comemtned out

        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
        transform.localScale = new Vector3(scale, scale, 1f);

        // Set alpha on fading children
        foreach (var child in fadeTargets)
        {
            if (child == null) continue;
            Color cc = child.color;
            cc.a = alpha;
            child.color = cc;
        }

        playerShadowPNG.SetActive(isPlayerStanding && !playerBehaviors.isJumping);
    }

    public void SetFadeInfo(float timer, float duration)
    {
        stateTimer = Mathf.Clamp(timer, 0f, duration);
        stateDuration = duration;
    }

    void Update()
    {
        stateTimer -= Time.deltaTime;
        stateTimer = Mathf.Clamp(stateTimer, 0f, stateDuration);
        RefreshVisual();
    }

    /*public void SetFadeInfo(float timeLeft, float totalDuration) //(float timer, float duration)
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
    }*/
}
