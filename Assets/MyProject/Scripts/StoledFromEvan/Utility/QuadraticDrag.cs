﻿using UnityEngine;
using System.Collections;

public class QuadraticDrag : MonoBehaviour
{

    public float drag = 1;
    new protected Rigidbody rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        rigidbody.AddForce(rigidbody.linearVelocity * rigidbody.linearVelocity.magnitude * -(drag * Time.fixedDeltaTime), ForceMode.VelocityChange);
    }
}
