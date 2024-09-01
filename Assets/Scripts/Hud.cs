using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Hud : MonoBehaviour
{
    private GameObject hud;
    private GameObject menu;
    private GameObject mainMenu;
    private GameObject credits;
    private GameObject gameOverScreen;
    private GameObject winScreen;
    private GameObject resumeButton;
    private TMP_Text skeletonsKilledLabel;

    private List<Transform> heartsGood = new List<Transform>();
    private List<Transform> heartsBad = new List<Transform>();
    bool endScreenShown = false;

    // Start is called before the first frame update
    void Start()
    {
        hud = transform.Find("Canvas/Hud").gameObject;
        menu = transform.Find("Canvas/Menu").gameObject;
        mainMenu = transform.Find("Canvas/Menu/MainMenu").gameObject;
        resumeButton = transform.Find("Canvas/Menu/MainMenu/ResumeButton").gameObject;
        credits = transform.Find("Canvas/Menu/Credits").gameObject;
        gameOverScreen = transform.Find("Canvas/Menu/GameOver").gameObject;
        winScreen = transform.Find("Canvas/Menu/WinScreen").gameObject;

        skeletonsKilledLabel = transform.Find("Canvas/Hud/SkeletonsKilledLabel").GetComponent<TMP_Text>();

        foreach (Transform child in transform.Find("Canvas/Hud/HeartsGood"))
        {
            heartsGood.Add(child);
        }
        foreach (Transform child in transform.Find("Canvas/Hud/HeartsBad"))
        {
            heartsBad.Add(child);
        }

        ShowMenu(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
        skeletonsKilledLabel.text = $"{GameState.Instance.skeletonsKilled}/{GameState.Instance.gravesNumber}";
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMenu(!menu.activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.F5)) GameState.Instance.gameResult = GameState.GameResult.WON;
        if (Input.GetKeyDown(KeyCode.F6)) GameState.Instance.gameResult = GameState.GameResult.LOST;

        if (GameState.Instance.gameResult == GameState.GameResult.WON)
        {
            ShowGameOver(true);
        }
        if (GameState.Instance.gameResult == GameState.GameResult.LOST)
        {
            ShowGameOver(false);
        }
    }

    public void ShowGameOver(bool won)
    {
        if (endScreenShown) return;
        endScreenShown = true;

        ShowMenu(true);
        mainMenu.SetActive(false);
        gameOverScreen.SetActive(!won);
        winScreen.SetActive(won);
        resumeButton.SetActive(false);
    }

    public void ShowMenu(bool show)
    {
        if (show)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            hud.SetActive(false);
            menu.SetActive(true);
            mainMenu.SetActive(true);
            credits.SetActive(false);
            gameOverScreen.SetActive(false);
            winScreen.SetActive(false);
            GameState.Instance.isPaused = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            hud.SetActive(true);
            menu.SetActive(false);
            GameState.Instance.isPaused = false;
        }
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(0);
        ShowMenu(false);
    }

    public void ExitGame()
    {
        Debug.Log("Exit game.");
        Application.Quit();
    }

    public void UpdateHealth()
    {
        var playerHealth = GameState.Instance.playerHealth;
        for (int i = 0; i < heartsGood.Count; ++i)
        {
            bool activate = i < playerHealth;
            heartsGood[i].gameObject.SetActive(activate);
            heartsBad[i].gameObject.SetActive(!activate);
        }
    }
}
