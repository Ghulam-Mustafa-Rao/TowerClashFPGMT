using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public GameObject startPoint;
    public GameObject endPoint;
    public GameObject army;
    public int armyID;
    public string pathName;
    public bool isPlayersPath = false;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpwanArmyCO());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateCollider(LineRenderer line)
    {
        MeshCollider collider = GetComponent<MeshCollider>();

        if (collider == null)
        {
            collider = gameObject.AddComponent<MeshCollider>();
        }

        Mesh mesh = new Mesh();
        line.BakeMesh(mesh);
        collider.sharedMesh = mesh;
    }

    IEnumerator SpwanArmyCO()
    {
        while (true)
        {
            SpwanArmy();
            yield return new WaitForSeconds(1f);
        }
    }

    public void SpwanArmy()
    {
        GameObject armyObject = Instantiate(army, new Vector3(startPoint.transform.position.x, 1, startPoint.transform.position.z), Quaternion.identity);
        armyObject.GetComponent<Army>().ownerID = armyID;
        armyObject.GetComponent<Army>().target = endPoint;
        armyObject.GetComponent<Renderer>().material = startPoint.GetComponent<TowerController>().stageManager.playerMaterials[startPoint.GetComponent<TowerController>().towerOwnerID];
        armyObject.GetComponent<Army>().pathName = this.gameObject.name;
        armyObject.GetComponent<Army>().alive = true;
    }

    public void DestroyPath()
    {
        StageManager stageManager = GameObject.FindObjectOfType<StageManager>();
        foreach (var item in stageManager.towers)
        {
            if (item.gameObject.GetComponent<TowerController>().towerID.ToString() == pathName[0].ToString())
            {
                item.gameObject.GetComponent<TowerController>().awayPaths--;
                break;
            }
        }
        stageManager.paths.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

}
