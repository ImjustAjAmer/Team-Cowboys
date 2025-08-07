using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    [ReadOnlyAtrribute] public bool isActive;
    [ReadOnlyAtrribute] public bool isAboutToBeActive = false;
    [ReadOnlyAtrribute] public bool isAboutToBeInactive = false;
    [ReadOnlyAtrribute] public bool isPlayerStanding;
    [ReadOnlyAtrribute] public bool isPlayerJumping;

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

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        levelManager = LevelManager.Instance;
        playerBehaviors = FindFirstObjectByType<PlayerBehaviors>();
    }

    public void SetFadeInfo(float timer, float duration)
    {
        stateTimer = Mathf.Clamp(timer, 0f, duration);
        stateDuration = duration;

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
    }

    void RefreshVisual()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (levelManager == null) return;

        float max = levelManager.globalMaxAlpha;
        float min = levelManager.globalMinAlpha;

        float alpha = 1f;
        float scale = 1f;
        Color color = sr.color;

        if (isNiceTiming)
        {
            color = niceTimingColor;
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
                alpha = max;
                scale = 1f;
            }

            color = sr.color;
        }

        color.a = alpha;
        sr.color = color;
        transform.localScale = new Vector3(scale, scale, 1f);

        foreach (var child in fadeTargets)
        {
            if (child == null) continue;
            Color cc = child.color;
            cc.a = alpha;
            child.color = cc;
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
