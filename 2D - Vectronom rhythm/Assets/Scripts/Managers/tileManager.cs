using UnityEditor.Build;
using UnityEngine;
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

    //private float maxAlpha = 1f;
    //private float minAlpha = 0.5f;

    //private SpriteRenderer[] edgePNGs;
    //private SpriteRenderer[] dividerPNGs;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        levelManager = FindObjectOfType<LevelManager>();
        //CacheEdgePNGs();
    }

    /*void CacheEdgePNGs()
    {
        var allChildren = GetComponentsInChildren<Transform>(includeInactive: true);
        var edgeList = new List<SpriteRenderer>();
        var dividerList = new List<SpriteRenderer>();

        foreach (var child in allChildren)
        {
            if (child.name.ToLower().Contains("edgepng"))
            {
                var sr = child.GetComponent<SpriteRenderer>();
                if (sr != null) edgeList.Add(sr);
            }
            else if (child.name.ToLower().Contains("dividerpng"))
            {
                var sr = child.GetComponent<SpriteRenderer>();
                if (sr != null) dividerList.Add(sr);
            }
        }

        edgePNGs = edgeList.ToArray();
        dividerPNGs = dividerList.ToArray();
    }*/

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

        // Set alpha on edge/divider PNGs
        foreach (var edge in GetComponentsInChildren<SpriteRenderer>(includeInactive: true))
        {
            if (edge == sr) continue;
            Color ec = edge.color;
            ec.a = alpha;
            edge.color = ec;
        }

        playerShadowPNG.SetActive(isPlayerStanding);
    }

    public void SetFadeInfo(float timer, float duration)
    {
        stateTimer = timer;
        stateDuration = duration;
        stateTimer = Mathf.Clamp(stateTimer, 0f, stateDuration);
    }

    void Update()
    {
        stateTimer -= Time.deltaTime;
        stateTimer = Mathf.Clamp(stateTimer, 0f, stateDuration);
        RefreshVisual();
    }
}
