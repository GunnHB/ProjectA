using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData
{
    public int _attackAnimHash;
    public float _transitionDuration;
}

public interface IAttack
{
    public void RegistAttackData();

    // animation events
    public void StartCheckHitCollider();
    public void EndCheckHitCollider();
}
