using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public int damage = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damageable"))
        {
            other.GetComponentInParent<HPController>().DealDamage(damage);
        }
        if (other.CompareTag("Obstacle"))
        {
            GetComponentInParent<HPController>().Death();          
        }
        if (other.CompareTag("DealDamage"))
        {
            other.GetComponentInParent<HPController>().DealDamage(damage/2);
        }
    }
}
