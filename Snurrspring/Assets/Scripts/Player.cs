using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool _isGrounded = true;
    public bool _jumping = false;
    public float jumpTime = 2.0f;
    private float jumpedTime = 0;
    public float jumpPowah = 0.01f;
    private SpriteRenderer sb;
    private Vector3 _offset;
    [SerializeField]
    private float _jumpTimeStamp;

    public float JumpTimeStamp
    {
        get { return _jumpTimeStamp; }
    }

    public bool isGrounded
    {
        get { return _isGrounded; }
        set
        {
            this._isGrounded = value;
            if (value)
                Jumping = false;
        }
    }

    public bool Jumping
    {
        get { return _jumping; }
        set
        {
            _jumping = value;
            jumpedTime = 0;
            _offset = Vector3.zero;
        }
    }

    public Vector3 Offset
    {
        get { return _offset; }
    }

    private void Start()
    {
        sb = this.GetComponent<SpriteRenderer>();
        _offset = Vector3.zero;
    }



    private void Update()
    {
        if (Input.anyKeyDown)
        {
            sb.color = Random.ColorHSV();

            if (isGrounded && !Jumping)
            {
                isGrounded = false;
                _jumping = true;

                _jumpTimeStamp = Time.time;
                StartCoroutine(Jump(this.transform.up));
            }
        }
        Debug.Log(Offset);
    }

    IEnumerator Jump(Vector2 jumpDirection)
    {
        while (Jumping)
        {
            while (Time.time - _jumpTimeStamp  < jumpTime)
            {
                jumpedTime += Time.deltaTime;
                if (Jumping)
                _offset += (this.transform.up * jumpPowah);
                yield return new WaitForEndOfFrame();
            }

            jumpedTime += Time.deltaTime;
            if (Jumping)
                _offset += (this.transform.up * jumpPowah);

            //if (isGrounded)
            //    jumping = false;
        }
        yield return new WaitForEndOfFrame();
    }
}
