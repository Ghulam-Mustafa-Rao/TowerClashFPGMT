using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum TowerStatus
{
    neutral,
    ai,
    player
}
public class TowerParent : MonoBehaviour
{

    public int towerID;
    public int towerOwnerID;
    public int maxArmySize = 30;
    public int maxPaths = 1;
    public int awayPaths;
    public int armySize;

    [SerializeField]
    TextMeshProUGUI armySizeText;
    [SerializeField]
    GameObject[] availablePathsIndicators;
    [SerializeField]
    GameObject pathHolder;
    [SerializeField]
    GameObject armyPrefab;
    


    public StageManager stageManager;
    public RaycastHit hit;
    public GameObject lastTarget;
    public float armySizeCounter = 0;
    public TowerStatus towerStatus;
    public ParticleSystem winEffect;
    public GameObject canvas;

    private void Awake()
    {
        
    }
    public void Initialise()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ArmyCheck()
    {
        if (armySizeCounter > 1f)
        {
            armySizeCounter = 0;
            if (awayPaths == 0)
            {
                armySize++;
                if (armySize > maxArmySize)
                {
                    armySize = maxArmySize;
                }
            }
        }

        if (armySize > 19)
        {
            maxPaths = 3;
        }
        else if (armySize > 9)
        {
            maxPaths = 2;
        }
        else if (armySize < 9)
        {
            maxPaths = 1;
        }

        if (armySize < maxArmySize)
            armySizeText.text = armySize.ToString();
        else
            armySizeText.text = "MAX";
    }

    public bool DrawPath(GameObject lastTarget)
    {
        if (lastTarget != null)
        {
            foreach (var item in stageManager.paths)
            {
                if (item.gameObject.name == towerID + "" + lastTarget.GetComponent<TowerParent>().towerID)
                {
                    return false;
                }
                if (towerOwnerID == lastTarget.GetComponent<TowerParent>().towerOwnerID && item.gameObject.name == lastTarget.GetComponent<TowerParent>().towerID + "" + towerID)
                {
                    item.gameObject.GetComponent<Path>().DestroyPath();
                    break;
                }
            }
            if (awayPaths < maxPaths)
            {
                GameObject pathObject = Instantiate(pathHolder, transform.position, Quaternion.identity);
                LineRenderer lR = pathObject.GetComponent<LineRenderer>();

                lR.positionCount = 2;
                lR.SetPosition(0, transform.position);
                lR.SetPosition(1, lastTarget.transform.position);

                Path path = pathObject.GetComponent<Path>();
                path.startPoint = this.gameObject;
                path.endPoint = lastTarget;
                path.name = towerID + "" + lastTarget.GetComponent<TowerParent>().towerID;
                path.pathName = towerID + "" + lastTarget.GetComponent<TowerParent>().towerID;
                path.armyID = towerOwnerID;
                if (towerOwnerID == 0)
                {
                    path.isPlayersPath = true;
                    //path.GenerateCollider(lR);
                }
                stageManager.paths.Add(path.gameObject);
                awayPaths++;
                return true;
            }
        }
        return false;
    }

    public void SetPathsIndicator()
    {
        foreach (var item in availablePathsIndicators)
        {
            item.SetActive(false);
            item.GetComponent<Image>().color = Color.white;
        }

        for (int i = 0; i < maxPaths; i++)
        {
            availablePathsIndicators[i].SetActive(true);
        }

        for (int i = 0; i < awayPaths; i++)
        {
            availablePathsIndicators[i].GetComponent<Image>().color = Color.black;
        }
    }
       
}
