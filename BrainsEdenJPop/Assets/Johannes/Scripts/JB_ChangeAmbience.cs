using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JB_ChangeAmbience : MonoBehaviour
{
    public string SwitchGroupName;

    public string SwitchStateName;

    private GameObject PC;

    private GameObject GO_AudioManager;

    void Start()
    {
        PC = GameObject.Find("PC");

        GO_AudioManager = GameObject.Find("AudioManager");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == PC)
        {
            AkSoundEngine.SetSwitch(SwitchGroupName, SwitchStateName, GO_AudioManager);
        }
    }
}
