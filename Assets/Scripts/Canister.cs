using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canister : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInChildren<HPController>().health = other.GetComponentInChildren<HPController>().maxHealth;
            gameObject.SetActive(false);
        }
    }
}
