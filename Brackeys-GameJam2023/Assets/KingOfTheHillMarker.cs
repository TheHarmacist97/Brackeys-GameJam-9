using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingOfTheHillMarker : MonoBehaviour
{
    Transform mainCam;
    private void Awake()
    {
        mainCam = Camera.main.transform;
    }

    public void Update()
    {
        transform.position = transform.position + Vector3.up * Mathf.Sin(Time.time*2f)*0.03f;
        transform.LookAt(mainCam);
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z = -90f;
        transform.rotation = Quaternion.Euler(rot);
    }
}
