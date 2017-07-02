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
        //Change the relic bool from true to false if its true or from false to true if its false
        //BL_HasRelic = (BL_HasRelic) ? false : true;
        BL_HasRelic = !BL_HasRelic;
        transform.GetComponent<ShrineAura>().RelicSwitch();

        if (BL_HasRelic)
        {
            GameObject.Find("AudioManager").GetComponent<JL_AudioManager>().PlaySound("PlaceRelicSuccess");
        }


        //send a message to both of the ghosts that tells them either to get angery or to not get angery depending on if you have placed the relic or not IDK maybe they are just having a bad day leave them2 a lone stop judgING.
        Ghost1.SendMessage("Change");
        Ghost2.SendMessage("Change");
    }
}
