using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Diagnostics.CodeAnalysis;

public class PlayerBehaviors1 : MonoBehaviour
{
    public static PlayerBehaviors Instance;
    public float moveDistance = 1f;

    Collider2D playerCollider;
    public SpriteRenderer playerSprite;

    public string tileTag = "TileTag";
    [ReadOnlyAtrribute] public TileManager currentStandingTile;

    //reference to the animator
    public Animator animator;
    public AudioManager audioManager;

    //isJumping
    [ReadOnlyAtrribute] public bool isJumping = false;
    public float isJumpingTime;

    // Jump buffer system
    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter = 0f;
    private string bufferedDirection = "";

    public float deathFreezeDuration = 0.5f;
    private bool isDead = false;

    private AudioSource playerDeathSound;
    private Quaternion originalRotation;

    private Playercontrols playerControls;

    private void Awake()
    {
        //Read performed and canceled input from Input Map

        playerCollider = GetComponent<Collider2D>();
        playerControls = new Playercontrols();
        animator = GetComponentInChildren<Animator>();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        playerControls.Controls.MoveUp.performed += ctx => HandleInput("Up", true);
        playerControls.Controls.MoveUp.canceled += ctx => HandleInput("Up", false);

        playerControls.Controls.MoveLeft.performed += ctx => HandleInput("Left", true);
        playerControls.Controls.MoveLeft.canceled += ctx => HandleInput("Left", false);

        playerControls.Controls.MoveDown.performed += ctx => HandleInput("Down", true);
        playerControls.Controls.MoveDown.canceled += ctx => HandleInput("Down", false);

        playerControls.Controls.MoveRight.performed += ctx => HandleInput("Right", true);
        playerControls.Controls.MoveRight.canceled += ctx => HandleInput("Right", false);



    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
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
                StartCoroutine(HandleGameOver());
            }
        }
        else
        {
            StartCoroutine(HandleGameOver());
        }
    }

    void Update()
    {

        if (isDead) return;

        if (!isJumping && jumpBufferCounter > 0f)
        {
            StartCoroutine(JumpingState(bufferedDirection));
            jumpBufferCounter = 0f;
        }

        if (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        /*Color c = playerSprite.color;
        c.a = isJumping ? 0.6f : 1.0f;
        playerSprite.color = c;*/

        if (!isJumping && currentStandingTile != null && !currentStandingTile.isActive)
        {
            StartCoroutine(HandleGameOver());
            
        }

        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Collectible"))
            {
                CollectableBehaviors1 collectible1 = col.GetComponent<CollectableBehaviors1>();
                if (collectible1 != null)
                {
                    collectible1.LoadThisScene();
                }
            }
        }

    }

    private void HandleInput(string direction, bool IsPressed)
    {
        if (isDead) return;

        string animatorParam = "";
        switch (direction)
        {
            case "Up":
                animatorParam = "isJumpingUp";
                break;

            case "Left":
                animatorParam = "isJumpingLeft";
                break;

            case "Down":
                animatorParam = "isJumpingDown";
                break;

            case "Right":
                animatorParam = "isJumpingRight";
                break;
        }

        animator.SetBool(animatorParam, IsPressed);

        if (IsPressed)
        {
            bufferedDirection = direction;
            jumpBufferCounter = jumpBufferTime;
        }

    }

    IEnumerator JumpingState(string direction)
    {
        isJumping = true;

        Vector2 startPos = transform.position;
        Vector2 targetPos = startPos;

        if (direction == "Up") targetPos += Vector2.up * moveDistance;
        if (direction == "Left") targetPos += Vector2.left * moveDistance;
        if (direction == "Down") targetPos += Vector2.down * moveDistance;
        if (direction == "Right") targetPos += Vector2.right * moveDistance;

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

        currentStandingTile = null;

        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        bool landedOnTile = false;

        foreach (Collider2D col in hits)
        {
            /*if (col.CompareTag("Collectible"))
            {
                CollectableBehaviors collectible = col.GetComponent<CollectableBehaviors>();
                if (collectible != null)
                {
                    collectible.Collect();
                }
            }*/

            if (col.CompareTag(tileTag))
            {
                currentStandingTile = col.GetComponent<TileManager>();
                currentStandingTile.isPlayerStanding = true;

                if (!currentStandingTile.isActive)
                {
                    isJumping = false;
                    StartCoroutine(HandleGameOver());
                    yield break;
                }

                landedOnTile = true;
                break;
            }
        }

        isJumping = false;

        if (!landedOnTile)
        {
            StartCoroutine(HandleGameOver());
        }
    }

    public IEnumerator HandleGameOver()
    {
        if (isDead) yield break;

        isDead = true;
        originalRotation = playerSprite.transform.rotation;

        // Flip the sprite upside down
        playerSprite.transform.rotation = Quaternion.Euler(0, 0, 180);

        audioManager.PlaySFX(audioManager.Death);
        // Mute all other sounds

        // Freeze input
        yield return new WaitForSeconds(deathFreezeDuration);

        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Visualize OverlapPointAll(transform.position) with a small sphere
        Gizmos.DrawWireSphere(transform.position, 0.05f);

        // Optional: visualize the player's actual collider bounds
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Bounds bounds = col.bounds;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
