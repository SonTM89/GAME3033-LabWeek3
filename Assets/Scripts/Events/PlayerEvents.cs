using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents
{
    public delegate void OnWeaponEquippedEvent(WeaponComponent weaponCOmponent);

    public static event OnWeaponEquippedEvent OnWeaponEquipped;

    public static void Invoke_OnWeaponEquipped(WeaponComponent weaponCOmponent)
    {
        OnWeaponEquipped?.Invoke(weaponCOmponent);
    }
}
