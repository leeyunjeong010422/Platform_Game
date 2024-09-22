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
    private RandomMoveState_M randomState = new RandomMoveState_M();
    private DamagedState_M damagedState = new DamagedState_M();

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<CircleCollider2D>();

        ChangeState(randomState);
    }

    private void FixedUpdate()
    {
        currentState.Update(this);
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
        ChangeState(damagedState);
    }

    private void DeActive()
    {
        gameObject.SetActive(false);
    }
}
