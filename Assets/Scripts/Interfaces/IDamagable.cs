using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    Vector3 Position { get; }
    void Damage(float damage);
}
