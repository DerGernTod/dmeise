using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
