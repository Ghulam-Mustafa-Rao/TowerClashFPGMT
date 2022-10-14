using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public GameObject levelSelection;
    public AudioSource swordFightAudioSource;
    public AudioClip swordFightClip;
    public GameObject playButton;
    public GameObject infoButton;
    public GameObject infoPanel;
    public GameObject levelSelectionCloseButton;
    public GameObject infoCloseButton;
    public GameObject mainPanel;
    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        if (PlayerPrefs.GetInt("LevelUnlocked") == 0)
        {
            PlayerPrefs.SetInt("LevelUnlocked", 1);
        }

        DontDestroyOnLoad(this.gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlaySwordFightSound());
    }

    // Update is called once per frame
    void Update()
    {
        if ((levelSelection == null || playButton == null || infoPanel == null || infoButton == null || mainPanel == null) && SceneManager.GetActiveScene().buildIndex == 0)
        {
            InitialiseAllUI();
        }
    }

    private void FixedUpdate()
    {

    }

    public void StageButtonClicked(int levelNo)
    {
        if (PlayerPrefs.GetInt("LevelUnlocked") >= levelNo)
        {
            PlayerPrefs.SetInt("CurrentLevel", levelNo - 1);
            SceneManager.LoadScene("Stage");
        }
    }

    public void PlayGame()
    {
        mainPanel.SetActive(false);
        levelSelection.SetActive(true);
    }

    IEnumerator PlaySwordFightSound()
    {
        while (true)
        {
            swordFightAudioSource.PlayOneShot(swordFightClip);
            yield return new WaitForSeconds(15f);
        }
    }

    public void CloseLevelSelection()
    {
        mainPanel.SetActive(true);
        levelSelection.SetActive(false);
    }

    public void OpenInfoPanel()
    {
        infoPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void CloseInfoPanel()
    {
        mainPanel.SetActive(true);
        infoPanel.SetActive(false);
    }

    void InitialiseAllUI()
    {
        levelSelection = GameObject.Find("LevelSelectionPanel");
        mainPanel = GameObject.Find("MainPanel");
        playButton = GameObject.Find("PlayButton");
        levelSelectionCloseButton = GameObject.Find("LevelSelectionCloseButton");
        infoCloseButton = GameObject.Find("InfoCloseButton");
        infoPanel = GameObject.Find("InfoPanel");
        infoButton = GameObject.Find("InfoButton");
        playButton.GetComponent<Button>().onClick.AddListener(PlayGame);
        infoCloseButton.GetComponent<Button>().onClick.AddListener(CloseInfoPanel);
        levelSelectionCloseButton.GetComponent<Button>().onClick.AddListener(CloseLevelSelection);
        infoButton.GetComponent<Button>().onClick.AddListener(OpenInfoPanel);
        levelSelection.SetActive(false);
        infoPanel.SetActive(false);
    }
}
