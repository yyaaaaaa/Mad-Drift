using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int level;
    public float speed;
    public float acceleration;
    public int hp;
    private int money;
    private int UpgradeCostSpeed = 1;
    private int UpgradeCostAcc = 1;
    private int UpgradeCostHP = 1;
    public List<Level> levels;
    private float prevSpeed;
    private float prevAcc;
    private int prevHp;
    public ArcadeVehicleController player;
    public TextMeshProUGUI moneyText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.GetInt("level")>=1)
        {
            level = PlayerPrefs.GetInt("level");
            speed = PlayerPrefs.GetFloat("speed");
            acceleration = PlayerPrefs.GetFloat("acceleration");
            hp = PlayerPrefs.GetInt("hp");
            money = PlayerPrefs.GetInt("money");
            UpgradeCostSpeed = PlayerPrefs.GetInt("upgradeCostSpeed");
            UpgradeCostAcc = PlayerPrefs.GetInt("upgradeCostAcc");
            UpgradeCostHP = PlayerPrefs.GetInt("upgradeCostHp");
            prevSpeed = player.MaxSpeed;
            prevAcc = player.accelaration;
            prevHp = player.GetComponentInChildren<HPController>().health;
            UpdateDef();
        }
        UpdateMoney();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetFloat("speed", speed);
        PlayerPrefs.SetFloat("acceleration", acceleration);
        PlayerPrefs.SetInt("hp", hp);
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetInt("upgradeCostSpeed", UpgradeCostSpeed);
        PlayerPrefs.SetInt("upgradeCostAcc", UpgradeCostAcc);
        PlayerPrefs.SetInt("upgradeCostHp", UpgradeCostHP);
        PlayerPrefs.Save();
    }

    public void PlusSpeed()
    {
        if (money >= UpgradeCostSpeed)
        {
            RemoveMoney(UpgradeCostSpeed);
            speed += 0.5f;
            UpgradeCostSpeed *= 2;
            UpdateDef();
        }
    }
    public void PlusAcceleration()
    {
        if (money >= UpgradeCostAcc)
        {
            RemoveMoney(UpgradeCostAcc);
            acceleration += 0.5f;
            UpgradeCostAcc *= 2;
            UpdateDef();
        }
    }

    public void PlusHP()
    {
        if (money >= UpgradeCostHP)
        {
            RemoveMoney(UpgradeCostHP);
            AddHp(1);
            UpgradeCostHP *= 2;
            UpdateDef();
        }        
    }

    public void AddMoney(int ammount)
    {
        this.money += ammount;
    }

    public void RemoveMoney(int ammount) {  this.money -= ammount; }

    public void AddHp(int ammount)
    {
        this.hp += ammount;
    }
    public void RemoveHp(int ammount) { this.hp -= ammount; }



    private void UpdateDef()
    {
        player.MaxSpeed = speed + prevSpeed; 
        player.accelaration = acceleration + prevAcc;
        player.GetComponentInChildren<HPController>().health = hp + prevHp;
        UpdateMoney();
        Save();
    }

    public void UpdateMoney()
    {
        moneyText.text = money.ToString();
    }
}
