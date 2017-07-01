using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JL_AudioManager : MonoBehaviour
{
    public float FL_GhostDistance;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(string vSound)
    {
        switch (vSound)
        {
            case "PickupRelic":
                AkSoundEngine.PostEvent("PickupRelic", gameObject);
                break;
            case "PainLow":
                AkSoundEngine.PostEvent("PainLow", gameObject);
                break;
            case "ShrineAura":
                AkSoundEngine.PostEvent("ShrineAura", gameObject);
                break;
            default:
                break;
        }
    }
}
