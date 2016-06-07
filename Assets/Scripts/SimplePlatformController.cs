using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class PlayerSprites
{
    public Sprite normalSprite, jumpSprite, fallSprite, boosterSprite;
}

[System.Serializable]
public class FiringShot
{
    public GameObject shot;
    public Transform shotSpawn;

    public float fireRate;
    public float nextFire;
}

public class SimplePlatformController : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;

    public PlayerSprites playerSprites;
    public FiringShot firingShot;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;

    private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    private float[] lastPositionY = { 0f, 0f, 0f };
    private bool firstRun = false;

    public int pointsPerCoin = 10;
    public int pointsPerBoost = 20;
    private int playerScore;
    public GUIText scoreText;

    private bool boosted = false;

    public float savedPositionY;
    SpriteRenderer spriteRenderer;

    private Quaternion calibrationQuaternion;

    private bool jumpState = false;

    public bool getBoosted()
    {
        return boosted;
    }

    public void setBoosted(bool value)
    {
        boosted = value;
    }

    void Start()
    {
        playerScore = 0;
        scoreText.text = "Score: " + playerScore;
        savedPositionY = transform.position.y;
        Transform child = transform.FindChild("body");
        spriteRenderer = child.GetComponent<SpriteRenderer>();

        CalibrateAccelerometer();
    }

    void Awake () {
        rb2d = GetComponent<Rigidbody2D>();
	}

    void Update() {

        CreateGameField();
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (!Physics.Raycast(transform.position, Vector3.down,  transform.localScale.y))
        {
            if (getBoosted())
            {
                if (spriteRenderer.sprite != playerSprites.boosterSprite)
                {
                    spriteRenderer.sprite = playerSprites.boosterSprite;
                }
            }else{

                if (spriteRenderer.sprite != playerSprites.jumpSprite)
                {
                    if (savedPositionY < transform.position.y)
                    {
                        spriteRenderer.sprite = playerSprites.jumpSprite;
                    }
                }

                if (spriteRenderer.sprite != playerSprites.fallSprite)
                {
                    if (savedPositionY > transform.position.y)
                    {
                        spriteRenderer.sprite = playerSprites.fallSprite;
                        jumpState = false;
                    }
                }

                if (grounded && !jumpState)
                {
                    if (spriteRenderer.sprite != playerSprites.normalSprite)
                    {
                        spriteRenderer.sprite = playerSprites.normalSprite;
                    }
                }                
            }
            savedPositionY = transform.position.y;
        }
        
        if (Input.GetButtonDown("Fire1") && grounded)
        {
            jump = true;
        }

        if (Input.GetButtonDown("Fire1") && !grounded)
        {
            firingShot.nextFire = Time.time + firingShot.fireRate;
            Instantiate(firingShot.shot, firingShot.shotSpawn.position, firingShot.shotSpawn.rotation);
        }
        
        GameObject[] boundary = GameObject.FindGameObjectsWithTag("Boundary");
        Vector3 boundaryPosition;

        for (int i = 0; i < boundary.Length; i++)
        {
            if (lastPositionY[i] < transform.position.y)
            {
                boundaryPosition = boundary[i].transform.position;
                boundary[i].transform.position = new Vector3(boundaryPosition.x, transform.position.y, boundaryPosition.z);
                lastPositionY[i] = transform.position.y;
            }
        }
    }

    void LateUpdate()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        Transform mainTransform = mainCamera.transform;
        mainTransform.position = new Vector3(0, mainTransform.position.y, mainTransform.position.z);
    }

    void FixedUpdate()
    {
        //float acceleration = Input.acceleration.x;
        float acceleration = Input.GetAxis("Horizontal");

        if (acceleration * rb2d.velocity.x < maxSpeed)
        {
            rb2d.AddForce(Vector2.right * acceleration * moveForce);
        }

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
        {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
        }
        
        if (jump)
        {
            if (firstRun)
            {
                rb2d.AddForce(new Vector2(0f, jumpForce));
                jumpForce = 1000f;
            }
            jumpState = true;
            jump = false;
            firstRun = true;
        }

        /* Flip element direction
        if(accelerationRaw.x > 0 && !facingRight)
        {
            Flip();
        }
        else if(accelerationRaw.x < 0 && facingRight)
        {
            Flip();
        }
        */
    }

    //Turn Player around based on x position
    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Coin"))
        {
            playerScore += pointsPerCoin;
            scoreText.text = "Score: "+ playerScore;
        }

        if (other.CompareTag("Boost"))
        {
            playerScore += pointsPerBoost;
            scoreText.text = "Score: " + playerScore;

            if (!getBoosted())
            {
                StartCoroutine(SpeedBoost());
                jump = true;
            }
        }
    }

    IEnumerator SpeedBoost()
    {
        setBoosted(true);
        jumpForce = 10000f;
        yield return new WaitForSeconds(2.0f);
        jumpForce = 1000f;
        setBoosted(false);
    }

    void CreateGameField()
    {
        SpawnManager.instance.SpawnBackground(transform.position.y);
        SpawnManager.instance.Spawn(1, transform.position.y);
    }

    void CalibrateAccelerometer()
    {
        Vector3 accelerationSnapshot = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
    }
}
