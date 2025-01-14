using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Systems.Health;

public class AK47Component : WeaponComponent
{
    private Vector3 HitLocation;

    protected override void FireWeapon()
    {

        if(WeaponStats.BulletsInClip > 0 && !Reloading && !WeaponHolder.Controller.IsRunning)
        {
            base.FireWeapon();

            if(!FiringEffect)
            {
                FiringEffect = Instantiate(FiringAnimation, ParticleSpawnLocation).GetComponent<ParticleSystem>();
            }
            
            Ray screenRay = MainCamera.ScreenPointToRay(new Vector3(CrossHairComponent.CurrentAimPosition.x, CrossHairComponent.CurrentAimPosition.y, 0));

            if (!Physics.Raycast(screenRay, out RaycastHit hit, WeaponStats.FireDistance, WeaponStats.WeaponHitLayers)) return;

            HitLocation = hit.point;


            TakeDamage(hit);


            Vector3 hitDirection = hit.point - MainCamera.transform.position;
            Debug.DrawRay(MainCamera.transform.position, hitDirection.normalized * WeaponStats.FireDistance, Color.red);
        }
        else if(WeaponStats.BulletsInClip <= 0)
        {
            if (!WeaponHolder) return;

            WeaponHolder.StartReloading();
        }
    }

    private void TakeDamage(RaycastHit hitInfo)
    {
        iDamageable damageable = hitInfo.collider.GetComponent<iDamageable>();
        damageable?.TakeDamage(WeaponInformation.Damage);
    }

    private void OnDrawGizmos()
    {
        if (HitLocation == Vector3.zero) return;

        Gizmos.DrawWireSphere(HitLocation, 0.2f);

        
    }
}
