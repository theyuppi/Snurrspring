﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpPower = 0.01f;
    private SpriteRenderer sb;
    private Vector3 _offset;

    private bool isGrounded = true;

    public Vector3 Offset
    {
        get { return _offset; }
    }

    private void Start()
    {
        sb = this.GetComponent<SpriteRenderer>();
        _offset = Vector3.zero;
    }

    public float gravity = -10;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (isGrounded)
            {
                this.isGrounded = false;
                sb.color = Random.ColorHSV();
                StartCoroutine(Jump(this.transform.up));
            }
        }
    }

    IEnumerator Jump(Vector2 jumpDirection)
    {
        float speed = this.jumpPower;
        float height = 0;
        while (height >= 0)
        {
            yield return new WaitForEndOfFrame();

            speed += gravity * Time.deltaTime;
            height += Time.deltaTime * speed;

            _offset = jumpDirection * height;
            yield return new WaitForEndOfFrame();
        }
        _offset = jumpDirection * 0;
        this.isGrounded = true;
    }
}
