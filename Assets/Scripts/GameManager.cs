using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int level;
    public int money;
    public List<Level> levels;
    private float prevSpeed;
    private float prevAcc;
    private int prevHp;
    public ArcadeVehicleController player;
    public TextMeshProUGUI moneyText;
    private List<Upgrade> speedUpgrade = new();
    private List<Upgrade> accUpgrade = new();
    private List<Upgrade> hpUpgrade = new();
    public TextAsset speedUpgradeText;
    public TextAsset accUpgradeText;
    public TextAsset hpUpgradeText;
    int levelSpeed = 0;
    int levelHp = 0;
    int levelAcc = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        ParseUpgrades();
        if (PlayerPrefs.GetInt("level")>=1)
        {
            level = PlayerPrefs.GetInt("level");
            money = PlayerPrefs.GetInt("money");
            levelSpeed = PlayerPrefs.GetInt("levelSpeed");
            levelHp = PlayerPrefs.GetInt("levelHp");
            levelAcc = PlayerPrefs.GetInt("levelAcc");
            SetPrevDef();
            UpdateDef();
        }
        UpdateMoney();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetInt("levelSpeed" , levelSpeed);
        PlayerPrefs.SetInt("levelHp", levelHp);
        PlayerPrefs.SetInt("levelAcc", levelAcc);
        PlayerPrefs.Save();
    }

    public void PlusSpeed()
    {
        if (money >= speedUpgrade[levelSpeed].UpdradeCost)
        {
            RemoveMoney(speedUpgrade[levelSpeed].UpdradeCost);
            levelSpeed++;
            UpdateDef();
        }
    }
    public void PlusAcceleration()
    {
        if (money >= accUpgrade[levelAcc].UpdradeCost)
        {
            RemoveMoney(accUpgrade[levelAcc].UpdradeCost);
            levelAcc++;
            UpdateDef();
        }
    }

    public void PlusHP()
    {
        if (money >= hpUpgrade[levelHp].UpdradeCost)
        {
            RemoveMoney(hpUpgrade[levelHp].UpdradeCost);
            levelHp++;
            UpdateDef();
        }
    }

    public void AddMoney(int ammount)
    {
        this.money += ammount;
    }

    public void RemoveMoney(int ammount) {  this.money -= ammount; }




    private void UpdateDef()
    {
        player.MaxSpeed = speedUpgrade[levelSpeed].addAmount + prevSpeed; 
        player.accelaration = accUpgrade[levelAcc].addAmount + prevAcc;
        player.GetComponentInChildren<HPController>().health = (int)hpUpgrade[levelHp].addAmount + prevHp;
        UpdateMoney();
        Save();
    }

    public void UpdateMoney()
    {
        moneyText.text = money.ToString();
    }

    void ParseUpgrades()
    {
        var lines = speedUpgradeText.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            var values = lines[i].Trim().Split(' ');
            var cost = int.Parse(values[0]);
            var amount = float.Parse(values[1]);
            speedUpgrade.Add(new Upgrade(cost, amount));
        }
        var lines2 = accUpgradeText.text.Split('\n');
        for (int i = 0; i < lines2.Length; i++)
        {
            var values = lines2[i].Trim().Split(' ');
            Debug.Log(values[0]);
            Debug.Log(values[1]);
            var cost = int.Parse(values[0]);
            var amount = float.Parse(values[1]);
            accUpgrade.Add(new Upgrade(cost, amount));
        }
        var lines3 = hpUpgradeText.text.Split('\n');
        for (int i = 0; i < lines3.Length; i++)
        {
            var values = lines3[i].Trim().Split(' ');
            Debug.Log(values[0]);
            Debug.Log(values[1]);
            var cost = int.Parse(values[0]);
            var amount = float.Parse(values[1]);
            hpUpgrade.Add(new Upgrade(cost, amount));
        }
    }

    void SetPrevDef()
    {
        prevSpeed = player.MaxSpeed;
        prevAcc = player.accelaration;
        prevHp = player.GetComponentInChildren<HPController>().health;
    }
}
