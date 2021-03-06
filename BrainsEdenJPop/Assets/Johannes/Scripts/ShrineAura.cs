﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineAura : MonoBehaviour
{
    public ParticleSystem Aura;

    public string EventName = "ShrineAura";

    public bool RelicPlaced;

    public float EffectCooldown = 3f;
    private float timer;

    void Update()
    {
        if (timer <= Time.time && RelicPlaced)
        {
            Aura.Play();
            AkSoundEngine.PostEvent(EventName, gameObject);
            timer = Time.time + EffectCooldown;
        }
    }

    public void RelicSwitch()
    {
        //RelicPlaced = (RelicPlaced) ? false : true;
        RelicPlaced = !RelicPlaced;
    }
}
