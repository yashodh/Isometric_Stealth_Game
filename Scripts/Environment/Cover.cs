using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    public bool WithinThisCover { get; private set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            WithinThisCover = true;
            Player.Instance.Cover = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && Player.Instance.Cover == this)
        {
            WithinThisCover = false;
            Player.Instance.Cover = null;
        }
    }
}
