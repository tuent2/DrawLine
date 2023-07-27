using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    private Camera _Cam;
    [SerializeField] private Line _lineFrefab;
    private Line currentLine;

    public const float RESOLUTION = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        _Cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = _Cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            currentLine = Instantiate(_lineFrefab, mousePos, Quaternion.identity);
        }
        if (Input.GetMouseButton(0))
        {
            currentLine.SetPosition(mousePos);
        }

    }
}
