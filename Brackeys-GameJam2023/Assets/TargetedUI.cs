using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetedUI : StaticInstances<TargetedUI>
{
    private Image targetImage;
    public bool calledInLastFrame;
    private Transform mainCamera;

    protected override void Awake()
    {
        base.Awake();
        targetImage = GetComponent<Image>();
        mainCamera = Camera.main.transform;
    }
    public void SetPosition(Vector3 worldPos)
    {
        calledInLastFrame = true;
        transform.position = worldPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!calledInLastFrame)
            targetImage.enabled = false;
        else
        {
            transform.LookAt(mainCamera);
            targetImage.enabled = true;
            calledInLastFrame = false;
        }
    }
}
