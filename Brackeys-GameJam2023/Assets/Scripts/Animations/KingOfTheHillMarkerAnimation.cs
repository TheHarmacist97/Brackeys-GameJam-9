using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingOfTheHillMarkerAnimation : MonoBehaviour
{
    Transform mainCam;
    private Image image;
    private void Awake()
    {
        mainCam = Camera.main.transform;
        image = GetComponent<Image>();
    }

    public void Update()
    {
        transform.position = transform.position + Vector3.up * Mathf.Sin(Time.time*2f)*0.03f;
        transform.LookAt(mainCam);
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z = -90f;
        transform.rotation = Quaternion.Euler(rot);
    }
    public void SetMarker(bool enabled)
    {
        if(image==null)
        {
            image = GetComponent<Image>();
        }
        image.enabled = enabled;
    }
}
