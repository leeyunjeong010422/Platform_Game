using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.OnDie();
    }

    public void Update(PlayerController player) { }

    public void Exit(PlayerController player) { }
}
