using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int bulletDamage;

    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Target"))
        {
            Debug.Log("hit" + objectWeHit.gameObject.name + "!");
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }

        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            Debug.Log("hit a wall");
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }

        if (objectWeHit.gameObject.CompareTag("Beer"))
        {
            
            Debug.Log("hit a beer bottle");
            objectWeHit.gameObject.GetComponent<BeerBottle>().Shatter();
            
        }

        if (objectWeHit.gameObject.CompareTag("Zombie"))
        {
            objectWeHit.gameObject.GetComponent<Zombie>().TakeDamage(bulletDamage);
            Destroy(gameObject);

        }
    }


    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        GameObject hole = Instantiate(
            GobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }
}
