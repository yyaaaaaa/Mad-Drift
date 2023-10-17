using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HPController : MonoBehaviour
{
    public float health = 100;
    public GameObject smokegm;
    public Slider slider;
    public float maxHealth;
    public GameObject explosion;
    [SerializeField] float timeDamage = 1f;
    private bool isInvincible = false;
    public GameObject body;
    public GameObject wheels;
    public GameObject canvas;
    public FollowPlayer cam;
    public int reward = 10;
    public GameObject text;
    private void Start()
    {
        slider.maxValue = maxHealth;
        health = maxHealth;
    }
    public void DealDamage(int damage)
    {
        if (health > 0 && !isInvincible)
        {
            health -= damage;
           // StartCoroutine(DamageSeq());
        }  
    }
    private void Update()
    {
        slider.value = health;
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
        Instantiate(explosion, transform.position, transform.rotation);
        Handheld.Vibrate();
        if (this.gameObject.CompareTag("Player"))
        {
        StartCoroutine(DeathSequence());  
        }
        else
        {
            Destroy(transform.parent.gameObject);
            text.GetComponent<TextMeshProUGUI>().text = "Car crush +" + reward;
            GameManager.instance.AddMoney(reward);
            text.SetActive(true);
        }       
    }


    private IEnumerator DeathSequence()
    {
        float duration = 2.5f;
        body.SetActive(false);
        wheels.SetActive(false);
        canvas.SetActive(false);
        health = maxHealth;
        cam.enabled = false;
        Time.timeScale = 0.3f;
        // ∆дем duration с реальным временем
        yield return new WaitForSecondsRealtime(duration);
        UIManager.instance.LoseLevel();
        GameManager.instance.AddMoney(LevelManager.instance.GetReward());
        cam.enabled = true;
        body.SetActive(true);
        wheels.SetActive(true);
        canvas.SetActive(true);
    }
    private IEnumerator DamageSeq()
    {
        LevelManager.instance.DisableCollisions(gameObject);
        var boxcolliders = GetComponentsInChildren<BoxCollider>();
        foreach (var boxcollider in boxcolliders)
        {
            boxcollider.enabled = false;
        }
        isInvincible = true;
        yield return new WaitForSeconds(timeDamage);
        LevelManager.instance.EnableCollisions(gameObject);
        foreach (var boxcollider in boxcolliders)
        {
            boxcollider.enabled = true;
        }
        isInvincible = false;
    }

}
