using UnityEditor.Build;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TileManager : MonoBehaviour
{
    [ReadOnlyAtrribute] public bool isActive;
    [ReadOnlyAtrribute] public bool isAboutToBeActive = false;
    [ReadOnlyAtrribute] public bool isAboutToBeInactive = false;

    [ReadOnlyAtrribute] public bool isPlayerStanding;

    private float stateTimer = 0f;
    private float stateDuration = 1f;

    private SpriteRenderer sr;
    public GameObject playerShadowPNG;

    private LevelManager levelManager;

    public List<SpriteRenderer> fadeTargets = new List<SpriteRenderer>();

    [Header("Visual Settings")]
    public float minScale = 0.3f;
    public Color activeColor = Color.white;
    public Color inactiveWarningColor = Color.red;
    public Color niceTimingColor = Color.yellow;

    [Header("Nice Timing")]
    public bool isNiceTiming = false;
    public float niceTimingWindow = 0.15f;
    public int niceTimingDamage = 25;

    private Coroutine niceTimingRoutine;
    private bool wasPreviouslyActive = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        levelManager = FindFirstObjectByType<LevelManager>();
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
        Color tileColor = activeColor;

        if (isNiceTiming)
        {
            alpha = max;
            scale = 1f;
            tileColor = niceTimingColor;
        }
        else if (!isActive)
        {
            alpha = 0f;
            scale = minScale;
            tileColor = inactiveWarningColor;
        }
        else
        {
            if (isAboutToBeInactive)
            {
                float t = 1f - (stateTimer / stateDuration);
                alpha = Mathf.Lerp(1f, min, t);
                scale = Mathf.Lerp(1f, minScale, t);
                tileColor = Color.Lerp(activeColor, inactiveWarningColor, t);
            }
            else
            {
                alpha = max;
                scale = 1f;
                tileColor = activeColor;
            }
        }

        // Set alpha on main tile
        Color c = tileColor;
        c.a = alpha;
        sr.color = c;
        transform.localScale = new Vector3(scale, scale, 1f);

        //gameObject.transform.localScale = new Vector3(scale, scale, 1f);
        transform.localScale = new Vector3(scale, scale, 1f);

        // Set alpha on fading children
        foreach (var child in fadeTargets)
        {
            if (child == null) continue;
            Color cc = child.color;
            cc.a = alpha;
            child.color = cc;
        }

        playerShadowPNG.SetActive(isPlayerStanding);
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

    IEnumerator NiceTimingWindow()
    {
        isNiceTiming = true;
        yield return new WaitForSeconds(niceTimingWindow);
        isNiceTiming = false;
    }

    void Update()
    {
        stateTimer -= Time.deltaTime;
        stateTimer = Mathf.Clamp(stateTimer, 0f, stateDuration);
        RefreshVisual();
    }
}
