using UnityEngine;

public class JumpState : IPlayerState
{
    public override void Enter(PlayerController player)
    {
        player.SetAnimation(PlayerController.jumpHash);
        PerformJump(player);
    }

    public override void Update(PlayerController player)
    {
        if (player.IsGrounded())
        {
            player.ChangeState(new IdleState());
        }
    }

    private void PerformJump(PlayerController player)
    {
        player.rigid.AddForce(Vector2.up * player.jumpPower, ForceMode2D.Impulse);
        player.isGrounded = false;
    }
}
