using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JC_LevelManager : MonoBehaviour
{
    protected GameObject[] mGO_ListOfNPCs;

    // Areas of Interest:
    // Area1
    private Vector2 mV2_Area1_X;
    private Vector2 mV2_Area1_Z;

    // Use this for initialization
    void Start()
    {
        mGO_ListOfNPCs = GameObject.FindGameObjectsWithTag("Ghost");
    }

    // Update is called once per frame
    void Update()
    {
        mV2_Area1_X = new Vector2(30, -3);
        mV2_Area1_Z = new Vector2(113, 80);
    }

    private void AssignNPCsToAreas()
    {
        foreach (GameObject vNPC in mGO_ListOfNPCs)
        {
            JC_FSM mSCR_FSM;
            mSCR_FSM = GetComponent<JC_FSM>();

            // Area 1: 30 < x < -3 && 113 < z < 80
            if ((vNPC.transform.position.x > mV2_Area1_X.y && vNPC.transform.position.x < mV2_Area1_X.x
                && (vNPC.transform.position.z > mV2_Area1_Z.y && vNPC.transform.position.z < mV2_Area1_Z.x)))
            {
                mSCR_FSM.mIN_AreaNo = 1;

                print("Is in Area 1");
            }

            //else if ((vNPC.transform.position.x > -3 && vNPC.transform.position.x < 30)
            //        && (vNPC.transform.position.z > 80 && vNPC.transform.position.z < 113))
            //{

            //}

            //else if ((vNPC.transform.position.x > -3 && vNPC.transform.position.x < 30)
            //        && (vNPC.transform.position.z > 80 && vNPC.transform.position.z < 113))
            //{

            //}

            //else if ((vNPC.transform.position.x > -3 && vNPC.transform.position.x < 30)
            //        && (vNPC.transform.position.z > 80 && vNPC.transform.position.z < 113))
            //{

            //}
        }
    }
}
