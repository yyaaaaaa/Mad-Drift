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
    public List<string> coinsModels = new();
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

    private List<GameObject> spawnedCanisters = new();
    private List<GameObject> spawnedCoins = new();
    public GameObject popuptext;
    public Animator animatorTimer;
    public Animator animatorClock;

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
        timer = timeCap;
    }

        void Update()
    {
        if (timerIsRunning)
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
                slider.value = timer;
                spawnTimer += Time.deltaTime;
                reward =(int)timer;
                rewardText.text = reward.ToString();
                loseRewardText.text = "+" + (timeCap - reward).ToString();
                if (enemies.Count <= 0)
                {
                    SpawnEnemy();
                }

                if (spawnTimer >= spawnInterval)
                {
                    SpawnEnemy();
                    SpawnOneTimes(0);
                    spawnTimer = 0f; // Сбрасываем таймер
                }
                if (timer <= 10f)
                {
                    animatorClock.SetBool("10sec", true);
                    animatorTimer.SetBool("10sec", true);
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
        animatorClock.SetBool("10sec", false);
        animatorTimer.SetBool("10sec", false);
        int addReward = Random.Range(minReward, maxReward);
        UIManager.instance.WinLevel();
        int totalReward = addReward + ((int)timeCap - reward); 
        addRewardText.text = "+" + totalReward.ToString();
        GameManager.instance.AddMoney(totalReward);
        GameManager.instance.level += 1;
        level += 1;
        GameManager.instance.Save();
        LoadLevel(level);
        ClearSpawned();
    }

    public void LevelStart()
    {
        animatorClock.SetBool("10sec", false);
        animatorTimer.SetBool("10sec", false);
        player.SetActive(false);
        enemies.Clear();
        player.transform.position = Vector3.zero;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponentInChildren<HPController>().health = player.GetComponentInChildren<HPController>().maxHealth;
        player.SetActive(true);
        timerIsRunning = true;
        slider.maxValue = timeCap;
        timer = timeCap;
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
            enemy.GetComponentInChildren<HPController>().PopUpText = popuptext;
            enemies.Add(enemy);

        }
    }
    private void ClearSpawned()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
    void LoadLevel(int neededLevel)
    {
        if (neededLevel < levels.Count)
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

    public void SpawnOneTimes( int amount)
    {
        for (int i = 0; i <= amount; i++)
        {
            float minDistance = 50f; // Минимальное допустимое расстояние между игроком и объектом
            float maxDistance = 200f; // Максимальное расстояние от игрока, на котором объекты могут спауниться

            float randomCoinX, randomCanisterX, randomCoinZ, randomCanisterZ;
            Vector3 coinOffset, canisterOffset;
            int randomCoinIndex;

            // Генерация случайных координат для объектов
            do
            {
                randomCoinX = Random.Range(-maxDistance, maxDistance);
                randomCanisterX = Random.Range(-maxDistance, maxDistance);
                randomCoinZ = Random.Range(-maxDistance, maxDistance);
                randomCanisterZ = Random.Range(-maxDistance, maxDistance);

                coinOffset = new Vector3(randomCoinX, 3f, randomCoinZ);
                canisterOffset = new Vector3(randomCanisterX, 3f, randomCanisterZ);
            } while (Vector3.Distance(player.transform.position, player.transform.position + coinOffset) < minDistance
                  || Vector3.Distance(player.transform.position, player.transform.position + canisterOffset) < minDistance);

            // Случайно выбираем модель монеты из списка
            randomCoinIndex = Random.Range(0, coinsModels.Count);

            // Создаем монету и бочку в случайных позициях относительно игрока
            spawnedCoins.Add(ObjectPooler.Instance.SpawnFromPool(coinsModels[randomCoinIndex], player.transform.position + coinOffset, Quaternion.identity));
            spawnedCanisters.Add(ObjectPooler.Instance.SpawnFromPool("Canister", player.transform.position + canisterOffset , Quaternion.identity));        
        }
    }
}
