using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * speed, _rigidbody.velocity.y, _joystick.Vertical);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
