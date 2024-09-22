using UnityEngine;

public class AttackState_M : MonsterStateBase
{
    [SerializeField] PlayerController playerController;
    public override void Enter(MonsterMover monster)
    {
        Debug.Log("АјАн!!");
    }

    public override void Update(MonsterMover monster)
    {
        monster.ChangeState(new RandomMoveState_M());
    }
}
