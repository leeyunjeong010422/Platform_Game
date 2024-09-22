using UnityEngine;

public class RunningState : IPlayerState
{
    public override void Enter(PlayerController player)
    {
        player.SetAnimation(PlayerController.runHash);
    }

    public override void Update(PlayerController player)
    {
        if (!player.IsMoving())
        {
            player.ChangeState(new IdleState());
        }
    }
}
