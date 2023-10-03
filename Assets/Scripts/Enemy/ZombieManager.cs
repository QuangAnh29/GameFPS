using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{    public ZombieHand zombieHand;    public int damage;    private void Start()
    {
        zombieHand.damage = damage;
    }}
