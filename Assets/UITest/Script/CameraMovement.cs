using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 dragOrigin;

    //[SerializeField] private float zoomState, minCamsize,maxCamsize;
    [SerializeField]
    private SpriteRenderer mapRenderer;
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private void Awake()
    {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x /2; 
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x /2;
        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y /2 ;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y /2;
    }
    void Update()
    {
        PanCamera();
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 Difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position = ClampCamera(cam.transform.position + Difference);
        }
    }

    //public void ZoomIn()
    //{
    //    float newSize = cam.orthographicSize + zoomState;
    //    cam.orthographicSize = Mathf.Clamp(newSize, minCamsize, maxCamsize);
    //}

    //public void ZoomOut()
    //{
    //    float newSize = cam.orthographicSize - zoomState;
    //    cam.orthographicSize = Mathf.Clamp(newSize, minCamsize, maxCamsize);
    //}

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z); 
    }
}
