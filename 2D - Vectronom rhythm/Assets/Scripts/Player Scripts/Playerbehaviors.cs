using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Playerbehaviors : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public float moveDistance = 1f;

    public tileManager currentStandingTile;
    public bool isJumping;

    private Rigidbody2D rb;
    private Vector2 Vec2;
    private PlayerInput playerInput;
    private Playercontrols playerControls;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new Playercontrols();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            isJumping = true;
            rb.MovePosition(rb.position + Vector2.up * moveDistance);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            isJumping = true;
            rb.MovePosition(rb.position + Vector2.left * moveDistance);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            isJumping = true;
            rb.MovePosition(rb.position + Vector2.down * moveDistance);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            isJumping = true;
            rb.MovePosition(rb.position + Vector2.right * moveDistance);
        }


        if(currentStandingTile.isActive == false)
        {
            Debug.Log("player is dead");
        }

        Debug.Log("Player is standing on: " + currentStandingTile);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "tile")
        {
            currentStandingTile = collision.GetComponent<tileManager>();
        }
    }

    
}
