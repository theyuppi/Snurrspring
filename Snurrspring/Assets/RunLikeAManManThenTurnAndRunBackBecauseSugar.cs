using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunLikeAManManThenTurnAndRunBackBecauseSugar : MonoBehaviour
{
    int dir = 1;

    void FixedUpdate()
    {
        var pos = this.transform.position;
        pos.x += ((0.5f) * dir);
        this.transform.position = pos;

        if (pos.x > 9.63f || pos.x < -9.38f)
        {
            dir *= -1;
            this.GetComponent<SpriteRenderer>().flipX = !this.GetComponent<SpriteRenderer>().flipX;
        }
    }
}
