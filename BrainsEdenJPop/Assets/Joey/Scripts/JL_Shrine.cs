using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JL_Shrine : MonoBehaviour
{
    public bool BL_HasRelic;

    public GameObject GO_SwitchObject;

    public GameObject Ghost1;
    public GameObject Ghost2;
    public GameObject Ghost3;

    public int IN_ShrineNum;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (BL_HasRelic) GO_SwitchObject.SetActive(false);
        //else GO_SwitchObject.SetActive(true);

        //DebugJL();
    }

    private void DebugJL()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Ghost1.SendMessage("ChangeGhost1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Ghost2.SendMessage("ChangeGhost2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Ghost3.SendMessage("ChangeGhost3");
        }
    }

    public void SwitchRelic()
    {
        BL_HasRelic = (BL_HasRelic) ? false : true;

        switch (IN_ShrineNum)
        {
            case 1:
                Ghost1.SendMessage("ChangeGhost1");
                break;
            case 2:
                Ghost1.SendMessage("ChangeGhost2");
                break;
            case 3:
                Ghost1.SendMessage("ChangeGhost3");
                break;
        }

        if (BL_HasRelic)
        {
            GameObject.Find("AudioManager").GetComponent<JL_AudioManager>().PlaySound("ShrineAura");
        }
    }
}
