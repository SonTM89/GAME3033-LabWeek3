using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Character;
using Character.UI;
using Scriptable_Objects;

public enum WeaponType
{
    None, 
    Pistol, 
    MachineGun
}

[Serializable]
public struct WeaponStats
{
    public WeaponType WeaponType;
    public string WeaponName;
    public float Damage; 
    public int BulletsInClip;
    public int ClipSize;
    public int BulletsAvailable;
    public float FireStartDelay;
    public float FireRate;
    public float FireDistance;
    public bool Repeating;
    public LayerMask WeaponHitLayers;
}

public class WeaponComponent : MonoBehaviour
{
    public Transform GripLocation => GripIKLocation;

    [SerializeField] private Transform GripIKLocation;
    [SerializeField] protected Transform ParticleSpawnLocation;


    public WeaponStats WeaponInformation => WeaponStats;

    [SerializeField] protected WeaponStats WeaponStats;

    [SerializeField] protected GameObject FiringAnimation;

    protected Camera MainCamera;
    protected WeaponHolder WeaponHolder;
    protected CrossHairScript CrossHairComponent;
    protected ParticleSystem FiringEffect;

    public bool Firing { get; private set; }

    public bool Reloading { get; private set; }

    private void Awake()
    {
        MainCamera = Camera.main;
    }

    public void Initialize(WeaponHolder weaponHolder, WeaponScriptable weaponScriptable)
    {
        WeaponHolder = weaponHolder;
        CrossHairComponent = WeaponHolder.CrossHair;

        if(weaponScriptable)
        {
            WeaponStats = weaponScriptable.weaponStats;
        }
    }

    public virtual void StartFiringWeapon()
    {
        Firing = true;
        
        if(WeaponStats.Repeating)
        {
            InvokeRepeating(nameof(FireWeapon), WeaponStats.FireStartDelay, WeaponStats.FireRate);
        }
        else
        {
            FireWeapon();
        }
    }

    public virtual void StopFiringWeapon()
    {
        Firing = false;

        if (FiringEffect) Destroy(FiringEffect.gameObject);

        CancelInvoke(nameof(FireWeapon));
    }

    protected virtual void FireWeapon()
    {
        Debug.Log("Firing Weapon");
        WeaponStats.BulletsInClip--;
    }

    public virtual void StartReloading()
    {
        Reloading = true;
        ReloadWeapon();
    }

    public virtual void StopReloading()
    {
        Reloading = false;
    }

    public virtual void ReloadWeapon()
    {
        if (FiringEffect) Destroy(FiringEffect.gameObject);

        int bulletsToReload = WeaponStats.ClipSize - WeaponStats.BulletsAvailable;

        if(bulletsToReload < 0)
        {
            WeaponStats.BulletsInClip = WeaponStats.ClipSize;
            WeaponStats.BulletsAvailable -= WeaponStats.ClipSize;
        }
        else
        {
            WeaponStats.BulletsInClip = WeaponStats.BulletsAvailable;
            WeaponStats.BulletsAvailable = 0;
        }
    }
}