using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key01 : MonoBehaviour
{
    // detect if key intercects keyhole 01

    public GameObject doorKeyOne;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KeyHole01")
        {
            doorKeyOne.SetActive(false);
            Destroy(gameObject);
        }
    }
}
