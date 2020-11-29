using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip landingAudio;
    [SerializeField]
    private AudioClip waterAudio;


    [SerializeField]
    private BoxCollider2D triggerCollL;
    [SerializeField]
    private BoxCollider2D triggerCollR;
    [SerializeField]
    private GameController gameController;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private CircleCollider2D circleColl;
    private BoxCollider2D boxColl;

    [SerializeField]
    private float jumpForce = 20.0F;
    public float speed = 10f;

    //изменения коллайдеров при Roll и возвращение обратно
    private Vector2 circleOffset;
    private Vector2 boxSize;
    private Vector2 boxOffset;
    private Vector2 triggerOffset;
    private Vector2 triggerSize;
    private Vector2 rollCircleOffset = new Vector2(0f, 0.16f);
    private Vector2 rollBoxSize = new Vector2(0.2f, 0.1f) ;
    private Vector2 rollBoxOffset = new Vector2(0f, 0.65f);
    private Vector2 rollTriggerOffset = new Vector2(0.7f, 0.68f);
    private Vector2 rollTriggerSize = new Vector2(0.19f, 1.11f);
    private float circleRadius;
    private const float rollCircleRadius = 0.2f;
    private const float rollBoxEdgeRadius = 0.47f;

    public bool isDead = false;
    public bool isLadder = false;
    public bool isGrounded = true;
    public bool isWater = false;
    public bool isRoll = false;
    public int isRun = 0;
    private bool hitR;
    private bool hitL;
    public bool hit;

    private int sphereLayerIndex1;
    private int sphereLayerIndex2;
    private int sphereLayerIndex3;
    private int layerMaskRoll;
    private int layerMaskGrounded;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        triggerOffset = triggerCollR.offset;
        triggerSize = triggerCollR.size;
        circleRadius = GetComponent<CircleCollider2D>().radius;
        boxOffset = GetComponent<BoxCollider2D>().offset;
        circleOffset = GetComponent<CircleCollider2D>().offset;
        boxSize = GetComponent<BoxCollider2D>().size;
        circleColl = GetComponent<CircleCollider2D>();
        boxColl = GetComponent<BoxCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sphereLayerIndex1 = LayerMask.NameToLayer("Default");
        sphereLayerIndex2 = LayerMask.NameToLayer("Ground");
        sphereLayerIndex3 = LayerMask.NameToLayer("Trigger");
        layerMaskRoll = (1 << sphereLayerIndex1) | (1 << sphereLayerIndex2);
        layerMaskGrounded = (1 << sphereLayerIndex1) | (1 << sphereLayerIndex2) | (1 << sphereLayerIndex3);
    }

    private void Update()
    {
        if (Input.GetButton("Vertical"))
        {
            rb.WakeUp();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isWater && !hit)
        {
            isRoll = true;
            Roll();
        }
        if (!Input.GetKey(KeyCode.LeftShift) && !isWater && !hit && isRoll)
        {
            isRoll = false;
            Idle();
        }
        if (Input.GetButton("Horizontal"))
        {
            rb.WakeUp();
            Run();
        }
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && !isWater)
        {
            Jump();
        }
    }

    private void CheckGround()
    {
        isLadder = GetComponent<CharacterAnimation>().isLadder;

        Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position + new Vector3(-0.45f, -0.2f, 0), transform.position + new Vector3(0.45f, 0.2f, 0), layerMaskGrounded);
        Collider2D[] colliders1 = Physics2D.OverlapAreaAll(transform.position + new Vector3(-0.45f, -0.2f, 0), transform.position + new Vector3(0.45f, 0.2f, 0), layerMaskRoll);

        bool isGrounded1 = colliders1.Length > 0;
        if (isGrounded != isGrounded1 && isGrounded == false && isRoll == false && isWater==false)
        {
            audioSource.PlayOneShot(landingAudio);
        }
        isGrounded = colliders.Length > 0;
    }

    private void FixedUpdate()
    {
        CheckGround();
        Raycasts();
    }

    private void Raycasts()
    {
        hitL = Physics2D.Raycast(transform.position + new Vector3(-0.4f, 1.2f, 0f), Vector2.up, 0.72f, layerMaskRoll);
        hitR = Physics2D.Raycast(transform.position + new Vector3(0.4f, 1.2f, 0f), Vector2.up, 0.72f, layerMaskRoll);
        hit = (hitL || hitR) == true;
    }

    private void Idle()
    {
        transform.position -= new Vector3(0f, 0.53f, 0f);
        sprite.transform.position = new Vector2(sprite.transform.position.x, sprite.transform.position.y + 0.53f);
        circleColl.radius = circleRadius;
        circleColl.offset = circleOffset;
        boxColl.size = boxSize;
        boxColl.offset = boxOffset;
        boxColl.edgeRadius = 0;
        triggerCollL.size = triggerSize;
        triggerCollL.offset = triggerOffset * new Vector2(-1, 1);
        triggerCollR.size = triggerSize;
        triggerCollR.offset = triggerOffset;
    }

    private void Roll()
    {
        transform.position += new Vector3(0f, 0.53f, 0f);
        sprite.transform.position = new Vector2(sprite.transform.position.x, sprite.transform.position.y - 0.53f);
        circleColl.radius = rollCircleRadius;
        circleColl.offset = rollCircleOffset;
        boxColl.offset = rollBoxOffset;
        boxColl.size = rollBoxSize;
        boxColl.edgeRadius = rollBoxEdgeRadius;
        triggerCollL.size = rollTriggerSize;
        triggerCollL.offset = rollTriggerOffset * new Vector2(-1,1);
        triggerCollR.size = rollTriggerSize;
        triggerCollR.offset = rollTriggerOffset;
    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        sprite.flipX = direction.x < 0.0f;
        if (isRun == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        }
        else if (isRun == 1)
        {
            float move = Input.GetAxis("Horizontal");    
            rb.velocity = new Vector2(move * ((speed / 2)+1), rb.velocity.y);
        }
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Dead()
    {
        isDead = true;
        gameController.LoseGame();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("DeathObjects"))
        {
            this.transform.parent = null;
            Dead();
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.name.Equals("MovingPlatform"))
        {
            this.transform.parent = col.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        this.transform.parent = null;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("DeathObjects"))
        {
            Dead();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water") && isWater==false)
        {
            audioSource.PlayOneShot(waterAudio);
        }
    }
}