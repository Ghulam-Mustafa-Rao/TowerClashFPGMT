using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public List<GameObject> towers;
    public List<Material> playerMaterials;
    public bool gameStart = false;
    public List<GameObject> paths;
    public TextMeshProUGUI levelNo;
    public GameObject startButton;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;
    public GameObject[] stages;
    public AudioClip towerWin;
    public AudioClip towerLose;
    public AudioSource audioSource;
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        levelNo.text = "Level No : " + (PlayerPrefs.GetInt("CurrentLevel") + 1);
        LoadStage();
    }

    void LoadStage()
    {
        stages[PlayerPrefs.GetInt("CurrentLevel")].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (gameStart)
        {
            Time.timeScale = 1;
            CheckIfPlayerHasWonOrLost();
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    void CheckIfPlayerHasWonOrLost()
    {
        int playerTowersCount = 0;
        foreach (var item in towers)
        {
            if (item.gameObject.GetComponent<TowerController>().towerOwnerID == 0)
            {
                playerTowersCount++;
            }
        }

        if (playerTowersCount == 0)
        {
            //Player lose
            gameStart = false;
            losePanel.SetActive(true);
        }
        else if (playerTowersCount == towers.Count)
        {
            //Player Won
            gameStart = false;

            if (PlayerPrefs.GetInt("LevelUnlocked") == PlayerPrefs.GetInt("CurrentLevel") + 1)
            {
                PlayerPrefs.SetInt("LevelUnlocked", PlayerPrefs.GetInt("LevelUnlocked") + 1);
            }


            winPanel.SetActive(true);
        }
    }

    public void StartGame()
    {
        startButton.SetActive(false);
        gameStart = true;
    }

    public void NextLevel()
    {
        if (PlayerPrefs.GetInt("CurrentLevel") < stages.Length - 1)
        {
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        gameStart = false;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        gameStart = true;
    }

    public void BackToHome()
    {
        SceneManager.LoadScene(0);
    }
}
