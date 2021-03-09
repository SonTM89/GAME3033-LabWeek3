using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : ZombieStates
{
    private static readonly int MovementZHash = Animator.StringToHash("MovementZ");

    public ZombieIdleState(ZombieComponent zombie, StateMachine stateMachine) : base(zombie, stateMachine)
    {

    }

    public override void Start()
    {
        base.Start();
        OwnerZombie.ZombieNavmesh.isStopped = true;
        OwnerZombie.ZombieNavmesh.ResetPath();
        OwnerZombie.ZombieAnimator.SetFloat(MovementZHash, 0.0f);
    }
}
