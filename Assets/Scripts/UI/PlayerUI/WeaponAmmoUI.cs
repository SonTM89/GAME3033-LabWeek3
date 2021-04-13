 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponAmmoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text WeaponNameText;
    [SerializeField] private TMP_Text CurrentBulletText;
    [SerializeField] private TMP_Text TotalBulletText;

    private WeaponComponent weaponComponent;


    private void OnEnable()
    {
        PlayerEvents.OnWeaponEquipped += OnWeaponEquipped;
    }


    private void OnWeaponEquipped(WeaponComponent weaponcomponent)
    {
        weaponComponent = weaponcomponent;
    }


    private void OnDisable()
    {
        PlayerEvents.OnWeaponEquipped -= OnWeaponEquipped;
    }

    private void Update()
    {
        if (!weaponComponent) return;

        WeaponNameText.text = weaponComponent.WeaponInformation.WeaponName;
        CurrentBulletText.text = weaponComponent.WeaponInformation.BulletsInClip.ToString();
        TotalBulletText.text = weaponComponent.WeaponInformation.BulletsAvailable.ToString();
    }
}
