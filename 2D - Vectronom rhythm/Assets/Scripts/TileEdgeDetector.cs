using UnityEngine;

public class TileEdgeDetector : MonoBehaviour
{
    public enum Direction { Left, Right, Up, Down }

    public Direction direction;
    public GameObject edgePNG;
    public GameObject dividerPNG;
    public float checkDistance = 1f;
    public float checkRadius = 0.1f;
    public LayerMask tileLayer;

    public void Update()
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

        //Debug.Log($"{name} ({direction}) checking at {checkPosition} — Hit: {(hit ? hit.name : "None")}");

        bool isEdge = true;

        if (hit != null)
        {
            TileManager neighbor = hit.GetComponent<TileManager>();
            if (neighbor != null)
            {
                //Debug.Log($"{name} hit tile {hit.name}, isActive: {neighbor.isActive}");

                if (neighbor.isActive)
                {
                    isEdge = false;
                }
            }
            else
            {
                Debug.Log($"{name} hit object {hit.name} but it has no TileManager");
            }
        }

        edgePNG.SetActive(isEdge);
        dividerPNG.SetActive(!isEdge);

        TileManager tile = GetComponentInParent<TileManager>();
        if (!tile.isActive)
        {
            edgePNG.SetActive(false);
            dividerPNG.SetActive(false);
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
