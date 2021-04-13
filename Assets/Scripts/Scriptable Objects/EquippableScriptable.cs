using Character;
using System.Collections;
using System.Collections.Generic;

namespace Scriptable_Objects
{
    public abstract class EquippableScriptable : ItemScriptable
    {
        public bool Equipped
        {
            get => m_Equipped;
            set
            {
                m_Equipped = value;
                OnEquipStatusChange?.Invoke();
            }
        }

        private bool m_Equipped = false;

        public delegate void EquipStatusChange();

        public event EquipStatusChange OnEquipStatusChange;

        public override void UseItem(PlayerController _controller)
        {
            Equipped = !Equipped;
        }
    }
}