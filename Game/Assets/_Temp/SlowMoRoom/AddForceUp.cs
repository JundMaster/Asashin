using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceUp : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 500, 0));
    }
}
