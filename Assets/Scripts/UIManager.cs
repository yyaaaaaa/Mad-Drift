using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject GameContainer;
    public GameObject MainMenuContainer;
    public GameObject GarageContainer;
    public GameObject WinPopUpContainer;
    public GameObject LoseContaier;
    public GameObject ShopContainer;
    GameObject current;
    public static UIManager instance;
    public InterstitialAd ad;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        current = MainMenuContainer;
        Time.timeScale = 0f;
    }
    public void PlayGame()
    {
        GameManager.instance.ChangeScene();
        LevelManager.instance.LevelStart();
        current.SetActive(false);
        GameContainer.SetActive(true);
        current = GameContainer;
        ad.LoadAd();
        Time.timeScale = 1f;
    }
    public void Garage()
    {
        ad.LoadAd();
        current.SetActive(false);
        GarageContainer.SetActive(true);
        current = GarageContainer;
    }
    public void Shop()
    {
        current.SetActive(false);
        ShopContainer.SetActive(true);
        current = ShopContainer;
    }
    public void ReturnToMainMenu()
    {
        current.SetActive(false);
        MainMenuContainer.SetActive(true);
        current = MainMenuContainer;
    }
    public void WinLevel()
    {
        ad.ShowAd();
        current.SetActive(false);
        WinPopUpContainer.SetActive(true);
        current = WinPopUpContainer;
        Time.timeScale = 0f;
    }

    public void LoseLevel()
    {
        ad.ShowAd();
        current.SetActive(false);
        LoseContaier.SetActive(true);
        current = LoseContaier;
        Time.timeScale = 0f;
    }
    public void RestartLevel()
    {
        GameManager.instance.ChangeScene();
        LevelManager.instance.LevelStart();
        current.SetActive(false);
        GameContainer.SetActive(true);
        current = GameContainer;
        Time.timeScale = 1f;
    }
    public void NextLevel()
    {
        ad.LoadAd();
        GameManager.instance.ChangeScene();
        LevelManager.instance.LevelStart();
        current.SetActive(false);
        GameContainer.SetActive(true);
        current = GameContainer;
        Time.timeScale = 1f;
    }
}
