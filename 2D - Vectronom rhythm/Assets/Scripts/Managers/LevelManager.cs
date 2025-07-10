using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;

[System.Serializable]
public class LevelState
{
    public float baseDuration; // Initial duration of this state before speed multiplier
    [ReadOnlyAtrribute] public float adjustedDuration; // Final duration after speed progression is applied

    //public List<TileManager> activeTiles; // Tiles that should be ON
    //public List<TileManager> inactiveTiles;  // Tiles that should be OFF
    public List<bool> tileActivationStates;
}

[System.Serializable]
public class LevelSection
{
    public string name; 
    public List<LevelState> states; // Sequence of tile states in this section
}

public class LevelManager : MonoBehaviour
{
    [Header("Master Tile List (Manually Assign Once)")]
    public List<TileManager> allTiles; // You assign this in Inspector, order matters

    //public List<LevelSection> sections;
    public LevelSection[] sections; // All sections in the level loop
    public float speedMultiplier = 1f; // Initial tempo multiplier
    public float speedMultiplierAdjuster = 0.85f;

    private int currentSectionIndex = 0; 
    private int currentStateIndex = 0; 
    private float timer;

    //private List<TileManager> allTiles = new List<TileManager>();

    /*[Header("Manual Tile Grid")]
    public int gridWidth = 3;
    public int gridHeight = 3;
    public TileManager[,] tileGrid; // 2D grid of tiles

    [Header("Assign Tiles Manually (Left to Right, Bottom to Top)")]
    public TileManager[] tileRefs; // Assigned in Inspector manually*/

    void Start()
    {
        //TileManager[] foundTiles = GameObject.FindObjectsOfType<TileManager>();
        //allTiles.AddRange(foundTiles);

        //BuildTileGrid();

        ApplySpeedMultiplier(); // Adjust all durations based on starting speed
        SetState(currentSectionIndex, currentStateIndex); // Start the first state
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            AdvanceState(); // Move to the next state
        }
    }

    /*void BuildTileGrid()
    {
        tileGrid = new TileManager[gridWidth, gridHeight];

        if (tileRefs.Length != gridWidth * gridHeight)
        {
            Debug.LogError("TileRefs array size does not match grid size.");
            return;
        }

        int index = 0;
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                TileManager tile = tileRefs[index];
                tile.col = x;
                tile.row = y;
                tileGrid[x, y] = tile;
                index++;
            }
        }
    }*/

    // This method applies the current speed multiplier to every state's duration
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

    // This sets the active tiles according to the current section/state
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

        /* Set tile states manually by (col, row)
        foreach (TileManager tile in state.activeTiles)
        {
            if (tile != null)
                tile.isActive = true;
        }

        foreach (TileManager tile in state.inactiveTiles)
        {
            if (tile != null)
                tile.isActive = false;
        }*/

        // Apply activation bools to real tiles
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
                ApplySpeedMultiplier(); // Re-apply updated speed to all states
                Debug.Log("Looped all sections — tempo increased.");
            }
        }

        SetState(currentSectionIndex, currentStateIndex);
    }

    /*bool IsValidCoord(Vector2Int coord)
    {
        return coord.x >= 0 && coord.x < gridWidth && coord.y >= 0 && coord.y < gridHeight;
    }*/
}
