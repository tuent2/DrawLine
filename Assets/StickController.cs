using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    private bool isDragging = false;
    HingeJoint2D hingeJoint;
    private void Start()
    {
         hingeJoint = gameObject.GetComponent<HingeJoint2D>();
        hingeJoint.enabled = false;
    }

    private void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            // Chuyển đổi tọa độ chuột/chạm sang không gian game
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Đảm bảo z = 0 để tránh vấn đề về chiều cao

            // Kiểm tra va chạm với GameObject "player"
            Collider2D playerCollider = GetComponent<Collider2D>();
            if (playerCollider.OverlapPoint(mousePosition))
            {
                isDragging = true;
                // Thiết lập Connected Anchor của HingeJoint2D thành vị trí chạm
                hingeJoint.anchor = mousePosition;
                hingeJoint.connectedAnchor = mousePosition;
               
            }
        }

        // Kiểm tra sự kiện thả chuột/ra khỏi màn hình
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            //hingeJoint.anchor = gameObject.transform.position;
            //hingeJoint.connectedAnchor = gameObject.transform.position;
            hingeJoint.enabled = false;
            
        }

       
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
           
                if (hingeJoint != null)
                {
                //Vector3 localTouchPosition = gameObject.transform.InverseTransformPoint(mousePosition);
                //hingeJoint.connectedAnchor = localTouchPosition;

                //hingeJoint.enabled = true;
                hingeJoint.enabled = true;

            }
        }
    }
}
