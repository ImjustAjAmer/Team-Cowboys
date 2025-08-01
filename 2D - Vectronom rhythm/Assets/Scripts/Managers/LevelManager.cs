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

    public List<Color> backgroundColorCycle;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Tiles")]
    public List<TileManager> allTiles;

    [Header("Sections")]
    public LevelSection[] sections;

    [Header("Tempo")]
    public float speedMultiplier = 1f; // Initial tempo multiplier
    public float speedMultiplierAdjuster = 0.85f;

    private int currentSectionIndex = 0;
    private int currentStateIndex = 0;
    private float timer;

    [Header("Boss Settings")]
    public int bossMaxHealth = 1000;
    [ReadOnlyAtrribute] public int currentBossHealth;
    [Tooltip("Speed increases every X% health lost")]
    [Range(0f, 1f)] public float speedStepPercent = 0.1f;
    public float speedIncreaseAmount = 0.1f;
    private int lastSpeedStep = 0;
    public float bossDeathAnimationTime = 2.0f;

    [Header("Scene Progression")]
    public string nextLevelSceneName;

    [Header("Audio")]
    public AudioClip[] stateSFX; 
    public AudioSource audioSource;

    [Header("Background")]
    public SpriteRenderer background;
    public float bgLerpDuration = 0.5f;
    private Color currentBGColor;

    //public bool loopLastStateOnly = false;

    [Range(0f, 1f)] public float globalMaxAlpha = 1f;
    [Range(0f, 1f)] public float globalMinAlpha = 0.5f;

    void Start()
    {
        //Instance = this;
        currentBossHealth = bossMaxHealth;
        ApplySpeedMultiplier();
        SetState(currentSectionIndex, currentStateIndex);
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            AdvanceState();
        }

        float currentDuration = sections[currentSectionIndex].states[currentStateIndex].adjustedDuration;

        foreach (TileManager tile in allTiles)
        {
            if (tile != null)
                tile.SetFadeInfo(timer, currentDuration);
        }
    }

    void ApplySpeedMultiplier()
    {
        foreach (var section in sections)
        {
            foreach (var state in section.states)
            {
                state.adjustedDuration = state.baseDuration * speedMultiplier;
            }
        }
    }

    void SetState(int sectionIndex, int stateIndex)
    {
        LevelState state = sections[sectionIndex].states[stateIndex];
        timer = state.adjustedDuration;


        // Sanity check — make sure state has matching number of bools
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

        foreach (var tile in allTiles)
        {
            if (tile != null)
                tile.RefreshVisual();
        }

        if (state.sfxIndex >= 0 && state.sfxIndex < stateSFX.Length && stateSFX[state.sfxIndex] != null)
        {
            audioSource.PlayOneShot(stateSFX[state.sfxIndex]);
        }

        Debug.Log($"SECTION: {sections[sectionIndex].name} | STATE: {stateIndex + 1}");

        PickAndLerpNewBGColor();
    }


    void AdvanceState()
    {
        /*if (loopLastStateOnly &&
            currentSectionIndex == sections.Length - 1 &&
            currentStateIndex == sections[currentSectionIndex].states.Count - 1)
        {
            SetState(currentSectionIndex, currentStateIndex); // Just stay on this one
            return;
        }*/

        currentStateIndex++;

        if (currentStateIndex >= sections[currentSectionIndex].states.Count)
        {
            currentStateIndex = 0;
            currentSectionIndex++;

            if (currentSectionIndex >= sections.Length)
            {
                currentSectionIndex = 0;
                //speedMultiplier *= speedMultiplierAdjuster;
                //speedMultiplier += speedIncreaseAmount;
                speedMultiplier *= speedIncreaseAmount;
                ApplySpeedMultiplier();
                Debug.Log("Looped all sections — tempo increased.");
            }
        }

        SetState(currentSectionIndex, currentStateIndex);
    }

    public void DealBossDamage(int amount)
    {
        if (currentBossHealth <= 0) return;

        currentBossHealth -= amount;
        currentBossHealth = Mathf.Max(currentBossHealth, 0);

        float percentLost = 1f - ((float)currentBossHealth / bossMaxHealth);
        int step = Mathf.FloorToInt(percentLost / speedStepPercent);

        if (step > lastSpeedStep)
        {
            lastSpeedStep = step;
            speedMultiplier += speedIncreaseAmount;
            ApplySpeedMultiplier();
            Debug.Log("Boss HP dropped — increased speed.");
        }

        if (currentBossHealth <= 0)
        {
            StartCoroutine(BossDeathSequence());
        }
    }

    IEnumerator BossDeathSequence()
    {
        Debug.Log("Boss defeated!");
        yield return new WaitForSeconds(bossDeathAnimationTime);
        SceneManager.LoadScene(nextLevelSceneName);
    }

    void PickAndLerpNewBGColor()
    {
        List<Color> colors = sections[currentSectionIndex].backgroundColorCycle;

        if (colors == null || colors.Count == 0 || background == null) return;

        Color newColor;
        do
        {
            newColor = colors[Random.Range(0, colors.Count)];
        } while (newColor == currentBGColor);

        StartCoroutine(LerpBackgroundColor(newColor));
        currentBGColor = newColor;
    }

    IEnumerator LerpBackgroundColor(Color targetColor)
    {
        Color startColor = background.color;
        float t = 0f;

        while (t < bgLerpDuration)
        {
            t += Time.deltaTime;
            background.color = Color.Lerp(startColor, targetColor, t / bgLerpDuration);
            yield return null;
        }

        background.color = targetColor;
    }
}