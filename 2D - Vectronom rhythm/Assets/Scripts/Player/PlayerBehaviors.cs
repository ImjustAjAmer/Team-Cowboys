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

    public string tileTag;

    [ReadOnlyAtrribute] public TileManager currentStandingTile;

    //isJumping
    public bool isJumping;
    public float isJumpingTime;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == tileTag)
        {
            currentStandingTile = collision.GetComponent<TileManager>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isJumping)
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
        }

        if (currentStandingTile.isActive == false)
        {
            Debug.Log("Game Over!");
        }
    }

    IEnumerator JumpingState(string direction)
    {
        isJumping = true;

        playerCollider.enabled = false;

        yield return new WaitForSeconds(isJumpingTime);

        if (direction == "Up")
        {
            rb.MovePosition(rb.position + Vector2.up * moveDistance);
        }

        if(direction == "Left")
        {
            rb.MovePosition(rb.position + Vector2.left * moveDistance);
        }

        if(direction == "Down")
        {
            rb.MovePosition(rb.position + Vector2.down * moveDistance);
        }

        if(direction == "Right")
        {
            rb.MovePosition(rb.position + Vector2.right * moveDistance);
        }

        isJumping = false;

        playerCollider.enabled = true;
    }
}
