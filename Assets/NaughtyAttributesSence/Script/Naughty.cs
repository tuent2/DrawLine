using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Naughty : MonoBehaviour
{
    [Dropdown("GetNames")]
    [SerializeField] private string name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button("Log")]
    private void DebugSomething()
    {
        Debug.Log("Something"); 
    }

    private List<string> GetNames()
    {
        return new List<string>() { "Tue", "Linh"};
    }
}
