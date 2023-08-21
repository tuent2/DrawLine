using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        CreateBoxCollider();
    }

    private void CreateBoxCollider()
    {
        // Get the screen dimensions in world coordinates
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Camera.main.aspect;

        // Calculate the height and position of the collider
        float colliderHeight = screenHeight * 0.8f; // 70% of the screen height
        float colliderPosY = screenHeight * 0.5f - colliderHeight * 0.5f; // Top position minus half of collider height

        // Add a BoxCollider component to the current GameObject
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();

        // Set the size of the collider to match the calculated dimensions
        boxCollider.size = new Vector2(screenWidth, colliderHeight);

        // Position the collider at the calculated position
        transform.position = new Vector3(Camera.main.transform.position.x, colliderPosY, 0f);
    }
}
