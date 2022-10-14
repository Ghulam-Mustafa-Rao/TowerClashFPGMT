using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITower : MonoBehaviour
{
    [SerializeField]
    TowerController towerController;
    [SerializeField]
    float timePassed = 0;
    [SerializeField]
    float startAttackingAfter = 0;

    bool startAttack = false;
    float makePathCounter = 0;
    List<GameObject> towersThatCanBeReached;
    // Start is called before the first frame update
    void Start()
    {
        towerController = GetComponent<TowerController>();
        StartCoroutine(FillTowersThatCanBeReachedList());
    }

    private void FixedUpdate()
    {
        timePassed += Time.fixedDeltaTime;
        if (timePassed > startAttackingAfter)
        {
            startAttack = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        makePathCounter += Time.deltaTime;
        if (startAttack && makePathCounter > 5 && towerController.maxPaths > towerController.awayPaths)
        {
            towerController.DrawPath(towersThatCanBeReached[Random.Range(0, towersThatCanBeReached.Count)]);
            makePathCounter = 0;
        }
    }

    IEnumerator FillTowersThatCanBeReachedList()
    {
        yield return new WaitForSeconds(2);
        towersThatCanBeReached = new List<GameObject>();
        foreach (var item in towerController.stageManager.towers)
        {
            if (item.gameObject != this.gameObject)
            {
                Vector3 direction = (item.transform.position - transform.position).normalized;
                //Raycast 
                if (Physics.Raycast(transform.position, direction, out towerController.hit))
                {
                    if (towerController.hit.transform.gameObject.CompareTag("Tower"))
                    {
                        towersThatCanBeReached.Add(towerController.hit.transform.gameObject);
                    }
                }
            }
        }
    }
}
