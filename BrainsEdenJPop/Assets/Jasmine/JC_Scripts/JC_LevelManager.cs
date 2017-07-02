using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JC_LevelManager : MonoBehaviour
{
    protected GameObject[] mGO_ListOfNPCs;

    public int IN_ChasingGhosts = 0;

    // Areas of Interest:
    // Area1
    [HideInInspector]
    public Vector2 mV2_Area1_X;
    [HideInInspector]
    public Vector2 mV2_Area1_Z;
    [HideInInspector]
    public Vector2 mV2_Area2_X;
    [HideInInspector]
    public Vector2 mV2_Area2_Z;
    [HideInInspector]
    public Vector2 mV2_Area3_X;
    [HideInInspector]
    public Vector2 mV2_Area3_Z;
    [HideInInspector]
    public Vector2 mV2_Area4_X;
    [HideInInspector]
    public Vector2 mV2_Area4_Z;


    // Use this for initialization
    void Start()
    {
        mGO_ListOfNPCs = GameObject.FindGameObjectsWithTag("Ghost");

        //print("Amount of NPCs: " + mGO_ListOfNPCs.Length);

        mV2_Area1_X = new Vector2(113, 80);
        mV2_Area1_Z = new Vector2(40, -3);

        mV2_Area2_X = new Vector2(91, 51);
        mV2_Area2_Z = new Vector2(93, 69);

        mV2_Area3_X = new Vector2(51, 17);
        mV2_Area3_Z = new Vector2(58, -3);

        mV2_Area4_X = new Vector2(70, 60);
        mV2_Area4_Z = new Vector2(37, 15);

        AssignNPCsToAreas();
    }

    // Update is called once per frame
    void Update()
    {
        IN_ChasingGhosts = 0;
    }

    private void AssignNPCsToAreas()
    {
        foreach (GameObject vNPC in mGO_ListOfNPCs)
        {
            JC_FSM mSCR_FSM;
            mSCR_FSM = vNPC.GetComponent<JC_FSM>();

            // Area 1: 30 < x < -3 && 113 < z < 80
            if (vNPC.transform.position.x > mV2_Area1_X.y && vNPC.transform.position.x < mV2_Area1_X.x)
            {
                if (vNPC.transform.position.z > mV2_Area1_Z.y && vNPC.transform.position.z < mV2_Area1_Z.x)
                {
                    mSCR_FSM.mIN_AreaNo = 1;

                    //print("Is in Area 1"); 
                }

                else
                {
                    //print("NPC Outside Area 1");
                }
            }

            else
            {
                //print("NPC Outside Area 1");
            }

            if (vNPC.transform.position.x > mV2_Area2_X.y && vNPC.transform.position.x < mV2_Area2_X.x)
            {
                if ((vNPC.transform.position.z > mV2_Area2_Z.y && vNPC.transform.position.z < mV2_Area2_Z.x))
                {
                    mSCR_FSM.mIN_AreaNo = 2;

                    //print("Is in Area 2"); 
                }

                else
                {
                    //print("NPC Outside Area 2");
                }
            }

            else
            {
                //print("NPC Outside Area 2");
            }

            if (vNPC.transform.position.x > mV2_Area3_X.y && vNPC.transform.position.x < mV2_Area3_X.x)
            {
                if (vNPC.transform.position.z > mV2_Area3_Z.y && vNPC.transform.position.z < mV2_Area3_Z.x)
                {
                    mSCR_FSM.mIN_AreaNo = 3;

                    //print("Is in Area 3");
                }

                else
                {
                    //print("NPC Outside Area 3");
                }
            }

            else
            {
                //print("NPC Outside Area 3");
            }

            if (vNPC.transform.position.x > mV2_Area4_X.y && vNPC.transform.position.x < mV2_Area4_X.x)
            {
                if (vNPC.transform.position.z > mV2_Area4_Z.y && vNPC.transform.position.z < mV2_Area4_Z.x)
                {
                    mSCR_FSM.mIN_AreaNo = 4;

                    print("Is in Area 4");
                }

                else
                {
                    //print("NPC Outside Area 3");
                }
            }

            else
            {
                //print("NPC Outside Area 3");
            }
        }
    }
}
