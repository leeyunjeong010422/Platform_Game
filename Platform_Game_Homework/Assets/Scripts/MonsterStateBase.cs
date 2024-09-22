using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateBase
{
    public virtual void Enter(MonsterMover monster) { }
    public virtual void Update(MonsterMover monster) { }
    public virtual void Exit(MonsterMover monster) { }
}