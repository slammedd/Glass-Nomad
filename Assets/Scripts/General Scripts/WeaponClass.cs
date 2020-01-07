﻿using UnityEngine;

public class WeaponClass
{
    public enum FireType
    {
        Single,
        Burst,
        FullAuto
    }
    // Number of magazines for the weapon.
    public int magCount;
    public FireType fireMode;

    protected int bulletsInCurrentMag;

    // inimum time delay between each shot.
    private float fireRate;
    // Max number of bullets stored in a magazine.
    private int magSize;
    private float range;
    private float damage;

    protected WeaponClass(int magazineCount, float shotsPerSecond, int magazingSize, float weaponRange, float weaponDamage)
    {
        magCount = magazineCount;
        fireRate = 1 / shotsPerSecond;
        magSize = magazingSize;
        range = weaponRange;
        damage = weaponDamage;

        // Starts the weapon with a full magazine.
        bulletsInCurrentMag = magSize;
    }

    protected void FireWeapon(ref float deltaTime, GameObject camera)
    {
        if (canFire(deltaTime))
        {
            deltaTime -= fireRate;

            RaycastHit hit;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
            {

            }
        }
    }

    private bool canFire(float deltaTime)
    {
        if (deltaTime > fireRate)
        {
            return true;
        }

        return false;
    }
}
