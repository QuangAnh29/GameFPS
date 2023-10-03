using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerHP = 100;

    public void TakeDamage(int damageAmount)
    {
        playerHP -= damageAmount;

        
        if (playerHP <= 0)
        {
            Debug.Log("You Die");
        }
        else
        {
            Debug.Log("You Hit");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
        }
    }
}
