using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelState
{
    public float baseDuration; 
    [HideInInspector] public float adjustedDuration;

    public List<bool> tileActivationStates;

    public int sfxIndex = -1; // -1 = no sound
}

[System.Serializable]
public class LevelSection
{
    public string name; 
    public List<LevelState> states;

    [Tooltip("Matches allTiles index. True = tile has a collectible this section.")]
    public List<bool> collectibleTileBools = new List<bool>();
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public List<TileManager> allTiles;

    public LevelSection[] sections;

    public float initialSpeedMultiplier = 1f; 
    public float speedMultiplierAdjuster = 0.85f;

    private int currentSectionIndex = 0;
    private int currentStateIndex = 0;
    private float currentStateTimer;

    public string nextLevelSceneName;

    public AudioClip[] stateSFX; 
    public AudioSource audioSource;

    [Range(0f, 1f)] public float globalMaxAlpha = 1f;
    [Range(0f, 1f)] public float globalMinAlpha = 0.5f;
    //[Range(0f, 1f)] public float minScale = 0.3f;

    [Header("Collectible Progression")]
    [ReadOnlyAtrribute] public int totalCollectiblesInSection;
    [ReadOnlyAtrribute] public int collectedInSection;
    public TMPro.TextMeshProUGUI collectibleCounterText;
    public TMPro.TextMeshProUGUI multiplierText;
    public float niceTimingMultiplierBonus = 0.25f;
    public float minSpeedMultiplier = 0.3f;
    public float niceTimingCooldown = 0f; // 0 = no cooldown
    private float niceTimingTimer = 0f;

    [ReadOnlyAtrribute] public float scoreMultiplier = 1f;


    void Start()
    {
        ApplySpeedMultiplier();
        SetState(currentSectionIndex, currentStateIndex);
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        currentStateTimer -= Time.deltaTime;

        if (currentStateTimer <= 0f)
        {
            AdvanceState();
        }

        float currentDuration = sections[currentSectionIndex].states[currentStateIndex].adjustedDuration;

        foreach (TileManager tile in allTiles)
        {
            if (tile != null)
                tile.SetFadeInfo(currentStateTimer, currentDuration);
        }

        if (niceTimingCooldown > 0f && niceTimingTimer > 0f)
        {
            niceTimingTimer -= Time.deltaTime;
        }
    }

    public void OnNiceTimingSuccess()
    {
        if (niceTimingCooldown > 0f && niceTimingTimer > 0f) return;

        scoreMultiplier += niceTimingMultiplierBonus;
        niceTimingTimer = niceTimingCooldown;

        UpdateCollectibleUI();
    }

    public void OnCollectibleCollected()
    {
        collectedInSection++;
        UpdateCollectibleUI();

        initialSpeedMultiplier = Mathf.Max(initialSpeedMultiplier * speedMultiplierAdjuster, minSpeedMultiplier);
        ApplySpeedMultiplier();
    }

    void UpdateCollectibleUI()
    {
        if (collectibleCounterText != null)
        {
            collectibleCounterText.text = $"{collectedInSection}/{totalCollectiblesInSection}";
        }

        if (multiplierText != null)
        {
            multiplierText.text = $"x{scoreMultiplier:F2}";
        }
    }

    void ApplySpeedMultiplier()
    {
        foreach (var section in sections)
        {
            foreach (var state in section.states)
            {
                state.adjustedDuration = state.baseDuration * initialSpeedMultiplier;
            }
        }
    }

    void SetState(int sectionIndex, int stateIndex)
    {
        LevelState state = sections[sectionIndex].states[stateIndex];
        currentStateTimer = state.adjustedDuration;

        if (stateIndex == 0)
        {
            totalCollectiblesInSection = 0;
            collectedInSection = 0;

            var collectibleFlags = sections[sectionIndex].collectibleTileBools;

            if (collectibleFlags.Count != allTiles.Count)
            {
                Debug.LogWarning("Collectible flags count doesn't match tile count.");
            }
            else
            {
                for (int i = 0; i < allTiles.Count; i++)
                {
                    if (collectibleFlags[i] && allTiles[i] != null)
                    {
                        Transform tile = allTiles[i].transform;
                        for (int j = 0; j < tile.childCount; j++)
                        {
                            var col = tile.GetChild(j).GetComponent<CollectableBehaviors>();
                            if (col != null)
                            {
                                col.gameObject.SetActive(false); // reset visibility
                                totalCollectiblesInSection++;
                            }
                        }
                    }
                }
            }

            UpdateCollectibleUI();
        }


        // Make sure state has matching number of bools
        if (state.tileActivationStates.Count != allTiles.Count)
        {
            Debug.LogWarning("Tile state count doesn't match tile list count.");
            return;
        }

        for (int i = 0; i < allTiles.Count; i++)
        {
            if (allTiles[i] != null)
            {
                allTiles[i].isActive = state.tileActivationStates[i];

                allTiles[i].isAboutToBeActive = false;
                allTiles[i].isAboutToBeInactive = false;
            }
        }

        int nextSectionIndex = currentSectionIndex;
        int nextStateIndex = stateIndex + 1;

        if (nextStateIndex >= sections[nextSectionIndex].states.Count)
        {
            nextStateIndex = 0;
            nextSectionIndex++;

            if (nextSectionIndex >= sections.Length)
            {
                nextSectionIndex = 0;
            }
        }

        LevelState nextState = sections[nextSectionIndex].states[nextStateIndex];

        // Compare next state with current to set about-to-be flags
        for (int i = 0; i < allTiles.Count; i++)
        {
            if (allTiles[i] == null) continue;

            bool current = state.tileActivationStates[i];
            bool next = nextState.tileActivationStates[i];

            allTiles[i].isAboutToBeActive = !current && next;
            allTiles[i].isAboutToBeInactive = current && !next;
        }

        if (collectibleFlags.Count == allTiles.Count)
        {
            for (int i = 0; i < allTiles.Count; i++)
            {
                if (collectibleFlags[i] && allTiles[i].isActive)
                {
                    Transform tile = allTiles[i].transform;
                    for (int j = 0; j < tile.childCount; j++)
                    {
                        var col = tile.GetChild(j).GetComponent<CollectibleBehavior>();
                        if (col != null)
                        {
                            col.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }

        if (state.sfxIndex >= 0 && state.sfxIndex < stateSFX.Length && stateSFX[state.sfxIndex] != null)
        {
            audioSource.PlayOneShot(stateSFX[state.sfxIndex]);
        }

        //Debug.Log($"SECTION: {sections[sectionIndex].name} | STATE: {stateIndex + 1}");
    }

    void AdvanceState()
    {
        bool isLastState = currentStateIndex == sections[currentSectionIndex].states.Count - 1;

        currentStateIndex++;

        if (currentStateIndex >= sections[currentSectionIndex].states.Count)
        {
            // Don't move to next section unless all collectibles are collected
            if (collectedInSection < totalCollectiblesInSection)
            {
                currentStateIndex = 0; // Restart section
            }
            else
            {
                currentSectionIndex++;
                currentStateIndex = 0;

                if (currentSectionIndex >= sections.Length)
                {
                    SceneManager.LoadScene(nextLevelSceneName);
                    return;
                }
            }
        }

        SetState(currentSectionIndex, currentStateIndex);
    }
}