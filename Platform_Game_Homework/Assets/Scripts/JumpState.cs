using UnityEngine;

public class JumpState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.SetAnimation(PlayerController.jumpHash);
        player.PerformJump();
    }

    public void Update(PlayerController player)
    {
        if (player.IsGrounded())
        {
            player.ChangeState(new IdleState());
        }
    }

    public void Exit(PlayerController player) { }
}
