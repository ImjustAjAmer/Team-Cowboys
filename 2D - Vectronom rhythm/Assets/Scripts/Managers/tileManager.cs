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

        if (!isActive)
        {
            if (isAboutToBeActive)
            {
                float t = 1f - (stateTimer / stateDuration);
                alpha = Mathf.Lerp(0f, max, t);
                scale = Mathf.Lerp(0f, 1f, t);
            }
            else
            {
                alpha = 0f;
                scale = 0f;
            }
        }
        else
        {
            if (isAboutToBeInactive)
            {
                float t = 1f - (stateTimer / stateDuration);
                alpha = Mathf.Lerp(1f, min, t);
                scale = Mathf.Lerp(1f, 0f, t);
            }
            else
            {
                alpha = 1f;
                scale = 1f;
            }
        }

        // Set alpha on main tile
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;

        gameObject.transform.localScale = new Vector3(scale, scale, 1f);

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
    }

    void Update()
    {
        stateTimer -= Time.deltaTime;
        stateTimer = Mathf.Clamp(stateTimer, 0f, stateDuration);
        RefreshVisual();
    }
}
