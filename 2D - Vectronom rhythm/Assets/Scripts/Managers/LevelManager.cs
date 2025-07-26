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
}

public class LevelManager : MonoBehaviour
{
    [Header("Master Tile List (Manually Assign Once)")]
    public List<TileManager> allTiles;

    public LevelSection[] sections;
    public float speedMultiplier = 1f; // Initial tempo multiplier
    public float speedMultiplierAdjuster = 0.85f;

    private int currentSectionIndex = 0;
    private int currentStateIndex = 0;
    private float timer;

    [Header("Audio")]
    public AudioClip[] stateSFX; 
    public AudioSource audioSource;

    [Header("Background")]
    public SpriteRenderer background;
    public Color[] backgroundColors; // Match number of states OR sections

    public bool loopLastStateOnly = false;

    [Range(0f, 1f)] public float globalMaxAlpha = 1f;
    [Range(0f, 1f)] public float globalMinAlpha = 0.5f;

    void Start()
    {
        ApplySpeedMultiplier();
        SetState(currentSectionIndex, currentStateIndex);
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

        if (background != null && backgroundColors.Length > stateIndex)
        {
            background.color = backgroundColors[stateIndex];
        }
    }

    void AdvanceState()
    {
        if (loopLastStateOnly &&
            currentSectionIndex == sections.Length - 1 &&
            currentStateIndex == sections[currentSectionIndex].states.Count - 1)
        {
            SetState(currentSectionIndex, currentStateIndex); // Just stay on this one
            return;
        }

        currentStateIndex++;

        if (currentStateIndex >= sections[currentSectionIndex].states.Count)
        {
            currentStateIndex = 0;
            currentSectionIndex++;

            if (currentSectionIndex >= sections.Length)
            {
                currentSectionIndex = 0;
                speedMultiplier *= speedMultiplierAdjuster;
                ApplySpeedMultiplier();
                Debug.Log("Looped all sections — tempo increased.");
            }
        }

        SetState(currentSectionIndex, currentStateIndex);
    }
}