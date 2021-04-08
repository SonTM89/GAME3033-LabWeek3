using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable", order = 1)]
    public class ConsumableScriptable : ItemScriptable
    {
        public int Effect = 0;

        public override void UseItem(PlayerController _controller)
        {
            if (_controller.Health.Health >= _controller.Health.MaxHealth) return;

            _controller.Health.HealPlayer(Effect);

            ChangeAmount(-1);

            if (Amount <= 0)
            {
                DeleteItem(_controller);
            }
        }
    }
}