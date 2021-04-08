using Character;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon", order = 2)]
    public class WeaponScriptable : EquippableScriptable
    {
        public WeaponStats weaponStats;

        public override void UseItem(PlayerController _controller)
        {
            if(Equipped)
            {
                _controller.WeaponHolder.UnEquipItem();
            }
            else
            {
                _controller.WeaponHolder.EquipWeapon(this);
            }

            base.UseItem(_controller);
        }
    }
}