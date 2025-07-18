using UnityEditor.Build;
using UnityEngine;

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

    public float maxAlpha = 1f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void RefreshVisual()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();

        Color c = sr.color;

        if (!isActive)
        {
            if (isAboutToBeActive)
            {
                float t = 1f - (stateTimer / stateDuration);
                c.a = Mathf.Lerp(0f, maxAlpha, t);
            }
            else
            {
                c.a = 0f;
            }
        }
        else
        {
            if (isAboutToBeInactive)
            {
                // Fade out (1 → 0)
                float t = 1f - (stateTimer / stateDuration);
                c.a = Mathf.Lerp(maxAlpha, 0f, t);
            }
            else
            {
                c.a = 1f;
            }
        }

        sr.color = c;
        //float t = 1f - Mathf.Clamp01(stateTimer / stateDuration);
    }

    public void SetFadeInfo(float timer, float duration, float gloablMaxAlpha)
    {
        stateTimer = timer;
        stateDuration = duration;
        maxAlpha = gloablMaxAlpha;
    }

    void Update()
    {
        stateTimer -= Time.deltaTime;
        stateTimer = Mathf.Clamp(stateTimer, 0f, stateDuration);

        RefreshVisual();
    }

    /*void Update()
    {
        isPlayerStanding = false;

        // Show/hide tile based on active state
        //sr.enabled = isActive;
        Color c = sr.color;

        if (!isActive)
        {
            c.a = 0f;
        }
        else if (isAboutToBeInactive)
        {
            c.a = 0.5f;
        }
        else if (isAboutToBeActive)
        {
            c.a = 0.25f;
        }
        else
        {
            c.a = 1f;
        }

        sr.color = c;

        playerShadowPNG.SetActive(isPlayerStanding);
    }*/
}
