using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 initialClickPosition;
    private HingeJoint2D hingeJoint;

    private void Start()
    {
        hingeJoint = gameObject.GetComponent<HingeJoint2D>();
        hingeJoint.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Collider2D playerCollider = GetComponent<Collider2D>();
            if (playerCollider.OverlapPoint(mousePosition))
            {
                if (!isDragging) // Chỉ thực hiện khi đang không kéo
                {
                    isDragging = true;
                    initialClickPosition = mousePosition; // Update initialClickPosition here
                    hingeJoint.anchor = transform.InverseTransformPoint(mousePosition);
                    
                    hingeJoint.enabled = true;
                    //hingeJoint.anchor = mousePosition;
                    hingeJoint.connectedAnchor = transform.InverseTransformPoint(mousePosition);


                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            hingeJoint.enabled = false;
        }

        if (isDragging)
        {
            
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector2 anchorDelta = (Vector2)mousePosition - initialClickPosition;
            hingeJoint.connectedAnchor = anchorDelta;
        }
    }
}
