using UnityEngine;

public class HPController : MonoBehaviour
{
    public int health = 100;
    public GameObject smokegm;
    public void DealDamage(int damage)
    {
        if(health > 0)
        health -= damage;
    }
    private void Update()
    {
        if (health <= 0)
        {
            health = 0;
            Death();
        }
        if (this.gameObject.CompareTag("Player") && health <= 50)
        {
            smokegm.SetActive(true);
        }
        else if(this.gameObject.CompareTag("Player"))
            smokegm.SetActive(false);
    }

    public void Death()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            UIManager.instance.LoseLevel();
            GameManager.instance.AddMoney(LevelManager.instance.GetReward());
            GameManager.instance.UpdateMoney();
            health = 100;
            transform.parent.gameObject.SetActive(false);
        }
        else 
        Destroy(transform.parent.gameObject);
    }
}
