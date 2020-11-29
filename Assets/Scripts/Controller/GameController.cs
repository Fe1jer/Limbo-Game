using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public Elevator startElevator;
    public Elevator finishElevator;
    public GameSetting control;
    public Fader retryFader;
    public bool isOpened; //Открыто ли меню
    public bool isStart; //Старт или конец уровня

    void Start()
    {
        Time.timeScale = 1f; 
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            StartCoroutine(StartRoutine());
            control.GetComponent<Canvas>().enabled = false;
            control.gameObject.SetActive(true);
        }
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            StartCoroutine(StartGame());
        }
        isOpened = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Menu")
        {
            ShowHideMenu();
            control.BackInMenu();
        }
    }

    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        if (!isStart)
        {
            player.GetComponent<PlayerControl>().enabled = !isOpened;
            player.GetComponent<CharacterAnimation>().enabled = !isOpened;
        }
        control.GetComponent<Canvas>().enabled = true;

        control.gameObject.SetActive(isOpened);
        if (isOpened)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator StartGame()
    {
        isStart = true;
        player.GetComponent<PlayerControl>().enabled = false; 
        player.GetComponent<CharacterAnimation>().enabled = false;
        yield return StartCoroutine(ElevatorMotion(startElevator));
        player.GetComponent<CharacterAnimation>().enabled = true;
        player.GetComponent<PlayerControl>().enabled = true;
        isStart = false;
    }

    public IEnumerator ElevatorMotion(Elevator Elevator)
    {
        Elevator.GetComponentInChildren<Turn>().enabled = true;
        yield return StartCoroutine(Elevator.Motion(player)); 
        Elevator.GetComponentInChildren<Turn>().enabled = false;
    }

    public IEnumerator SelectMap()
    {
        isStart = true;
        retryFader.gameObject.SetActive(true);
        player.GetComponent<PlayerControl>().enabled = false;
        yield return StartCoroutine(ElevatorMotion(finishElevator));
        yield return StartCoroutine(retryFader.Fade(true));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        isStart = false;
    }

    public IEnumerator StartRoutine()
    {
        retryFader.gameObject.SetActive(true);
        yield return StartCoroutine(retryFader.Fade(false)); 
        retryFader.gameObject.SetActive(false);
    }

    public void LoseGame()
    {
        Time.timeScale = 0.4f;

        StartCoroutine(LoseRoutine());
        control.gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void NewGameLoadScenceLivel1()
    {
        SceneManager.LoadSceneAsync("Training");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public IEnumerator LoseRoutine()
    {
        Time.timeScale = 0.4f;
        retryFader.gameObject.SetActive(true);
        yield return StartCoroutine(retryFader.Fade(true));
        retryFader.button.gameObject.SetActive(true);
    }
}