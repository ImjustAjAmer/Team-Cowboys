using UnityEditor.Build;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    //public static TileManager Instance;

    //public bool isPlayerStanding;
    //public int col;         
    //public int row;

    public bool isLeftEdge;
    public bool isRightEdge;
    public bool isUpEdge;
    public bool isDownEdge;

    public GameObject currentPNG;
    public GameObject[] tilePNGs = new GameObject[16];

    public bool isActive;

    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        //col = Mathf.RoundToInt(transform.position.x);
        //row = Mathf.RoundToInt(transform.position.y);
    }

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
