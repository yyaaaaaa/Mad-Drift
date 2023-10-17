using UnityEngine;

public class Coin : MonoBehaviour
{
    private int amount = 1;
    public GameObject FX;
    public GameObject FFX;
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 200, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddMoney(amount);
            FX.SetActive(true);
            gameObject.SetActive(false);
            FFX.SetActive(false);
        }
    }
}
