using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour
{
    [SerializeField]
    int stageNo;
    [SerializeField]
    GameObject LevelText;
    [SerializeField]
    GameObject LockImage;
    // Start is called before the first frame update
    void Start()
    {
        if (stageNo.ToString().Length == 1)
            LevelText.GetComponent<TextMeshProUGUI>().text = "0" + stageNo.ToString();
        else
            LevelText.GetComponent<TextMeshProUGUI>().text = stageNo.ToString();
        if (PlayerPrefs.GetInt("LevelUnlocked") >= stageNo)
        {
            LevelText.SetActive(true);
            LockImage.SetActive(false);
        }
        else
        {
            LevelText.SetActive(false);
            LockImage.SetActive(true);
        }

        GetComponent<Button>().onClick.AddListener(() => GameManager.gameManager.StageButtonClicked(stageNo));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
