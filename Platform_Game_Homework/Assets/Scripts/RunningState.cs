using UnityEngine;

public class RunningState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.SetAnimation(PlayerController.runHash);
    }

    public void Update(PlayerController player)
    {
        if (!player.IsMoving())
        {
            player.ChangeState(new IdleState());
        }

        if (player.IsJumping())
        {
            player.ChangeState(new JumpState());
        }
    }

    public void Exit(PlayerController player) { }
}
