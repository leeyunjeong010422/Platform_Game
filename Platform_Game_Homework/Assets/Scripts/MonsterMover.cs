using UnityEngine;

public class MonsterMover : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] CircleCollider2D collider2d;

    [SerializeField] int nextMove;
    [SerializeField] float speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<CircleCollider2D>();

        Invoke("RandomMove", 3);
    }

    private void FixedUpdate()
    {
        //�������� �̵���Ŵ
        rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y);

        Vector2 frontVector = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);

        Debug.DrawRay(frontVector, Vector3.down, new Color(0, 1, 0));

        //����ĳ��Ʈ�� �տ� ���� Ȯ���ؼ�
        RaycastHit2D hit = Physics2D.Raycast(frontVector, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if (hit.collider == null)
        {
            Turn(); //���� �ƴϸ� ������ ��ȯ��
        }
    }

    private void RandomMove() //���͸� �������� �����̰� ��
    {
        nextMove = Random.Range(-1, 2); // -1, 0, 1 ����

        animator.SetInteger("WalkSpeed", nextMove);

        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }

        Invoke("RandomMove", 3);
    }

    private void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("RandomMove", 3);
    }

    //�÷��̾�� ���ظ� ������
    public void OnDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        collider2d.enabled = false;

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        Invoke("DeActive", 5);
    }

    private void DeActive()
    {
        gameObject.SetActive(false);
    }
}
