﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject focusObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(focusObject.transform.position.x, transform.position.y, -10);
    }
}
