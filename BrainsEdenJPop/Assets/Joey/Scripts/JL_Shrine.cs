using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JL_Shrine : MonoBehaviour
{
    public bool BL_HasRelic;

    public GameObject GO_SwitchObject;

    public GameObject Ghost1;
    public GameObject Ghost2;

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
        Ghost1.SendMessage("Change");
        Ghost2.SendMessage("Change");
    }
}
