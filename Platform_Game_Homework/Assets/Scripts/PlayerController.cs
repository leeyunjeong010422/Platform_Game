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

    private float x;

    private static int idleHash = Animator.StringToHash("Idle");
    private static int runHash = Animator.StringToHash("Run");
    private static int jumpHash = Animator.StringToHash("Jump");

    private int curAniHash;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        GroundCheck();
        AnimatorPlay();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigid.AddForce(Vector2.right * x * movePower, ForceMode2D.Force);

        if (rigid.velocity.x > maxMoveSpeed)
        {
            rigid.velocity = new Vector2(maxMoveSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < -maxMoveSpeed)
        {
            rigid.velocity = new Vector2(-maxMoveSpeed, rigid.velocity.y);
        }

        if (rigid.velocity.y < -maxFallSpeed)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -maxFallSpeed);
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

    private void Jump()
    {
        if (!isGrounded)
            return;

        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        isGrounded = false;
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 1f, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void AnimatorPlay()
    {
        int checkAniHash;

        if (rigid.velocity.y > 0.01f)
        {
            checkAniHash = jumpHash;
        }

        else if (rigid.velocity.sqrMagnitude < 0.01f)
        {
            checkAniHash = idleHash;
        }
        else
        {
            checkAniHash = runHash;
        }

        if (curAniHash != checkAniHash)
        {
            curAniHash = checkAniHash;
            animator.Play(curAniHash);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Enemy 태그를 가지고 있는 적과 충돌을 하면
        if (collision.collider.CompareTag("Enemy"))
        {
            bool isAttack = false;

            //충돌지점을 체크해서
            foreach (ContactPoint2D contact in collision.contacts)
            {
                //적의 위쪽으로 닿으면
                if (contact.point.y > collision.transform.position.y + 0.1f)
                {
                    //공격한 것으로 함
                    isAttack = true;
                    break;
                }
            }

            if (isAttack)
            {
                OnAttack(collision.transform);
            }
            else
            {
                OnDamaged(collision.transform.position);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //아이템과 충돌했을 떄
        if (collision.gameObject.tag == "Item")
        {
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            int points = 0;

            //아이템 종류에 따라 얻는 점수 다름
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

        //Finish 태그와 충돌했을 때
        else if (collision.gameObject.tag == "Finish")
        {
            soundManager.StopBGM();
            gameManager.GameClear();
        }
    }


    //적을 공격했을 때
    private void OnAttack(Transform enemy)
    {
        gameManager.stagePoint += 200;

        soundManager.PlayMonsterDieSound();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        MonsterMover enemyMove = enemy.GetComponent<MonsterMover>();
        enemyMove.OnDamaged();
    }

    //적에게 피해를 받았을 때
    private void OnDamaged(Vector2 targetPos)
    {
        gameManager.HealthDown();

        gameObject.layer = 8;

        spriteRenderer.color = new Color(1, 1, 1, 0.4f); //캐릭터 색 반투명으로 변경

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1; //어느쪽으로 피해를 받을 건지 피해 받을 방향
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse); //부딪혔을 때 받는 힘

        animator.SetTrigger("doDamaged");
        Invoke("OffDamaged", 3);
    }

    private void OffDamaged() //3초 후에 피해 받기 전 원래 상태로 돌아옴
    {
        gameObject.layer = 3;

        spriteRenderer.color = new Color(1, 1, 1, 1); //색을 원래 상태로 돌림
    }

     public void OnDie()
    {
        rigid.velocity = Vector2.zero;

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        collider2d.enabled = false;

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }
}
