using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPlayerState
{
    public virtual void Enter(PlayerController player) { }
    public virtual void Update(PlayerController player) { }
    public virtual void Exit(PlayerController player) { }
}
