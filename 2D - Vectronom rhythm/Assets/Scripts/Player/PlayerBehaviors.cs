using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerBehaviors : MonoBehaviour
{
    public float moveDistance = 1f;

    Collider2D playerCollider;
    public SpriteRenderer playerSprite;

    public string tileTag;

    [ReadOnlyAtrribute] public TileManager currentStandingTile;

    //isJumping
    [ReadOnlyAtrribute] public bool isJumping = false;
    public float isJumpingTime;

    // Jump buffer system
    //public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter = 0f;
    private string bufferedDirection = "";

    public float deathFreezeDuration = 0.5f;
    private bool isDead = false;
    private Quaternion originalRotation;

    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position);
        if (hit != null && hit.CompareTag(tileTag))
        {
            currentStandingTile = hit.GetComponent<TileManager>();
            currentStandingTile.isPlayerStanding = true;

            if (!currentStandingTile.isActive)
            {
                Debug.Log("Game Over");
                StartCoroutine(HandleGameOver());
                //playerSprite.color = new Color(0f, 0f, 0f, 1f);
            }
        }
    }

    /*
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == tileTag && !isJumping)
        {
            currentStandingTile = collision.GetComponent<TileManager>();
            currentStandingTile.isPlayerStanding = true;
        } 
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == tileTag && !isJumping)
        {
            currentStandingTile = collision.GetComponent<TileManager>();
            currentStandingTile.isPlayerStanding = false;
        }
    }
    */

    private void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.W)) { bufferedDirection = "Up"; jumpBufferCounter = isJumpingTime; }
        if (Input.GetKeyDown(KeyCode.A)) { bufferedDirection = "Left"; jumpBufferCounter = isJumpingTime; }
        if (Input.GetKeyDown(KeyCode.S)) { bufferedDirection = "Down"; jumpBufferCounter = isJumpingTime; }
        if (Input.GetKeyDown(KeyCode.D)) { bufferedDirection = "Right"; jumpBufferCounter = isJumpingTime; }
        

        if (!isJumping && jumpBufferCounter > 0f)
        {
            StartCoroutine(JumpingState(bufferedDirection));
            jumpBufferCounter = 0f;
        }

        if (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (isJumping)
        {
            Color c = playerSprite.color;
            c.a = 0.4f;
            playerSprite.color = c;
        }
        else
        {
            Color c = playerSprite.color;
            c.a = 1.0f;
            playerSprite.color = c;
        }

        if (!isJumping && currentStandingTile != null)
        {
            if (!currentStandingTile.isActive)
            {
                Debug.Log("Game Over");
                StartCoroutine(HandleGameOver());

                /*if (playerSprite != null)
                {
                    Color c = playerSprite.color;
                    c.r = 0f;
                    c.g = 0f;
                    c.b = 0f;
                    c.a = 1f;
                    playerSprite.color = c;
                }*/
            }
        }
    }

    IEnumerator JumpingState(string direction)
    {
        isJumping = true;

        //playerCollider.enabled = false;

        Vector2 startPos = transform.position;
        Vector2 targetPos = startPos;

        if (direction == "Up")
        {
            targetPos += Vector2.up * moveDistance;
        }

        if (direction == "Left")
        {
            targetPos += Vector2.left * moveDistance;
        }

        if (direction == "Down")
        {
            targetPos += Vector2.down * moveDistance;
        }

        if (direction == "Right")
        {
            targetPos += Vector2.right * moveDistance;
        }

        float elapsed = 0f;

        while (elapsed < isJumpingTime)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveDistance / isJumpingTime * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        if (currentStandingTile != null)
        {
            currentStandingTile.isPlayerStanding = false;
        }

        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        foreach (Collider2D col in hits)
        {
            if (col.CompareTag(tileTag))
            {
                currentStandingTile = col.GetComponent<TileManager>();
                currentStandingTile.isPlayerStanding = true;

                if (!currentStandingTile.isActive)
                {
                    Debug.Log("Game Over");
                    StartCoroutine(HandleGameOver());

                    /*if (playerSprite != null)
                    {
                        Color c = playerSprite.color;
                        c.r = 0f;
                        c.g = 0f;
                        c.b = 0f;
                        c.a = 1f;
                        playerSprite.color = c;
                    }*/
                }

                break;
            }
        }

        isJumping = false;

        //playerCollider.enabled = true;
    }

    IEnumerator HandleGameOver()
    {
        isDead = true;
        originalRotation = playerSprite.transform.rotation;

        // Flip the sprite upside down
        playerSprite.transform.rotation = Quaternion.Euler(0, 0, 180);

        // Darken the sprite
        //Color c = playerSprite.color;
        //c = Color.black;
        //playerSprite.color = c;

        // Freeze input
        yield return new WaitForSeconds(deathFreezeDuration);

        // Reset rotation (optional if you're reloading scene anyway)
        //playerSprite.transform.rotation = originalRotation;

        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
