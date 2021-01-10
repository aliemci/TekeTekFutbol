using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followObject;

    Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = this.transform.position - followObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = Vector3.Lerp(this.transform.position, followObject.transform.position + cameraOffset, Time.deltaTime);
        this.transform.position = cameraPosition;
    }
}
