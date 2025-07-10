using UnityEditor.Build;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    //public static TileManager Instance;

    //public bool isPlayerStanding;
    //public int col;         
    //public int row;

    //public bool isLeftEdge;
    //public bool isRightEdge;
    //public bool isUpEdge;
    //public bool isDownEdge;

    /*[Header("Directional Edge PNGs")]
    public GameObject leftEdgePNG;
    public GameObject rightEdgePNG;
    public GameObject upEdgePNG;
    public GameObject downEdgePNG;*/

    //public GameObject currentPNG;
    //public GameObject[] tilePNGs = new GameObject[16];

    public bool isActive;

    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        //UpdateDirectionalPNGs();

        //col = Mathf.RoundToInt(transform.position.x);
        //row = Mathf.RoundToInt(transform.position.y);
    }

    /*public void SetEdgeState(TileEdgeDetector.EdgeDirection direction, bool exposed)
    {
        switch (direction)
        {
            case TileEdgeDetector.EdgeDirection.Left:
                isLeftEdge = exposed;
                break;
            case TileEdgeDetector.EdgeDirection.Right:
                isRightEdge = exposed;
                break;
            case TileEdgeDetector.EdgeDirection.Up:
                isUpEdge = exposed;
                break;
            case TileEdgeDetector.EdgeDirection.Down:
                isDownEdge = exposed;
                break;
        }

        UpdateDirectionalPNGs();
    }*/

    /*void UpdateDirectionalPNGs()
    {
        if (leftEdgePNG != null)
            leftEdgePNG.SetActive(isLeftEdge);
        if (rightEdgePNG != null)
            rightEdgePNG.SetActive(isRightEdge);
        if (upEdgePNG != null)
            upEdgePNG.SetActive(isUpEdge);
        if (downEdgePNG != null)
            downEdgePNG.SetActive(isDownEdge);
    }*/

    // Update is called once per frame
    void Update()
    {
        sr.enabled = isActive; // Show/hide tile based on active state

        /*if (isActive)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else //( isActive)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }*/

        //Debug.Log(this.gameObject.name + ": " + isActive);
    }

    /*public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isPlayerStanding = true;
        }
    }*/
}
