using UnityEditor.Build;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public bool isActive;
    [ReadOnlyAtrribute] public bool isPlayerStanding;
    [ReadOnlyAtrribute] public bool isAboutToBeActive = false;
    [ReadOnlyAtrribute] public bool isAboutToBeInactive = false;

    private SpriteRenderer sr;
    public GameObject playerShadowPNG;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void RefreshVisual()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();

        Color c = sr.color;

        if (isAboutToBeActive)
        {
            c.a = 0.25f;
        }
        else if (isAboutToBeInactive)
        {
            c.a = 0.5f;
        }
        else if (!isActive)
        {
            c.a = 0f;
        }
        else
        {
            c.a = 1f;
        }

        sr.color = c;
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
