using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canister : MonoBehaviour
{
    public GameObject FX;
    public GameObject FFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FX.SetActive(true);
            other.GetComponentInChildren<HPController>().health = other.GetComponentInChildren<HPController>().maxHealth;
            gameObject.SetActive(false);
            FFX.SetActive(false);
        }
    }


}
