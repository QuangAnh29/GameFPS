using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach(GameObject weaponSlot in weaponSlots)
        {
            if(weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }
    }

    public void PickupWeapon(GameObject pickupWeapon)
    {
        AddWeaponIntoActiveASlot(pickupWeapon);
    }

    private void AddWeaponIntoActiveASlot(GameObject pickupWeapon)
    {
        DropCurrentWeapon(pickupWeapon);

        pickupWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickupWeapon.GetComponent<Weapon>();

        pickupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickupWeapon)
    {
        if(activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickupWeapon.transform.localRotation;

        }
    }

    internal void PickupAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.RifmeAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
            
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if(activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;

        }
        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
             newWeapon.isActiveWeapon = true;

        }
    }

    internal void DecreaseToTalAmmo(int bulletsToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.Heavy:
                totalRifleAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.M1911:
                totalPistolAmmo -= bulletsToDecrease;
                break;
        }
    }

    public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.Heavy:
                return totalRifleAmmo;

            case Weapon.WeaponModel.M1911:
                return totalPistolAmmo;

            default:
                return 0;
        }
    }
}
