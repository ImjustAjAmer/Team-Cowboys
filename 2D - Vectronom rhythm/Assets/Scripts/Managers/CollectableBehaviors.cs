using UnityEngine;
using System.Collections;

public class CollectableBehaviors : MonoBehaviour
{
    public bool hasBeenCollected = false;

    public void Collect()
    {
        if (hasBeenCollected) return;

        hasBeenCollected = true;
        gameObject.SetActive(false);
        LevelManager.Instance.OnCollectibleCollected(this.gameObject);
    }

    public void ResetCollectible()
    {
        hasBeenCollected = false;
        //gameObject.SetActive(false);
        TryActivate(false);
    }

    public void TryActivate(bool tileIsActive)
    {
        // Show if this hasn't been collected and its tile is active
        gameObject.SetActive(tileIsActive && !hasBeenCollected);
    }
}
