using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : IPlayerState
{
    private Vector2 targetPosition;

    public DamagedState(Vector2 targetPos)
    {
        targetPosition = targetPos;
    }

    public void Enter(PlayerController player)
    {
        player.OnDamaged(targetPosition);
    }

    public void Update(PlayerController player) { }

    public void Exit(PlayerController player) { }
}


