using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;
    private bool isDead = false;

    private NavMeshAgent navAgent;

    private float timeUntilDisappear = 5.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (isDead)
        {
            return; 
        }
        if (HP <= 0)
        {
            isDead = true;
            int RandomValue = Random.Range(0, 2);

            if(RandomValue == 0)
            {
                animator.SetTrigger("Die1");

            }
            else
            {
                animator.SetTrigger("Die2");
            }

            StartCoroutine(DisappearAfterDelay());

        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }

    private IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(timeUntilDisappear);

        // Sau 5 giây, hủy đối tượng Zombie
        Destroy(gameObject);
    }
}
