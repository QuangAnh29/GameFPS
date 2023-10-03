using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage;

    //shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDeplay = 2f;

    //Burst
    public int bulletsPerBurst = 3;
    public int BurstBulletLeft;

    //Spread
    public float spreadIntensity;

    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;

    internal Animator animator;
    public GameObject muzzleEffect;

    //Loading
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    public enum WeaponModel
    {
        M1911,
        Heavy
    }

    public WeaponModel thisWeaponModel;
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        BurstBulletLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
    }

    private void Update()
    {
        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;

            if (bulletsLeft == 0 && isShooting)
            {
                AudioManager.instance.Play("EmptyMagazine");
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                //Hoding down left mouse button
                isShooting = Input.GetKey(KeyCode.Q);
            }

            else if (currentShootingMode == ShootingMode.Burst || currentShootingMode == ShootingMode.Single)
            {
                //clicking left mouse button once
                isShooting = Input.GetKeyDown(KeyCode.Q);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                ReLoad();
            }

            //If you want to automatically reload when magazine is empty
            if (readyToShoot && !isShooting && isReloading == false && bulletsLeft <= 0 &&  WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                ReLoad();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                BurstBulletLeft = bulletsPerBurst;
                FireWeapon();
            }

            /*if (AmmoManager.Instance.ammoDisplay != null)
            {
                AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/{magazineSize / bulletsPerBurst}";
            } */
        }
    }



    private void FireWeapon() 
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        AudioManager.instance.PlayShootingSound(thisWeaponModel);
        

        animator.SetTrigger("shot");
        

        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        //Poiting the billet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        //Shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        //Destroy the bullet after some time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        //Checking if we are done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDeplay);
            allowReset = false;
        }


        //Burst Mode
        if (currentShootingMode == ShootingMode.Burst && BurstBulletLeft > 1)
        {
            BurstBulletLeft--;
            Invoke("FireWeapon", shootingDeplay);
        }

    }

    private void ReLoad()
    {
        animator.SetTrigger("reload");
        //AudioManager.instance.Play("Reload");
        AudioManager.instance.PlayReloadSound(thisWeaponModel);
        isReloading = true;
        Invoke("ReLoadCompleted", reloadTime);
    }

    private void ReLoadCompleted()
    {
        if(WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseToTalAmmo(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseToTalAmmo(bulletsLeft, thisWeaponModel);
        }

        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        //shooting from the middle of the screen to check where are we pointing 
        /*Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            //hiting something
            targetPoint = hit.point;
        }
        else
        {
            //shootig at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //Returning the shooting direction and spread
        return direction + new Vector3(x, y, 0);*/

        // Tạo một raycast từ trung tâm màn hình
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 shootingDirection;

        if (Physics.Raycast(ray, out hit))
        {
            // Hướng bắn đến điểm hit
            shootingDirection = (hit.point - bulletSpawn.position).normalized;
        }
        else
        {
            // Nếu không trúng bất kì đối tượng nào, thì bắn thẳng đi
            shootingDirection = ray.direction;
        }

        return shootingDirection;

    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }


    
}
