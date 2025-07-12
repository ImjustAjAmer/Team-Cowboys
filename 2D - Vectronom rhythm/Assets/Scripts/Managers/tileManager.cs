using UnityEditor.Build;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public bool isActive;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Show/hide tile based on active state
        sr.enabled = isActive; 
    }
}
