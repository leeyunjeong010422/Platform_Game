using UnityEngine;

public class IdleState : IPlayerState
{
    public override void Enter(PlayerController player)
    {
        player.SetAnimation(PlayerController.idleHash);
    }

    public override void Update(PlayerController player)
    {
        if (player.IsMoving())
        {
            player.ChangeState(new RunningState());
        }
    }
}
