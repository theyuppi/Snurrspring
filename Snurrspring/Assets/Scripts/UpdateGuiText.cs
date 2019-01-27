using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UpdateGuiText : MonoBehaviour
{
    private BasicDudeMove player;
    private Text text;

    private void Awake()
    {
        GetPlayer();
        text = this.GetComponent<Text>();
    }

    private void GetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<BasicDudeMove>();
    }

    private void FixedUpdate()
    {
        if (player == null)
            GetPlayer();

        if (player != null)
            text.text = string.Format("{0}% complete!", (player.TotalPercentageComplete * 100).ToString("0.0"));
    }
}
