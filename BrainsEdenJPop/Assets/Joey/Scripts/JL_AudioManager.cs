using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JL_AudioManager : MonoBehaviour
{
    public float FL_GhostDistance;

    public bool BL_GhostPlaying = false;

    public float FL_Cooldown;
    public float FL_NextStep;

    public bool BL_Stepping;

    private List<GameObject> LS_GO_Spikes;
    private int IN_SoundCount;

    private GameObject GO_PC;

    // Use this for initialization
    void Start()
    {
        GO_PC = GameObject.Find("PC");

        //foreach (GameObject Spike in GameObject.FindGameObjectsWithTag("Spike"))
        //{
        //    LS_GO_Spikes.Add(Spike);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //If I'm walking check when to play a footstep
        if (BL_Stepping)
        {
            if (FL_NextStep <= Time.time)
            {
                AkSoundEngine.PostEvent("FootstepsFast", gameObject);
                FL_NextStep = Time.time + FL_Cooldown;
            }
        }
    }

    void LateUpdate()
    {
        //If there are no ghosts chasing
        if (GameObject.Find("LevelManager").GetComponent<JC_LevelManager>().IN_ChasingGhosts == 0 || FL_GhostDistance > 5)
        {
            if (BL_GhostPlaying)
            {
                AkSoundEngine.PostEvent("GhostSoundStop", gameObject);
            }
            BL_GhostPlaying = false;
        }
        //If there are ghosts chasing
        else
        {
            if (!BL_GhostPlaying)
            {
                AkSoundEngine.PostEvent("GhostSound", gameObject);
            }
            BL_GhostPlaying = true;

            AkSoundEngine.SetRTPCValue("GhostDistance", FL_GhostDistance);
        }
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
            case "PlaceRelic":
                AkSoundEngine.PostEvent("PlaceRelic", gameObject);
                break;
            case "PlaceRelicSuccess":
                AkSoundEngine.PostEvent("PlaceRelicSuccess", gameObject);
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

        FL_GhostDistance = vDistance;
    }

    public void SwitchFootsteps(string State)
    {
        switch (State)
        {
            case "Fast":
                BL_Stepping = true;
                FL_Cooldown = 0.5f;
                break;
            case "Slow":
                BL_Stepping = true;
                FL_Cooldown = 0.75f;
                break;
            case "Stop":
                BL_Stepping = false;
                break;
        }
    }

    public void Snickt(GameObject vSpike)
    {
        //Play a snickt sound, but limit it to 3 per activation.
        if (IN_SoundCount < 3)
        {
            if (Vector3.Distance(GO_PC.transform.position, vSpike.transform.position) < 10f)
            {
                AkSoundEngine.PostEvent("Snickt", gameObject);
                IN_SoundCount++;
                Debug.Log("Spike at: " + vSpike.transform.position.ToString() + "Making a sound");
                Destroy(vSpike);
                //Invoke("ClearSnickt", 1);
            }
        }
    }
    private void ClearSnickt()
    {
        IN_SoundCount = 0;
    }
}
