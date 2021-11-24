using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrin : MonoBehaviour
{

    public float hoverForce = 12f;

    // Start is called before the first frame update
     void OnTriggerStay(Collider other)
    {

        other.GetComponent<Rigidbody>().AddForce(Vector3.up * hoverForce, ForceMode.Acceleration);
    }
}
