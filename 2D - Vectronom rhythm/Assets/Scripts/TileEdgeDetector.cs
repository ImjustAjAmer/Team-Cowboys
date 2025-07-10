using UnityEngine;

public class TileEdgeDetector : MonoBehaviour
{
    public enum Direction { Left, Right, Up, Down }

    public Direction direction;
    public GameObject edgePNG;
    public float checkDistance = 1f;
    public float checkRadius = 0.1f;
    public LayerMask tileLayer;

    public void Update() //CheckEdge()
    {
        Vector2 offset = Vector2.zero;

        switch (direction)
        {
            case Direction.Left: offset = Vector2.left; break;
            case Direction.Right: offset = Vector2.right; break;
            case Direction.Up: offset = Vector2.up; break;
            case Direction.Down: offset = Vector2.down; break;
        }

        Vector2 rotatedOffset = (Vector2)(transform.rotation * offset * checkDistance);
        Vector2 checkPosition = (Vector2)transform.position + rotatedOffset;

        Collider2D hit = Physics2D.OverlapCircle(checkPosition, checkRadius, tileLayer);

        Debug.Log($"{name} ({direction}) checking at {checkPosition} — Hit: {(hit ? hit.name : "None")}");

        bool isEdge = true;

        if (hit != null)
        {
            TileManager neighbor = hit.GetComponent<TileManager>();
            if (neighbor != null)
            {
                Debug.Log($"{name} hit tile {hit.name}, isActive: {neighbor.isActive}");
                if (neighbor.isActive)
                    isEdge = false;
            }
            else
            {
                Debug.Log($"{name} hit object {hit.name} but it has no TileManager");
            }
        }

        if (edgePNG != null)
        {
            edgePNG.SetActive(isEdge);
            Debug.Log($"{name} ({direction}) PNG set to {(isEdge ? "ON" : "OFF")}");
        }

        TileManager tile = GetComponentInParent<TileManager>();
        if (tile != null && !tile.isActive)
        {
            if (edgePNG != null) edgePNG.SetActive(false);
            return;
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector2 offset = Vector2.zero;

        switch (direction)
        {
            case Direction.Left: offset = Vector2.left; break;
            case Direction.Right: offset = Vector2.right; break;
            case Direction.Up: offset = Vector2.up; break;
            case Direction.Down: offset = Vector2.down; break;
        }

        Vector2 rotatedOffset = (Vector2)(transform.rotation * offset * checkDistance);
        Vector2 checkPosition = (Vector2)transform.position + rotatedOffset;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(checkPosition, checkRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, checkPosition);
    }
}
