using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class JL_Interactable : MonoBehaviour
{
    GameObject GO_PC;
    JL_PCMovement SC_PCScript;

    [HideInInspector]
    public bool BL_Carried;

    // Use this for initialization
    void Start()
    {
        GO_PC = GameObject.Find("PC");
        SC_PCScript = GO_PC.GetComponent<JL_PCMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        switch (gameObject.name)
        {
            case "Relic":
                Debug.Log("I am a relic");
                if (!BL_Carried)
                {
                    //If i'm on a shrine, deactivate that shrine
                    if (transform.parent != null)
                    {
                        if (transform.parent.name == "Shrine")
                        {
                            transform.parent.GetComponent<JL_Shrine>().SwitchRelic();
                        }
                    }

                    gameObject.transform.SetParent(GO_PC.transform);
                    Transform temp = GO_PC.transform.Find("Target");
                    transform.localPosition = temp.localPosition;
                    BL_Carried = true;
                    SC_PCScript.BL_Carrying = true;
                    GameObject.Find("AudioManager").GetComponent<JL_AudioManager>().PlaySound("PickupRelic");
                }
                break;
            case "Shrine":
                if (SC_PCScript.BL_Carrying)
                {
                    GO_PC.transform.Find("Relic").transform.SetParent(gameObject.transform);
                    transform.Find("Relic").localPosition = gameObject.transform.Find("Target").transform.localPosition;
                    transform.Find("Relic").GetComponent<JL_Interactable>().BL_Carried = false;
                    SC_PCScript.BL_Carrying = false;
                    GameObject.Find("AudioManager").GetComponent<JL_AudioManager>().PlaySound("PlaceRelic");
                    gameObject.GetComponent<JL_Shrine>().SwitchRelic();
                }
                else Debug.Log("You are not carrying a relic");
                break;
            case "FakeShrine":
                if (SC_PCScript.BL_Carrying)
                {
                    GO_PC.transform.Find("Relic").transform.SetParent(gameObject.transform);
                    transform.Find("Relic").localPosition = gameObject.transform.Find("Target").transform.localPosition;
                    transform.Find("Relic").GetComponent<JL_Interactable>().BL_Carried = false;
                    SC_PCScript.BL_Carrying = false;
                    GameObject.Find("AudioManager").GetComponent<JL_AudioManager>().PlaySound("PlaceRelic");
                }
                else Debug.Log("You are not carrying a relic");
                break;
            case "EndPlatform":
                GameObject.Find("LevelManager").GetComponent<JL_LevelManager>().WinState();
                break;
            default:
                break;
        }
    }
}