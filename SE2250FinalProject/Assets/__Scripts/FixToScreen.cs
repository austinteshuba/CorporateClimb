﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FixToScreen : MonoBehaviour
{
    public Vector2 screenPosition = new Vector2(0, 0);
    private Renderer renderer;

    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }
    void Update()
    {
        Vector3 tempScreenPosition = screenPosition;
        tempScreenPosition.z = -Camera.main.transform.position.z;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(tempScreenPosition);
        worldPosition.x -= renderer.bounds.size.x * tempScreenPosition.x / Screen.width;
        worldPosition.y += renderer.bounds.size.y * (1 - tempScreenPosition.y / Screen.height);
        transform.position = worldPosition;
    }
}
