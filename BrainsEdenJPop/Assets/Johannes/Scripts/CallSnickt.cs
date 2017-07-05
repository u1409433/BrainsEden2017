using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallSnickt : MonoBehaviour
{
    float timer;
    public float cooldown = 4;

    private JL_AudioManager SC_AudioManager;

    // Use this for initialization
    void Start()
    {
        SC_AudioManager = GameObject.Find("AudioManager").GetComponent<JL_AudioManager>();

        timer = gameObject.GetComponent<JL_Spikes>().FL_SwitchTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= Time.time)
        {
            if (SC_AudioManager.CanSnickt(gameObject))
            {
                AkSoundEngine.PostEvent("Snickt", gameObject);
            }
            timer = Time.time + cooldown;
        }
    }
}
