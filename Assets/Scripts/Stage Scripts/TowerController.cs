using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : TowerParent
{
    [SerializeField]
    PllayerTower pllayerTower;
    [SerializeField]
    AITower aITower;
    private void Awake()
    {
        pllayerTower = GetComponent<PllayerTower>();
        aITower = GetComponent<AITower>();
    }
    // Start is called before the first frame update
    void Start()
    {
        stageManager = GameObject.FindObjectOfType<StageManager>();
        stageManager.towers.Add(this.gameObject);
        towerID = stageManager.towers.FindIndex(a => a == this.gameObject);
        SetTowerStatus();
    }

    // Update is called once per frame
    void Update()
    {
        if (towerStatus != TowerStatus.neutral)
        {
            armySizeCounter += Time.deltaTime;
        }
        else
        {
            armySizeCounter = 0;
        }
        ArmyCheck();
        SetPathsIndicator();
    }

    public void SetTowerStatus()
    {
        //Set Tower Status
        if (towerOwnerID == 0)
        {
            towerStatus = TowerStatus.player;
            winEffect.gameObject.SetActive(true);
            winEffect.Play();
        }
        else if (towerOwnerID == stageManager.playerMaterials.Count - 1)
        {
            towerStatus = TowerStatus.neutral;
        }
        else
        {
            towerStatus = TowerStatus.ai;
        }

        //Active player or ai script
        switch (towerStatus)
        {
            case TowerStatus.ai:
                {
                    pllayerTower.enabled = false;
                    aITower.enabled = true;
                }
                break;
            case TowerStatus.player:
                {
                    pllayerTower.enabled = true;
                    aITower.enabled = false;
                }
                break;
            default:
                {
                    pllayerTower.enabled = false;
                    aITower.enabled = false;
                }
                break;
        }

        //Chnage material
        GetComponent<Renderer>().material = stageManager.playerMaterials[towerOwnerID];
    }
}
