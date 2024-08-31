using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    private GameObject hud;
    private GameObject menu;
    private GameObject mainMenu;
    private GameObject credits;
    private List<Transform> heartsGood = new List<Transform>();
    private List<Transform> heartsBad = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        hud = transform.Find("Canvas/Hud").gameObject;
        menu = transform.Find("Canvas/Menu").gameObject;
        mainMenu = transform.Find("Canvas/Menu/MainMenu").gameObject;
        credits = transform.Find("Canvas/Menu/Credits").gameObject;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMenu(!menu.activeInHierarchy);
        }
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
