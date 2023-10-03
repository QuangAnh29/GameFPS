using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon hoveredWeapon = null;

    public AmmoBox hoveredAmmoBox = null;
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



    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            GameObject objectHitByRayCat = hit.transform.gameObject;
            float distanceToWeapon = Vector3.Distance(Camera.main.transform.position, objectHitByRayCat.transform.position);

            if (objectHitByRayCat.GetComponent<Weapon>() && objectHitByRayCat.GetComponent<Weapon>().isActiveWeapon == false)
            {
                

                if (distanceToWeapon <= 5)
                {
                    hoveredWeapon = objectHitByRayCat.gameObject.GetComponent<Weapon>();
                    hoveredWeapon.GetComponent<Outline>().enabled = true;

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        WeaponManager.Instance.PickupWeapon(objectHitByRayCat.gameObject);
                    }
                }
            }

            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            //Ammo Box

            if (objectHitByRayCat.GetComponent<AmmoBox>())
            {

                if (distanceToWeapon <= 5)
                {
                    hoveredAmmoBox = objectHitByRayCat.gameObject.GetComponent<AmmoBox>();
                    hoveredAmmoBox.GetComponent<Outline>().enabled = true;

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                        Destroy(objectHitByRayCat.gameObject);
                    }
                }
            }

            else
            {
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
