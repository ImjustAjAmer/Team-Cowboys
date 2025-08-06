using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectableBehaviors : MonoBehaviour
{
    private bool collected = false;

    public void Collect()
    {
        if (collected) return;

        collected = true;
        gameObject.SetActive(false);
        LevelManager.Instance.OnCollectibleCollected();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }
}
