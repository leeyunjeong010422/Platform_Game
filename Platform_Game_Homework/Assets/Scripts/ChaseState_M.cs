using UnityEngine;

//�÷��̾ �����ϴ� ����
public class ChaseState_M : MonsterStateBase
{
    public override void Update(MonsterMover monster)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        float distance = Vector2.Distance(monster.transform.position, player.transform.position);

        if (distance < 1f) //�Ÿ��� 1���� ���� ª����
        {
            monster.ChangeState(new AttackState_M()); //���� ���·� ��ȯ
        }
        else if (distance < 3f) //3���� ���� ª����
        {
            Vector2 direction = (player.transform.position - monster.transform.position).normalized;

            monster.spriteRenderer.flipX = direction.x > 0; //���� ��ȯ

            monster.rigid.velocity = new Vector2(direction.x * monster.speed, monster.rigid.velocity.y); //�÷��̾� ����
        }
        else //�Ѵ� �ƴϸ� �ٽ� ���� �̵� ���·� ��ȯ
        {
            monster.ChangeState(new RandomMoveState_M());
        }
    }
}
