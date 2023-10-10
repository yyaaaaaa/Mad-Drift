using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private float timeCap = 10;
    private float timer = 0;
    private int level = 0;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    private int minReward = 10;
    private int maxReward = 100;
    public static LevelManager instance;
    bool timerIsRunning = false;
    public Slider slider;
    public GameObject player;

    private float spawnTimer = 0f;
    private float spawnInterval = 5f;

    private List<GameObject> enemies = new List<GameObject>();
    private int reward = 0;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI loseRewardText;
    public TextMeshProUGUI addRewardText;
    private List<Level> levels = new();

    public TextAsset levelsTextAsset;

    

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }       
        instance = this;
        ParseLevels();
        level = GameManager.instance.level;
        LoadLevel(level);
    }

        void Update()
    {
        if (timerIsRunning)
        {
            if (timer < timeCap)
            {
                timer += Time.deltaTime;
                slider.value = timer;
                spawnTimer += Time.deltaTime;
                reward = (int)(timer * (1 + level / 100));
                rewardText.text = reward.ToString();
                loseRewardText.text = "+" + reward.ToString();
                if (enemies.Count <= 0)
                {
                    SpawnEnemy();
                }

                if (spawnTimer >= spawnInterval)
                {
                    SpawnEnemy();
                    spawnTimer = 0f; // —брасываем таймер
                }
            }
            else
            {
                timerIsRunning = false;
                OnLevelComplete();
                timer = timeCap;
            }
        }
    }

    private void OnLevelComplete()
    {
        int addReward = Random.Range(minReward, maxReward);
        UIManager.instance.WinLevel();
        int totalReward = addReward + reward; 
        addRewardText.text = "+" + totalReward.ToString();
        GameManager.instance.AddMoney(totalReward);
        GameManager.instance.level += 1;
        level += 1;
        GameManager.instance.UpdateMoney();
        GameManager.instance.Save();
        LoadLevel(level);
    }

    public void LevelStart()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        GameManager.instance.UpdateMoney();
        player.SetActive(false);
        enemies.Clear();
        player.transform.position = Vector3.zero;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponentInChildren<HPController>().health = 100;
        player.SetActive(true);
        timer = 0;
        timerIsRunning = true;
        slider.maxValue = timeCap;
        SpawnEnemy();
        Time.timeScale = 1f;
    }

    public void SpawnEnemy()
    {
        int amount = Random.Range(1, 3);
        int random = Random.Range(0, enemiesToSpawn.Capacity);

        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = Instantiate(enemiesToSpawn[random], new Vector3(player.transform.position.x - 13f + random, 0, player.transform.position.z - 60f - random), Quaternion.identity);
            enemy.GetComponent<ArcadeVehicleController>().player = player;
            enemies.Add(enemy);
        }
    }

    void LoadLevel(int neededLevel)
    {
        if (neededLevel <= levels.Count)
        {
            maxReward = levels[neededLevel].maxReward;
            minReward = levels[neededLevel].minReward;
            timeCap = levels[neededLevel].time;
        }
        else
        {
            maxReward = 200;
            minReward = 100;
            timeCap = 45f;
        }
    }

    void ParseLevels()
    {
        var lines = levelsTextAsset.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            var values = lines[i].Split(' ');
            int levelNumber = int.Parse(values[0]);
            float time = float.Parse(values[1]);
            int minReward = int.Parse(values[2]);
            int maxReward = int.Parse(values[3]);
            levels.Add(new Level(levelNumber, time, minReward, maxReward));
        }

    }
     
    public int GetReward() { return reward; }
}
