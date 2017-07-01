using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JB_ChangeAmbience : MonoBehaviour
{
    public string SwitchGroupName;

    public string SwitchStateName;

    private GameObject PC;

    void Start()
    {
        PC = GameObject.Find("PC");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == PC)
        {
            AkSoundEngine.SetSwitch(SwitchGroupName, SwitchStateName, PC);
        }
    }
}
