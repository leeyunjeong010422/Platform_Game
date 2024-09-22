using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;
    [SerializeField] public SoundManager soundManager;
    [SerializeField] public PetController petController;

    [SerializeField] public Rigidbody2D rigid;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public Animator animator;
    [SerializeField] public CircleCollider2D collider2d;

    [SerializeField] public float movePower;
    [SerializeField] public float maxMoveSpeed;
    [SerializeField] public float jumpPower;
    [SerializeField] public float maxFallSpeed;

    [SerializeField] public bool isGrounded;

    private IPlayerState currentState;

    //private float x;

    public static int idleHash = Animator.StringToHash("Idle");
    public static int runHash = Animator.StringToHash("Run");
    public static int jumpHash = Animator.StringToHash("Jump");

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<CircleCollider2D>();
        ChangeState(new IdleState());
    }

    private void Update()
    {
        GroundCheck();
        currentState?.Update(this);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            ChangeState(new JumpState());
        }

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * x * movePower, ForceMode2D.Force);

        if (rigid.velocity.x > maxMoveSpeed)
        {
            rigid.velocity = new Vector2(maxMoveSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < -maxMoveSpeed)
        {
            rigid.velocity = new Vector2(-maxMoveSpeed, rigid.velocity.y);
        }

        if (x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (rigid.velocity.y < -maxFallSpeed)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -maxFallSpeed);
        }
    }

    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    public void SetAnimation(int animationHash)
    {
        animator.Play(animationHash);
    }

    public bool IsMoving()
    {
        return Mathf.Abs(rigid.velocity.x) > 0.01f;
    }

    public bool IsJumping()
    {
        return Input.GetKeyDown(KeyCode.Space) && isGrounded;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 1f, LayerMask.GetMask("Ground"));
        isGrounded = hit.collider != null;
    }

    public void OnDie()
    {
        ChangeState(new DeadState());
    }

    public void OffDamaged()
    {
        gameObject.layer = 3;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            bool isAttack = false;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.point.y > collision.transform.position.y + 0.1f)
                {
                    isAttack = true;
                    break;
                }
            }

            if (isAttack)
            {
                gameManager.AddScore(200);
                soundManager.PlayMonsterDieSound();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                collision.gameObject.GetComponent<MonsterMover>().OnDamaged();
            }
            else
            {
                if (petController != null && petController.isActive)
                {
                    petController.TakeDamage();
                }
                else
                {
                    ChangeState(new DamagedState(collision.transform.position));
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            int points = 0;

            if (isBronze)
            {
                points = 50;
            }
            else if (isSilver)
            {
                points = 70;
            }
            else if (isGold)
            {
                points = 100;
            }

            soundManager.PlayCoinSound();
            gameManager.AddScore(points);

            collision.gameObject.SetActive(false);
        }

        else if (collision.gameObject.tag == "Finish")
        {
            soundManager.StopBGM();
            gameManager.GameClear();
        }
    }
}
