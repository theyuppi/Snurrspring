using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    private float pullStrength = 10.0f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player != null)
        {
            var dir = (player.transform.position - this.transform.position).normalized;
            rb.velocity = dir * pullStrength;
        }
    }
}
