using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] SoundManager soundManager;

    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] CircleCollider2D collider2d;

    [SerializeField] float movePower;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float maxFallSpeed;

    [SerializeField] bool isGrounded;

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

    public void PerformJump()
    {
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        isGrounded = false; // ���� �ÿ��� ���鿡�� ������ ������ �����մϴ�.
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

    public void OnDamaged(Vector2 targetPos)
    {
        gameManager.HealthDown(); // �÷��̾� ü�� ���� ó��

        gameObject.layer = 8; // ���� ���̾�� ����

        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // �÷��̾ �������ϰ� ����

        // �浹 ���⿡ ���� �÷��̾ �з����� ���� ����
        int direction = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(direction, 1) * 7, ForceMode2D.Impulse);

        animator.SetTrigger("doDamaged"); // �ǰ� �ִϸ��̼� ����
        Invoke("OffDamaged", 3); // 3�� �Ŀ� ���� ���� ����
    }

    private void OffDamaged()
    {
        gameObject.layer = 3; // ���� ���̾�� ����
        spriteRenderer.color = new Color(1, 1, 1, 1); // ���� ������� ����
    }



    public void OnDie()
    {
        rigid.velocity = Vector2.zero; // �÷��̾��� ��� �������� ����

        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // �÷��̾ �������ϰ� ����

        spriteRenderer.flipY = true; // �÷��̾ ������

        collider2d.enabled = false; // �浹�� ��Ȱ��ȭ�Ͽ� �ٸ� ������Ʈ�� �浹���� �ʵ��� ��

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse); // ���� ���� �־� �������� �������� ��

        // �߰��� GameManager���� ���� ���� ó���� �� �� ����
        //gameManager.OnPlayerDie();
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
                gameManager.stagePoint += 200;
                soundManager.PlayMonsterDieSound();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                collision.gameObject.GetComponent<MonsterMover>().OnDamaged();
            }
            else
            {
                ChangeState(new DamagedState(collision.transform.position));
            }
        }
    }


private void OnTriggerEnter2D(Collider2D collision)
    {
        //�����۰� �浹���� ��
        if (collision.gameObject.tag == "Item")
        {
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            int points = 0;

            //������ ������ ���� ��� ���� �ٸ�
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

        //Finish �±׿� �浹���� ��
        else if (collision.gameObject.tag == "Finish")
        {
            soundManager.StopBGM();
            gameManager.GameClear();
        }
    }
}
