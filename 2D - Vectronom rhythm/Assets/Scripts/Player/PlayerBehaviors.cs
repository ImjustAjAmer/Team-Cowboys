
/*using UnityEngine;

using System.Collections;

using UnityEngine.SceneManagement;


public class PlayerBehaviors : MonoBehaviour
{

    public static PlayerBehaviors Instance;
    public float moveDistance = 1f;

    Collider2D playerCollider;
    public SpriteRenderer playerSprite;

    public string tileTag;
    [ReadOnlyAtrribute] public TileManager currentStandingTile;

    //reference to the animator
    public Animator animator;

    //isJumping
    [ReadOnlyAtrribute] public bool isJumping = false;
    public float isJumpingTime;

    // Jump buffer system
    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter = 0f;
    private string bufferedDirection = "";

    public float deathFreezeDuration = 0.5f;
    private bool isDead = false;

    private Quaternion originalRotation;

    //public AudioSource[] allAudio;
    //public AudioClip[] deathSound;

    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.W)) { bufferedDirection = "Up"; jumpBufferCounter = jumpBufferTime; }
        if (Input.GetKeyDown(KeyCode.A)) { bufferedDirection = "Left"; jumpBufferCounter = jumpBufferTime; }
        if (Input.GetKeyDown(KeyCode.S)) { bufferedDirection = "Down"; jumpBufferCounter = jumpBufferTime; }
        if (Input.GetKeyDown(KeyCode.D)) { bufferedDirection = "Right"; jumpBufferCounter = jumpBufferTime; }

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("isJumpingUp", true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("isJumpingUp", false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("isJumpingLeft", true);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool("isJumpingLeft", false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("isJumpingDown", true);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("isJumpingDown", false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("isJumpingRight", true);
        }
        else if(Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("isJumpingRight", false);
        }


        if (!isJumping && jumpBufferCounter > 0f)
        {
            StartCoroutine(JumpingState(bufferedDirection));
            jumpBufferCounter = 0f;
        }

        if (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        Color c = playerSprite.color;
        c.a = isJumping ? 0.6f : 1.0f;
        playerSprite.color = c;

        if (!isJumping && currentStandingTile != null && !currentStandingTile.isActive)
        {
            StartCoroutine(HandleGameOver());
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

        //Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        bool landedOnTile = false;

        foreach (Collider2D col in hits)
        {
            // Collectible handling
            if (col.CompareTag("Collectible"))
            {
                CollectableBehaviors collectible = col.GetComponent<CollectableBehaviors>();
                if (collectible != null)
                {
                    collectible.Collect(); // Ensure you have this method
                }
            }

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

        // Mute all other sounds

        // Freeze input
        yield return new WaitForSeconds(deathFreezeDuration);

        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}*/


using UnityEngine;

using System.Collections;

using UnityEngine.SceneManagement;



public class PlayerBehaviors : MonoBehaviour
{

    public static PlayerBehaviors Instance;
    public float moveDistance = 1f;

    Collider2D playerCollider;
    public SpriteRenderer playerSprite;

    public string tileTag;
    [ReadOnlyAtrribute] public TileManager currentStandingTile;

    //reference to the animator
    public Animator animator;

    //isJumping
    [ReadOnlyAtrribute] public bool isJumping = false;
    public float isJumpingTime;

    // Jump buffer system
    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter = 0f;
    private string bufferedDirection = "";

    public float deathFreezeDuration = 0.5f;
    private bool isDead = false;

    private Quaternion originalRotation;

    private Playercontrols playerControls;

    //public AudioSource[] allAudio;
    //public AudioClip[] deathSound;

    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
        playerControls = new Playercontrols();
        animator = GetComponentInChildren<Animator>();

        playerControls.Controls.MoveUp.performed += ctx => HandleInput("Up", true);
        playerControls.Controls.MoveUp.canceled += ctx => HandleInput("Up", false);

        playerControls.Controls.MoveLeft.performed += ctx => HandleInput("Left", true);
        playerControls.Controls.MoveLeft.canceled += ctx => HandleInput("Left", false);

        playerControls.Controls.MoveDown.performed += ctx => HandleInput("Down", true);
        playerControls.Controls.MoveDown.canceled += ctx => HandleInput("Down", false);

        playerControls.Controls.MoveRight.performed += ctx => HandleInput("Right", true);
        playerControls.Controls.MoveRight.canceled += ctx => HandleInput("Right", false);

        //set up callback for reset and quit
        playerControls.Controls.Reset.performed += ctx => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        playerControls.Controls.Quit.performed += ctx => Application.Quit();

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

        Color c = playerSprite.color;
        c.a = isJumping ? 0.6f : 1.0f;
        playerSprite.color = c;

        if (!isJumping && currentStandingTile != null && !currentStandingTile.isActive)
        {
            StartCoroutine(HandleGameOver());
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

        //Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        bool landedOnTile = false;

        foreach (Collider2D col in hits)
        {
            // Collectible handling
            if (col.CompareTag("Collectible"))
            {
                CollectableBehaviors collectible = col.GetComponent<CollectableBehaviors>();
                if (collectible != null)
                {
                    collectible.Collect(); // Ensure you have this method
                }
            }

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

        // Mute all other sounds

        // Freeze input
        yield return new WaitForSeconds(deathFreezeDuration);

        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
