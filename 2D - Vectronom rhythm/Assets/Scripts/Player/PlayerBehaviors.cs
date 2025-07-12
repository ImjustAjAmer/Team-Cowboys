using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
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

    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == tileTag && !isJumping)
        {
            currentStandingTile = collision.GetComponent<TileManager>();
            currentStandingTile.isPlayerStanding = true;

            if (currentStandingTile.isActive == false)
            {
                Debug.Log("Game Over");
            }
        } 
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.W) && !isJumping)
        {
            StartCoroutine(JumpingState("Up"));
        }

        if (Input.GetKeyDown(KeyCode.A) && !isJumping)
        {
            StartCoroutine(JumpingState("Left"));
        }

        if (Input.GetKeyDown(KeyCode.S) && !isJumping)
        {
            StartCoroutine(JumpingState("Down"));
        }

        if (Input.GetKeyDown(KeyCode.D) && !isJumping)
        {
            StartCoroutine(JumpingState("Right"));
        }*/

        /*if (!isJumping)
        {
            if (Input.GetKeyDown(KeyCode.W)) { bufferedDirection = "Up"; jumpBufferCounter = isJumpingTime; }
            if (Input.GetKeyDown(KeyCode.A)) { bufferedDirection = "Left"; jumpBufferCounter = isJumpingTime; }
            if (Input.GetKeyDown(KeyCode.S)) { bufferedDirection = "Down"; jumpBufferCounter = isJumpingTime; }
            if (Input.GetKeyDown(KeyCode.D)) { bufferedDirection = "Right"; jumpBufferCounter = isJumpingTime; }
        }*/

        
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

        isJumping = false;

        //playerCollider.enabled = true;
    }
}
