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
    public List<GameObject> coinsModels = new List<GameObject>();
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

    public GameObject canister;
    public GameObject textobj;
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
                    SpawnOneTimes();
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
        GameManager.instance.Save();
        LoadLevel(level);
    }

    public void LevelStart()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        player.SetActive(false);
        enemies.Clear();
        player.transform.position = Vector3.zero;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponentInChildren<HPController>().health = player.GetComponentInChildren<HPController>().maxHealth;
        player.SetActive(true);
        timer = 0;
        timerIsRunning = true;
        slider.maxValue = timeCap;
        SpawnEnemy();
        Time.timeScale = 1f;
    }

    public void SpawnEnemy()
    {
        Vector3[] positions = new Vector3[4];
        positions[0] = new Vector3(player.transform.position.x - 37f, 0, player.transform.position.z);
        positions[1] = new Vector3(player.transform.position.x + 55f, 0, player.transform.position.z);
        positions[2] = new Vector3(player.transform.position.x, 0, player.transform.position.z - 37f);
        positions[3] = new Vector3(player.transform.position.x - 37f, 0, player.transform.position.z + 32f);
        int amount = Random.Range(1, level/10);
        for (int i = 0; i < amount; i++)
        {
            int random = Random.Range(0, enemiesToSpawn.Capacity);
            int randomPos = Random.Range(0, positions.Length);
            if (positions[randomPos] == Vector3.zero)
            {
                randomPos = Random.Range(0, positions.Length);
            }
            GameObject enemy = Instantiate(enemiesToSpawn[random], positions[randomPos], Quaternion.identity);
            positions[randomPos] = Vector3.zero;
            enemy.GetComponent<ArcadeVehicleController>().player = player;
            enemy.GetComponentInChildren<HPController>().text = textobj;
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

    public void SpawnOneTimes()
    {
        float randomCoin = Random.Range(100, 200);
        float randomCanister = Random.Range(100, 200);
        bool y = false;
        int random = Random.Range(0, coinsModels.Capacity);
        if (Random.Range(0, 2) == 1)
        {
            y = true;
        }

        if (y)
        {
            Instantiate(coinsModels[random], new Vector3(player.transform.position.x - randomCoin, 3f, player.transform.position.z), Quaternion.identity);
            Instantiate(canister, new Vector3(player.transform.position.x + randomCanister, 3f, player.transform.position.z), Quaternion.identity);
        }
        else
        {
            Instantiate(coinsModels[random], new Vector3(player.transform.position.x + randomCoin, 3f, player.transform.position.z), Quaternion.identity);
            Instantiate(canister, new Vector3(player.transform.position.x - randomCanister, 3f, player.transform.position.z), Quaternion.identity);
        }
    }

    public void DisableCollisions(GameObject obj)
    {
        obj.GetComponentInParent<BoxCollider>().enabled = false;
    }
    public void EnableCollisions(GameObject obj)
    {
        obj.GetComponentInParent<BoxCollider>().enabled = true;
    }
}
