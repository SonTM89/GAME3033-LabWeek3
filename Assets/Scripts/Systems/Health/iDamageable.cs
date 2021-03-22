using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Health
{
    public interface iDamageable
    {
        void TakeDamage(float damage);

        void Destroy();
    }
}