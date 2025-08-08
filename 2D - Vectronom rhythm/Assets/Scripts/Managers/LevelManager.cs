using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class LevelState
{
    public float baseDuration;
    [HideInInspector] public float adjustedDuration;

    public List<bool> tileActivationStates;

    public int sfxIndex = -1;
}

[System.Serializable]
public class LevelSection
{
    public string name;
    public List<LevelState> states;

    public List<CollectableBehaviors> sectionCollectibles;

    //public List<bool> collectibleTileBools = new List<bool>();
    //public List<CollectableBehaviors> collectibles = new List<CollectableBehaviors>();
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance; //{ get; private set; }

    public List<TileManager> allTiles;

    public LevelSection[] sections;

    public float initialSpeedMultiplier = 1f;
    public float speedMultiplierAdjuster = 0.85f;

    //public float minSpeedMultiplier = 0.3f;

    private int currentSectionIndex = 0;
    private int currentStateIndex = 0;
    private float currentStateTimer;

    public string nextLevelSceneName;

    public AudioClip[] stateSFX;
    public AudioSource audioSource;

    [Range(0f, 1f)] public float globalMaxAlpha = 1f;
    [Range(0f, 1f)] public float globalMinAlpha = 0.5f;

    //[Range(0f, 1f)] public float aboutToBeActiveAlpha = 0.3f;
    //[Range(0f, 1f)] public float aboutToBeInactiveAlpha = 0.8f;
    //[Range(0f, 1f)] public float aboutToBeActiveScale = 0.75f;
    //[Range(0f, 1f)] public float aboutToBeInactiveScale = 0.5f;

    //public Vector3 aboutToBeActiveScale = Vector3.one * 0.75f;
    //public Vector3 aboutToBeInactiveScale = Vector3.one * 0.5f;

    [Header("Debug UI")]
    public TextMeshProUGUI collectibleCounterText;
    //public TextMeshProUGUI scoreMultiplierText;
    //public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI sectionStateText;
    //public TextMeshProUGUI currentStateDuration;
    //public TextMeshProUGUI remainingCollectiblesText;

    [Header("Collectible Progression")]
    [ReadOnlyAtrribute] public int totalCollectiblesInSection;
    [ReadOnlyAtrribute] public int collectedInSection;

    //[Header("Score System")]
    //public int scoreMultiplier = 1;
    //public int niceTimingMultiplierBonus = 1;
    //public float niceTimingCooldown = 0f;
    //private float niceTimingTimer = 0f;

    //private HashSet<GameObject> collectedThisSection = new HashSet<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ApplySpeedMultiplier();
        SetState(currentSectionIndex, currentStateIndex);
        UpdateUI();
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

        /*if (niceTimingCooldown > 0f)
        {
            niceTimingTimer -= Time.deltaTime;
        }*/
    }

    /*public void OnNiceTimingSuccess()
    {
        if (niceTimingCooldown > 0f && niceTimingTimer > 0f) return;

        scoreMultiplier += niceTimingMultiplierBonus;
        niceTimingTimer = niceTimingCooldown;

        UpdateUI();
    }*/

    public void OnCollectibleCollected(GameObject collectibleGO)
    {
        collectedInSection++;

        //initialSpeedMultiplier = Mathf.Max(initialSpeedMultiplier * speedMultiplierAdjuster, minSpeedMultiplier);
        //ApplySpeedMultiplier();
        UpdateUI();
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
            nextSectionIndex = (nextSectionIndex + 1) % sections.Length;
        }

        LevelState nextState = sections[nextSectionIndex].states[nextStateIndex];

        for (int i = 0; i < allTiles.Count; i++)
        {
            if (allTiles[i] == null) continue;

            bool current = state.tileActivationStates[i];
            bool next = nextState.tileActivationStates[i];

            allTiles[i].isAboutToBeActive = !current && next;
            allTiles[i].isAboutToBeInactive = current && !next;
        }

        /*if (stateIndex == 0)
        {
            ResetCollectiblesForSection();
        }*/

        //ActivateCollectiblesForState();

        if (state.sfxIndex >= 0 && state.sfxIndex < stateSFX.Length && stateSFX[state.sfxIndex] != null)
        {
            audioSource.PlayOneShot(stateSFX[state.sfxIndex]);
        }

        UpdateUI();
    }

    void AdvanceState()
    {
        currentStateIndex++;

        if (currentStateIndex >= sections[currentSectionIndex].states.Count)
        {
            currentStateIndex = 0;
            currentSectionIndex++;

            if (currentSectionIndex >= sections.Length)
            {
                currentSectionIndex = 0;

                initialSpeedMultiplier *= speedMultiplierAdjuster;
                ApplySpeedMultiplier();

                return;
            }

            /*if (true)//if (collectedInSection < totalCollectiblesInSection)
            {
                // Loop section
                currentStateIndex = 0;
                SetState(currentSectionIndex, currentStateIndex);
                return;
            }
            else
            {
                // Advance section
                currentSectionIndex++;
                currentStateIndex = 0;

                if (currentSectionIndex >= sections.Length)
                {
                    SceneManager.LoadScene(nextLevelSceneName);
                    return;
                }
            }*/
        }

        SetState(currentSectionIndex, currentStateIndex);
    }

    /*void ResetCollectiblesForSection()
    {
        collectedThisSection.Clear();
        collectedInSection = 0;
        totalCollectiblesInSection = 0;

        for (int i = 0; i < allTiles.Count; i++)
        {
            TileManager tile = allTiles[i];
            if (tile == null) continue;

            Transform tileTf = tile.transform;

            //no more child collectibles
            for (int j = 0; j < tileTf.childCount; j++)
            {
                var col = tileTf.GetChild(j).GetComponent<CollectableBehaviors>();
                if (col != null)
                {
                    col.ResetCollectible();

                    if (sections[currentSectionIndex].collectibleTileBools.Count > i &&
                        sections[currentSectionIndex].collectibleTileBools[i])
                    {
                        totalCollectiblesInSection++;
                    }
                }
            }
        }
    }*/

    /*void ActivateCollectiblesForState()
    {
        var flags = sections[currentSectionIndex].collectibleTileBools;

        if (flags.Count != allTiles.Count) return;

        for (int i = 0; i < allTiles.Count; i++)
        {
            TileManager tile = allTiles[i];
            if (tile == null) continue;

            bool shouldHaveCollectible = flags[i];
            bool tileIsActive = tile.isActive;

            Transform tileTf = tile.transform;

            //for (int j = 0; j < tileTf.childCount; j++)
            {
                //var col = tileTf.GetChild(j).GetComponent<CollectableBehaviors>();
                var col = tileTf.GetComponentInChildren<CollectableBehaviors>();
                if (col != null && !col.hasBeenCollected)
                {
                    //col.TryActivate(true); 
                    bool shouldShow = shouldHaveCollectible && tileIsActive && !col.hasBeenCollected;
                    col.TryActivate(shouldShow);
                }
            }
        }
    }*/

    void UpdateUI()
    {
        if (collectibleCounterText != null)
        {
            collectibleCounterText.text = $"{collectedInSection}/{totalCollectiblesInSection}";
        }

        /*if (multiplierText != null)
        {
            multiplierText.text = $"x{scoreMultiplier}";
        }*/

        if (sectionStateText != null)
        {
            sectionStateText.text = $"Section: {sections[currentSectionIndex].name} | State: {currentStateIndex + 1}/{sections[currentSectionIndex].states.Count}";
        }
    }
}
