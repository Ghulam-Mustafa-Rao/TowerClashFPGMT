using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PllayerTower : MonoBehaviour
{
    bool mouseDown = false;

    [SerializeField]
    TowerController towerController;
    LineRenderer lineRenderer;
    [SerializeField]
    Material[] linerendererMaterials;
    [SerializeField]
    GameObject fingerPositionRing;

    // Start is called before the first frame update
    void Start()
    {
        towerController = GetComponent<TowerController>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        fingerPositionRing.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        if (mouseDown)
        {
            lineRenderer.enabled = true;
            Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPosition = transform.position;
            if (Physics.Raycast(ray,out RaycastHit raycastHit))
            {
                worldPosition = raycastHit.point;
            }

            worldPosition.y = 0;
            //worldPosition.z = 10;
            float distance = Vector3.Distance(transform.position, worldPosition);
            //Debug.LogError(distance);
            Vector3 direction = (worldPosition - transform.position).normalized;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, worldPosition);
            if (Physics.Raycast(transform.position, direction, out towerController.hit, distance))
            {
                lineRenderer.SetPosition(1, towerController.hit.point);
                fingerPositionRing.transform.position = towerController.hit.point;
                fingerPositionRing.SetActive(true);
                //Debug.LogError(hit.transform.gameObject.tag);
                if (towerController.hit.transform.gameObject.CompareTag("Tower"))
                {
                    towerController.lastTarget = towerController.hit.transform.gameObject;

                    lineRenderer.material = linerendererMaterials[1];
                }
                else
                {
                    towerController.lastTarget = null;

                    lineRenderer.material = linerendererMaterials[0];
                }
            }
            else
            {
                //Debug.DrawLine(transform.position, worldPosition, Color.white);

                lineRenderer.material = linerendererMaterials[0];
                towerController.lastTarget = null;
                fingerPositionRing.transform.position = worldPosition;
                fingerPositionRing.SetActive(true);
            }
        }
        else
        {
            lineRenderer.enabled = false;
            fingerPositionRing.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        mouseDown = true;
    }

    private void OnMouseUp()
    {
        towerController.DrawPath(towerController.lastTarget);
        mouseDown = false;
    }


}
