using System.Collections;
using System.Collections.Generic;
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
    private void Start()
    {
        current = MainMenuContainer;
        Time.timeScale = 0f;
    }
    public void PlayGame()
    {
        current.SetActive(false);
        GameContainer.SetActive(true);
        current = GameContainer;
        Time.timeScale = 1f;
    }
    public void Garage()
    {
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
        current.SetActive(false);
        WinPopUpContainer.SetActive(true);
        current= WinPopUpContainer;
        Time.timeScale = 0f;
    }

    public void LoseLevel()
    {
        current.SetActive(false);
        LoseContaier.SetActive(true);
        current = LoseContaier;
        Time.timeScale = 0f;
    }
    public void RestartLevel()
    {
        current.SetActive(false);
        GameContainer.SetActive(true);
        current = GameContainer;
        Time.timeScale = 1f;
    }
    public void NextLevel()
    {
        current.SetActive(false);
        GameContainer.SetActive(true);
        current = GameContainer;
        Time.timeScale = 1f;
    }
}
