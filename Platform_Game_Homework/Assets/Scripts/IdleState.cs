using UnityEngine;

public class IdleState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.SetAnimation(PlayerController.idleHash);
    }

    public void Update(PlayerController player)
    {
        if (player.IsMoving())
        {
            player.ChangeState(new RunningState());
        }

        if (player.IsJumping())
        {
            player.ChangeState(new JumpState());
        }
    }

    public void Exit(PlayerController player) { }
}
