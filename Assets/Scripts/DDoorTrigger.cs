using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class DDoorTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject doorLeft;

    [SerializeField]
    private GameObject doorRight;
    [SerializeField]
    private bool doorOpen = false;

    //new Vector Positions
    [SerializeField]
    private Vector3 doorLeftOpenPos;

    [SerializeField]
    private Vector3 doorRightOpenPos;

    private Vector3 doorLeftOpenPosInternal;
    private Vector3 doorRightOpenPosInternal;

    private Vector3 doorLeftClosedPos;
    private Vector3 doorRightClosedPos;
   
    //Positions
    private void Start()
    {
        doorLeftClosedPos = doorLeft.transform.position;
        doorRightClosedPos = doorRight.transform.position;

        doorLeftOpenPosInternal = doorLeftClosedPos + doorLeftOpenPos;
        doorRightOpenPosInternal = doorRightClosedPos + doorRightOpenPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "CannonBall")
        {
            return;
        }
        doorOpen = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CannonBall")
        {
            return;
        }

        doorOpen = false;
    }

    private void Update()
    {
        if (doorOpen)
        {
          doorLeft.gameObject.transform.position = Vector3.Lerp(
                doorLeft.gameObject.transform.position,
                doorLeftOpenPosInternal,
                Time.deltaTime);

            doorRight.gameObject.transform.position = Vector3.Lerp(
                doorRight.gameObject.transform.position,
                doorRightOpenPosInternal,
                Time.deltaTime);
        }
        else
        {

        }
    }
       
       
}
