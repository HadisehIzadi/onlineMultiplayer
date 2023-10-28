using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    public Vector3 direction = Vector3.right;

    public float speed = 2f;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }
}
