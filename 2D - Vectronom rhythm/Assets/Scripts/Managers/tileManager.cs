using UnityEditor.Build;
using UnityEngine;

public class tileManager : MonoBehaviour
{
    public static tileManager Instance;

    public bool isPlayerStanding;
    public bool isActive;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( isActive )
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else //( isActive)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        Debug.Log(this.gameObject.name + ": " + isActive);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isPlayerStanding = true;
        }
    }
}
