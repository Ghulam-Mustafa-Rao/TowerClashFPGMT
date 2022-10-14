using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    public int ownerID;
    public bool alive = true;
    public GameObject target;
    public float speed;
    public string pathName;
    public float maxSpeed;
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alive)
        {
            if (other.CompareTag("Tower"))
            {
                if (other.gameObject == target)
                {
                    OnCollisionWithTower(other);

                    Destroy(this.gameObject);
                }
            }
            if (other.CompareTag("Army"))
            {
                //Destory the army it colldies and itself too if both are on same path but have diffrent owner
                if (ownerID != other.GetComponent<Army>().ownerID
                    && pathName[0] == other.GetComponent<Army>().pathName[1]
                    && pathName[1] == other.GetComponent<Army>().pathName[0])
                {
                    alive = false;
                    Destroy(this.gameObject);

                }

            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            transform.LookAt(target.transform);
            rigidbody.AddForce(transform.forward * Time.deltaTime * speed, ForceMode.Impulse);
            if (rigidbody.velocity.magnitude > maxSpeed)
            {
                rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);
            }
        }
    }

    private void OnCollisionWithTower(Collider other)
    {
        alive = false;
        TowerController towerController = other.GetComponent<TowerController>();
        //Decrease 1 unit of army of tower it collides with if Owner is diffrent
        if (towerController.armySize > 0 && towerController.towerOwnerID != ownerID)
        {
            towerController.armySize--;
        }
        //Increase 1 unit of army of tower it collides with if Owner is same
        else if (towerController.armySize < 1 && towerController.towerOwnerID != ownerID)
        {
            StageManager stageManager = GameObject.FindObjectOfType<StageManager>();
            if (ownerID == 0)
            {
                stageManager.audioSource.PlayOneShot(stageManager.towerWin);
            }
            if (towerController.towerOwnerID == 0)
            {
                stageManager.audioSource.PlayOneShot(stageManager.towerLose);
            }

            //Change Owner Of tower this army collides with
            towerController.towerOwnerID = ownerID;
            List<GameObject> pathsOfTheTowerThisArmyCollidesWith = new List<GameObject>();
            //Get all paths Of tower this army collides with
            foreach (var item in stageManager.paths)
            {
                if (item.gameObject.name[0].ToString() == towerController.towerID.ToString())
                {
                    pathsOfTheTowerThisArmyCollidesWith.Add(item);
                }
            }

            //Destroy all paths Of tower this army collides with
            foreach (var item in pathsOfTheTowerThisArmyCollidesWith)
            {
                item.gameObject.GetComponent<Path>().DestroyPath();
            }
            towerController.SetTowerStatus();
        }

        //Check if tower owner and army owner is same
        if (towerController.towerOwnerID == ownerID)
        {
            //Add 1 unit army in tower this army collides with
            if (towerController.armySize < towerController.maxArmySize)
            {
                towerController.armySize++;
            }
            else if (towerController.awayPaths > 0)
            {
                //If the tower has max army then Spwan 1 unit of army on every path that leaves this tower
                StageManager stageManager = GameObject.FindObjectOfType<StageManager>();
                foreach (var item in stageManager.paths)
                {
                    if (item.gameObject.name[0].ToString() == towerController.towerID.ToString())
                    {
                        item.gameObject.GetComponent<Path>().SpwanArmy();
                    }
                }
            }
        }
    }
}
