
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JL_Spikes : MonoBehaviour
{
    public float FL_SwitchTime;
    public float FL_Cooldown;

    public bool BL_Up;

    public float startY;

    public ParticleSystem SmokePoof;

    // Use this for initialization
    void Start()
    {
        startY = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (FL_SwitchTime <= Time.time)
        {
            FL_SwitchTime = Time.time + FL_Cooldown;
            BL_Up = (BL_Up) ? false : true;
            if (BL_Up)
            {
                gameObject.transform.position = new Vector3(transform.position.x, startY + 1, transform.position.z);
                //SmokePoof.Play();
                //AkSoundEngine.PostEvent("Snickt", gameObject);
            }
            else gameObject.transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        }
    }
}
