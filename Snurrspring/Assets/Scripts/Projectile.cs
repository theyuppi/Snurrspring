using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float moveSpeed = 2.62f;
    public Vector3 dir = Vector3.zero;
    private Vector3 startPos;

    private void Start()
    {
        startPos = this.transform.position;
    }

    void Update()
    {
        this.transform.position += (dir * moveSpeed * Time.deltaTime);
        //this.transform.position += ((dir - startPos).normalized * moveSpeed);
    }
}
