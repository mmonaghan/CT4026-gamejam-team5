using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTarget : MonoBehaviour {
    [SerializeField]
    private GameObject m_linkedDoor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        // Debug.Log("Trigger Entered");
        if (other.gameObject.CompareTag("CannonBall")) {
            //do stuff
            m_linkedDoor.GetComponent<DoorController>().Open();
        }
    }
}
