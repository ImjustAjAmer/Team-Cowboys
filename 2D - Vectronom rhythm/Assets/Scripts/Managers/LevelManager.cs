using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;

[System.Serializable]
public class LevelState
{
    public float baseDuration; 
    [ReadOnlyAtrribute] public float adjustedDuration;

    public List<bool> tileActivationStates;
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
            Debug.LogWarning("Tile state count doesn't match tile list count! Make sure each state has one checkbox per tile.");
            return;
        }

        for (int i = 0; i < allTiles.Count; i++)
        {
            if (allTiles[i] != null)
            {
                allTiles[i].isActive = state.tileActivationStates[i];
            }
        }

        Debug.Log($"SECTION: {sections[sectionIndex].name} | STATE: {stateIndex + 1}");
    }

    void AdvanceState()
    {
        currentStateIndex++;

        // If we’ve finished all states in the current section...
        if (currentStateIndex >= sections[currentSectionIndex].states.Count)
        {
            currentStateIndex = 0;
            currentSectionIndex++;

            // If we’ve finished all sections, loop back and increase tempo
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
