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

    private Vector3 originalScale;
    private Color originalColor;

    private float stateTimer = 0f;
    private float stateDuration = 1f;

    private SpriteRenderer sr;
    public GameObject playerShadowPNG;

    private LevelManager levelManager;
    private PlayerBehaviors playerBehaviors;

    public List<SpriteRenderer> fadeTargets = new List<SpriteRenderer>();
    public float minScale = 0.3f;

    [Header("Nice Timing")]
    public Color niceTimingColor = Color.yellow;
    public float niceTimingWindow = 0.15f;
    private Coroutine niceTimingRoutine;
    private bool wasPreviouslyActive = false;
    private bool isNiceTiming = false;

    private CollectableBehaviors collectible;

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

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Collectible"))
            {
                collectible = child.GetComponent<CollectableBehaviors>();
                break;
            }
        }
    }

    public void SetFadeInfo(float timeLeft, float totalDuration) //(float timer, float duration)
    {
        /*if (isAboutToBeActive)
        {
            float t = 1f - (timeLeft / totalDuration);
            tileVisual.color = Color.Lerp(
                new Color(originalColor.r, originalColor.g, originalColor.b, LevelManager.Instance.aboutToBeActiveAlpha),
                new Color(originalColor.r, originalColor.g, originalColor.b, LevelManager.Instance.globalMaxAlpha),
                t
            );
            tileVisual.transform.localScale = Vector3.Lerp(
                LevelManager.Instance.aboutToBeActiveScale,
                originalScale,
                t
            );
        }
        else if (isAboutToBeInactive)
        {
            tileVisual.color = new Color(originalColor.r, originalColor.g, originalColor.b, LevelManager.Instance.aboutToBeInactiveAlpha);
            tileVisual.transform.localScale = LevelManager.Instance.aboutToBeInactiveScale;
        }
        else if (!isActive)
        {
            tileVisual.color = new Color(originalColor.r, originalColor.g, originalColor.b, LevelManager.Instance.globalMinAlpha);
        }
        else
        {
            tileVisual.color = new Color(originalColor.r, originalColor.g, originalColor.b, LevelManager.Instance.globalMaxAlpha);
            tileVisual.transform.localScale = originalScale;
        }*/

        //stateTimer = Mathf.Clamp(timer, 0f, duration);
        //stateDuration = duration;

        if (!wasPreviouslyActive && isActive)
        {
            if (niceTimingRoutine != null) StopCoroutine(niceTimingRoutine);
            niceTimingRoutine = StartCoroutine(NiceTimingWindow());
        }

        wasPreviouslyActive = isActive;
    }

    void Update()
    {
        stateTimer -= Time.deltaTime;
        stateTimer = Mathf.Clamp(stateTimer, 0f, stateDuration);
        RefreshVisual();

        if (collectible != null)
        {
            collectible.TryActivate(isActive);
        }
    }

    void RefreshVisual()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (levelManager == null) return;

        float max = levelManager.globalMaxAlpha;
        float min = levelManager.globalMinAlpha;

        float alpha = 1f;
        float scale = 1f;
        Color baseColor = defaultColor;

        if (isNiceTiming)
        {
            baseColor = niceTimingColor;
            alpha = max;
            scale = 1f;
        }
        else if (!isActive)
        {
            if (isAboutToBeActive)
            {
                float t = 1f - (stateTimer / stateDuration);
                alpha = Mathf.Lerp(0f, max, t);
                scale = Mathf.Lerp(minScale, 1f, t);
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
                alpha = Mathf.Lerp(max, min, t);
                scale = Mathf.Lerp(1f, minScale, t);
            }
            else
            {
                //alpha = max;
                alpha = 1f;
                scale = 1f;
            }
        }

        baseColor.a = alpha;
        sr.color = baseColor;
        transform.localScale = new Vector3(scale, scale, 1f);

        foreach (var child in fadeTargets)
        {
            if (child == null) continue;
            Color childColor = child.color;
            childColor.a = alpha;
            child.color = childColor;
        }

        if (playerShadowPNG != null)
            playerShadowPNG.SetActive(isPlayerStanding && !playerBehaviors.isJumping);
    }

    IEnumerator NiceTimingWindow()
    {
        isNiceTiming = true;
        yield return new WaitForSeconds(niceTimingWindow);
        isNiceTiming = false;
    }
}
