
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

    private JL_AudioManager SC_AudioManager;

    // Use this for initialization
    void Start()
    {
        SC_AudioManager = GameObject.Find("AudioManager").GetComponent<JL_AudioManager>();

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
                //check distance to PC
                SC_AudioManager.Snickt(gameObject);
            }
            else gameObject.transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        }
    }
}
