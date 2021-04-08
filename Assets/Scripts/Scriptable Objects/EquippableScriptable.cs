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
            }
        }

        private bool m_Equipped;

        public override void UseItem(PlayerController _controller)
        {
            m_Equipped = !m_Equipped;
        }
    }
}