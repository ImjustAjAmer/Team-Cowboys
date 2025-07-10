using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;

[System.Serializable]
public class LevelState
{
    public float baseDuration; // Initial duration of this state before speed multiplier
    [ReadOnlyAtrribute] public float adjustedDuration; // Final duration after speed progression is applied

    public List<Vector2Int> activeTileCoords; // Tiles that should be ON
    public List<Vector2Int> inactiveTileCoords;  // Tiles that should be OFF
}

[System.Serializable]
public class LevelSection
{
    public string name; 
    public List<LevelState> states; // Sequence of tile states in this section
}

public class LevelManager : MonoBehaviour
{
    public LevelSection[] sections; // All sections in the level loop
    public float speedMultiplier = 1f; // Initial tempo multiplier

    private int currentSectionIndex = 0; 
    private int currentStateIndex = 0; 
    private float timer;

    //private List<TileManager> allTiles = new List<TileManager>();

    [Header("Manual Tile Grid")]
    public int gridWidth = 3;
    public int gridHeight = 3;
    public TileManager[,] tileGrid; // 2D grid of tiles

    [Header("Assign Tiles Manually (Left to Right, Top to Bottom)")]
    public TileManager[] tileRefs; // Assigned in Inspector manually

    void Start()
    {
        //TileManager[] foundTiles = GameObject.FindObjectsOfType<TileManager>();
        //allTiles.AddRange(foundTiles);

        BuildTileGrid();

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

    void BuildTileGrid()
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
    }

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

        // Set tile states manually by (col, row)
        foreach (Vector2Int coord in state.activeTileCoords)
        {
            if (IsValidCoord(coord))
                tileGrid[coord.x, coord.y].isActive = true;
        }

        foreach (Vector2Int coord in state.inactiveTileCoords)
        {
            if (IsValidCoord(coord))
                tileGrid[coord.x, coord.y].isActive = false;
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
                speedMultiplier *= 0.85f; // Example tempo ramp: 15% faster each full loop
                ApplySpeedMultiplier(); // Re-apply updated speed to all states
                Debug.Log("Looped all sections — tempo increased.");
            }
        }

        SetState(currentSectionIndex, currentStateIndex);
    }

    bool IsValidCoord(Vector2Int coord)
    {
        return coord.x >= 0 && coord.x < gridWidth && coord.y >= 0 && coord.y < gridHeight;
    }
}
