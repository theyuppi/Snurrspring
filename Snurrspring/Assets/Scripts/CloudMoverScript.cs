using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMoverScript : MonoBehaviour
{
    private Vector3 startPos;
    private float moveSpeedX;
    private float goalY;
    private float heightChangeY = 0.04f;

    private void Start()
    {
        this.startPos = transform.position;
        goalY = 0;
        moveSpeedX = UnityEngine.Random.Range(0.05f, 0.4f);
    }

    private void FixedUpdate()
    {
        MoveCloud();
    }

    private void MoveCloud()
    {
        this.transform.position = Vector2.Lerp(this.transform.position, GetNewPosition(), Time.fixedDeltaTime);
        if (this.transform.position.x >= 12)
            this.transform.position = new Vector3(this.transform.position.x * -1, startPos.y, this.transform.position.z);
    }

    private Vector3 GetNewPosition()
    {
        if (UnityEngine.Random.Range(0, 100) > 90)
            goalY += UnityEngine.Random.Range(-heightChangeY, heightChangeY);

        goalY = Mathf.Clamp(goalY, -heightChangeY, heightChangeY);

        var newPosition = new Vector3(this.transform.position.x + moveSpeedX, 
                                      this.transform.position.y + goalY, 
                                      this.transform.position.z);
                
        return newPosition;
    }
}