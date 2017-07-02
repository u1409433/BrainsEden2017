using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallSnickt : MonoBehaviour
{
    float timer;
    public float cooldown = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= Time.time)
        {
            AkSoundEngine.PostEvent("Snickt", gameObject);
            timer = Time.time + cooldown;
        }
    }
}
