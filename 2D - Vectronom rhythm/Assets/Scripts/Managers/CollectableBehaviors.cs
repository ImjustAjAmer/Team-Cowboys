using UnityEngine;

public class CollectableBehaviors : MonoBehaviour
{
    public bool HasBeenCollected { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (HasBeenCollected) return;

        if (other.CompareTag("Player"))
        {
            HasBeenCollected = true;
            gameObject.SetActive(false);
            LevelManager.Instance.OnCollectibleCollected(gameObject);
        }
    }

    public void ResetCollectible()
    {
        HasBeenCollected = false;
        gameObject.SetActive(false);
    }
}
