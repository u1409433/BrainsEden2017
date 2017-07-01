using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JL_AudioManager : MonoBehaviour
{
    public float FL_GhostDistance;

    public bool BL_GhostPlaying = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        if (GameObject.Find("LevelManager").GetComponent<JC_LevelManager>().IN_ChasingGhosts == 0)
        {
            AkSoundEngine.PostEvent("GhostSoundStop", gameObject);
        }
        else BL_GhostPlaying = false;
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

    public void ReceiveGhostInfo(float vDistance)
    {
        if (!BL_GhostPlaying)
        {
            BL_GhostPlaying = true;
            AkSoundEngine.PostEvent("GhostSound", gameObject);
        }

        if (vDistance < FL_GhostDistance)
        {
            FL_GhostDistance = vDistance;
        }
    }
}
