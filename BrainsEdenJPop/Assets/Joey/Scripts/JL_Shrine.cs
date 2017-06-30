using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JL_Shrine : MonoBehaviour
{
    public bool BL_HasRelic;

    public GameObject GO_SwitchObject;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BL_HasRelic) GO_SwitchObject.SetActive(false);
        else GO_SwitchObject.SetActive(true);
    }

    public void SwitchRelic()
    {
        BL_HasRelic = (BL_HasRelic) ? false : true;
    }
}
