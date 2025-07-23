using UnityEditor.Build;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TileManager : MonoBehaviour
{
    public bool isActive;
    [ReadOnlyAtrribute] public bool isPlayerStanding;
    [ReadOnlyAtrribute] public bool isAboutToBeActive = false;
    [ReadOnlyAtrribute] public bool isAboutToBeInactive = false;

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
        //levelManager = FindObjectOfType<LevelManager>();
    }


    public void RefreshVisual()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();

        float max = levelManager.globalMaxAlpha;
        float min = levelManager.globalMinAlpha;

        float alpha = 1f;

        if (!isActive)
        {
            if (isAboutToBeActive)
            {
                float t = 1f - (stateTimer / stateDuration);
                alpha = Mathf.Lerp(0f, max, t);
            }
            else
            {
                alpha = 0f;
            }
        }
        else
        {
            if (isAboutToBeInactive)
            {
                float t = 1f - (stateTimer / stateDuration);
                alpha = Mathf.Lerp(1f, min, t);
            }
            else
            {
                alpha = 1f;
            }
        }

        // Set alpha on main tile
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;

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
