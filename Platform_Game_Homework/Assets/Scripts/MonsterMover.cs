using UnityEngine;

public class MonsterMover : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rigid;
    [SerializeField] public Animator animator;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public CircleCollider2D collider2d;

    public int nextMove;
    public float speed;

    private MonsterStateBase currentState;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<CircleCollider2D>();

        ChangeState(new RandomMoveState_M());
    }

    private void FixedUpdate()
    {
        currentState.Update(this);
        CheckPlayerDistance();
    }

    public void ChangeState(MonsterStateBase newState)
    {
        if (currentState != null)
            currentState.Exit(this);

        currentState = newState;
        currentState.Enter(this);
    }

    private void RandomMove()
    {
        nextMove = Random.Range(-1, 2);
        animator.SetInteger("WalkSpeed", nextMove);
        spriteRenderer.flipX = nextMove == 1;

        Invoke("RandomMove", 3);
    }

    public void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("RandomMove", 3);
    }

    public void OnDamaged()
    {
        ChangeState(new DamagedState_M());
    }

    private void DeActive()
    {
        gameObject.SetActive(false);
    }

    private void CheckPlayerDistance()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < 1f)
        {
            ChangeState(new AttackState_M());
        }
        else if (distance < 3f)
        {
            ChangeState(new ChaseState_M());
        }
    }
}
