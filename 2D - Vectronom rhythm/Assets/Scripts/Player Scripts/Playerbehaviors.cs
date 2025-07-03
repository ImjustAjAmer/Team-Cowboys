using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.IO;

public class Playerbehaviors : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public float moveDistance = 1f;

    public GameObject playerCollider;
    public tileManager currentStandingTile;
    public bool isJumping;
    public float isJumpingTime;

    private Rigidbody2D rb;
    private Vector2 Vec2;
    private PlayerInput playerInput;
    private Playercontrols playerControls;


    /*private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }*/

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // = new Playercontrols();
        //playerInput = GetComponent<PlayerInput>();
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

        if (isJumping)
        {
            playerCollider.SetActive(false);
        }

        if(currentStandingTile.isActive == false)
        {
            Debug.Log("player is dead");
        }

        Debug.Log("Player is standing on: " + currentStandingTile);
    }
    IEnumerator JumpingState(string direction)
    {
        //Print the time of when the function is first called.
        isJumping = true; 

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(isJumpingTime);

        //After we have waited 5 seconds print the time again.
        if(direction == "Up")
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
        playerCollider.SetActive(true);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "tile")
        {
            currentStandingTile = collision.GetComponent<tileManager>();
        }
    }

    
}
