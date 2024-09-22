using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void Enter(PlayerController player);
    public void Update(PlayerController player);
    public void Exit(PlayerController player);
}
