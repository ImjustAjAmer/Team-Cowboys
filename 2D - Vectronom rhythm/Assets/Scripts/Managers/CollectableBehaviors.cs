using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectableBehaviors : MonoBehaviour
{
    //public float timeToAdd = 5f;
    public enum CollectibleTier { Tier1, Tier2, Tier3 }
    public CollectibleTier tier;

    private int GetDamage()
    {
        switch (tier)
        {
            case CollectibleTier.Tier1: return 10;
            case CollectibleTier.Tier2: return 50;
            case CollectibleTier.Tier3: return 100;
            default: return 0;
        }
    }

    public void Collect()
    {
        //TimerBehavior timer = FindAnyObjectByType<TimerBehavior>();
        //if (timer != null) timer.addToTimer(timeToAdd);

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.DealBossDamage(GetDamage());
        }

        //Debug.Log("HIT!");
        Destroy(gameObject);
    }
}
