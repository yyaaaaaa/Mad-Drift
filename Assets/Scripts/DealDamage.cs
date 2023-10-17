using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public GameObject FX;
    public int damage = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damageable"))
        {
            FX.SetActive(true);
            other.GetComponentInParent<HPController>().DealDamage(damage);
        }
        if (other.CompareTag("Obstacle"))
        {
            GetComponentInParent<HPController>().Death();          
        }
        if (other.CompareTag("DealDamage"))
        {
            FX.SetActive(true);
            other.GetComponentInParent<HPController>().DealDamage(damage/2);
        }
    }
}
