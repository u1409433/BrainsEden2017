using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JC_FSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Roam,
        Chase,
        GoBack
    }

    // Getters, Setters variable for FSM:
    private State mStartState;
    public State mCurrentState;
    private State mPreviuosState;

    // Find PC:
    protected GameObject mGO_PC;
    public GameObject mPF_TestObjOK;
    public GameObject mPF_TestObjNotOK;

    // Navmesh:
    protected NavMeshAgent mNMA_NavMeshAgent;
    protected Vector3 mV3_NextDest;
    private float mFL_RoamSpeed;
    private float mFL_ChaseSpeed;

    public float mFL_SpeedGhost1;
    public float mFL_SpeedGhost2;
    public float mFL_SpeedGhost3;
    //public float mFL_SpeedArea4;

    public float mFL_FinalSpeedGhost1;
    public float mFL_FinalSpeedGhost2;
    public float mFL_FinalSpeedGhost3;
    //public float mFL_FinalSpeedArea4;

    public float mFL_ChasingSpeedGhost1;
    public float mFL_ChasingSpeedGhost2;
    public float mFL_ChasingSpeedGhost3;
    //public float mFL_ChasingSpeed4;

    public float mFL_FinalChasingSpeedGhost1;
    public float mFL_FinalChasingSpeedGhost2;
    public float mFL_FinalChasingSpeedGhost3;
    //public float mFL_FinalChasingSpeed4;

    [HideInInspector]
    public int mIN_AreaNo;

    // Movement Variables:
    [HideInInspector]
    public Vector3 mV3_TargetPos;
    protected Vector3 mV3_StartPos;

    // Random Destination:
    public float mFL_MaxRandomMove;
    public float mFL_MinRandomMove;

    // Roam:
    protected bool mBL_RoamReachedDest = false;
    protected bool mBL_IsRoaming = false;
    protected bool mBL_RoamTimerSet = false;
    protected float mFL_RoamTimer;
    public double mDB_RoamPause;

    // Chase:
    [HideInInspector]
    public float mFL_ChasingStoppingDistance;
    public float mFL_ChaseRange;
    public float mFL_ChaseDistance;

    protected bool mBL_ChaseReachedDest = false;
    protected bool mBL_IsChasing = false;
    protected bool mBL_ChaseTimerSet = false;
    protected float mFL_ChaseTimer;
    public double mDB_ChasePause;

    // Attack:
    protected bool mBL_IsAttacking;
    protected bool mBL_AttackTimerSet = false;
    protected float mFL_AttackTimer;
    protected double mDB_AttackPause;
    protected float mFL_AttackRange;
    protected float mFL_AttackDistance;

    // Blocking Movement for Chase and Roam:
    Vector2 mV2_AreaOfInterestX;
    Vector2 mV2_AreaOfInterestZ;

    JC_LevelManager mSCR_JCLevelManager;
    JL_PCMovement mSCR_Movement;

    bool mBL_AreaOfInterestSet = false;
    bool mBL_IsInArea = true;
    bool mBL_PCIsInArea = true;

    // Lights:
    //public Light mLG_FaceLight;

    // Use this for initialization
    void Start()
    {
        mNMA_NavMeshAgent = GetComponent<NavMeshAgent>();
        mGO_PC = GameObject.FindGameObjectWithTag("Player");
        mSCR_Movement = mGO_PC.GetComponent<JL_PCMovement>();
        mSCR_JCLevelManager = GameObject.Find("LevelManager").GetComponent<JC_LevelManager>();

        if (mSCR_JCLevelManager == null)
        {
            print("No Level Manager");
        }

        SetState(State.Roam);
    }

    // Update is called once per frame
    void Update()
    {
        //print("Area No: " + mIN_AreaNo);

        ApplyAreasOfInterest();
        ApplyFSM();
        AILogic();

        if (mCurrentState == State.Chase)
        {
            GameObject.Find("AudioManager").GetComponent<JL_AudioManager>().ReceiveGhostInfo(Vector3.Distance(transform.position, mV3_TargetPos));
            GameObject.Find("LevelManager").GetComponent<JC_LevelManager>().IN_ChasingGhosts++;
        }

        //print("STATE: " + GetState());
        //print("NPC is in area" + mBL_IsInArea);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (mIN_AreaNo == 1)
            {
                mV2_AreaOfInterestX = mSCR_JCLevelManager.mV2_Area1_X;
                mV2_AreaOfInterestZ = mSCR_JCLevelManager.mV2_Area1_Z;

                mFL_RoamSpeed = mFL_FinalSpeedGhost1;
                mFL_ChaseSpeed = mFL_FinalChasingSpeedGhost1;

                print("Area1 NewSpeed");
            }

            if (mIN_AreaNo == 4)
            {
                if (gameObject.transform.name == "FinalGhostArea1")
                {
                    mFL_RoamSpeed = mFL_FinalSpeedGhost1;
                    mFL_ChaseSpeed = mFL_FinalChasingSpeedGhost1;

                    print("Area4 NewSpeed Ghost1");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (mIN_AreaNo == 2)
            {
                mV2_AreaOfInterestX = mSCR_JCLevelManager.mV2_Area2_X;
                mV2_AreaOfInterestZ = mSCR_JCLevelManager.mV2_Area2_Z;

                mFL_RoamSpeed = mFL_FinalSpeedGhost2;
                mFL_ChaseSpeed = mFL_FinalChasingSpeedGhost2;

                print("Area2 NewSpeed");
            }

            if (mIN_AreaNo == 4)
            {
                if (gameObject.transform.name == "FinalGhostArea2")
                {
                    mFL_RoamSpeed = mFL_FinalSpeedGhost2;
                    mFL_ChaseSpeed = mFL_FinalChasingSpeedGhost2;

                    print("Area4 NewSpeed Ghost2");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (mIN_AreaNo == 3)
            {
                mV2_AreaOfInterestX = mSCR_JCLevelManager.mV2_Area3_X;
                mV2_AreaOfInterestZ = mSCR_JCLevelManager.mV2_Area3_Z;

                mFL_RoamSpeed = mFL_FinalSpeedGhost3;
                mFL_ChaseSpeed = mFL_FinalChasingSpeedGhost3;

                print("Area3 NewSpeed");
            }

            if (mIN_AreaNo == 4)
            {
                if (gameObject.transform.name == "FinalGhostArea3")
                {
                    mFL_RoamSpeed = mFL_FinalSpeedGhost3;
                    mFL_ChaseSpeed = mFL_FinalChasingSpeedGhost3;

                    print("Area4 NewSpeed Ghost3");
                }
            }
        }

        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    if (mIN_AreaNo == 4)
        //    {
        //        mV2_AreaOfInterestX = mSCR_JCLevelManager.mV2_Area4_X;
        //        mV2_AreaOfInterestZ = mSCR_JCLevelManager.mV2_Area4_Z;

        //        mV2_AreaOfInterestX = mSCR_JCLevelManager.mV2_Area4_X;
        //        mV2_AreaOfInterestZ = mSCR_JCLevelManager.mV2_Area4_Z;

        //        if (gameObject.transform.tag == "AreaFour")
        //        {
        //            if (gameObject.transform.name == "FinalGhostArea1")
        //            {
        //                mFL_RoamSpeed = mFL_FinalSpeedGhost1;
        //                mFL_ChaseSpeed = mFL_FinalChasingSpeedGhost1;

        //                print("Area4 NewSpeed Ghost1");
        //            }

        //            if (gameObject.transform.name == "FinalGhostArea2")
        //            {
        //                mFL_RoamSpeed = mFL_FinalSpeedGhost2;
        //                mFL_ChaseSpeed = mFL_FinalChasingSpeedGhost2;
        //                print("Area4 NewSpeed Ghost2");
        //            }

        //            if (gameObject.transform.name == "FinalGhostArea3")
        //            {
        //                mFL_RoamSpeed = mFL_FinalSpeedGhost3;
        //                mFL_ChaseSpeed = mFL_FinalChasingSpeedGhost3;
        //                print("Area4 NewSpeed Ghost3");
        //            }
        //        }
        //    }
        //}

        print("SPEED: " + mNMA_NavMeshAgent.speed);
    }

    protected void ApplyFSM()
    {
        switch (GetState())
        {
            case State.Idle: /*Do nothing*/; break;

            case State.Roam: Roam();
                            /*mLG_FaceLight.enabled = false;*/ break;

            case State.Chase: Chase();
                            //mLG_FaceLight.enabled = true;
                            /*AkSoundEngine.PostEvent("FaceLight", gameObject)*/;
                            break;

            case State.GoBack:; break;
        }
    }

    private void ApplyAreasOfInterest()
    {
        if (!mBL_AreaOfInterestSet)
        {
            if (mIN_AreaNo == 1)
            {
                mV2_AreaOfInterestX = mSCR_JCLevelManager.mV2_Area1_X;
                mV2_AreaOfInterestZ = mSCR_JCLevelManager.mV2_Area1_Z;

                mFL_RoamSpeed = mFL_SpeedGhost1;
                mFL_ChaseSpeed = mFL_ChasingSpeedGhost1;
            }

            else if (mIN_AreaNo == 2)
            {
                mV2_AreaOfInterestX = mSCR_JCLevelManager.mV2_Area2_X;
                mV2_AreaOfInterestZ = mSCR_JCLevelManager.mV2_Area2_Z;

                mFL_RoamSpeed = mFL_SpeedGhost2;
                mFL_ChaseSpeed = mFL_ChasingSpeedGhost2;
            }

            else if (mIN_AreaNo == 3)
            {
                mV2_AreaOfInterestX = mSCR_JCLevelManager.mV2_Area3_X;
                mV2_AreaOfInterestZ = mSCR_JCLevelManager.mV2_Area3_Z;

                mFL_RoamSpeed = mFL_SpeedGhost3;
                mFL_ChaseSpeed = mFL_ChasingSpeedGhost3;
            }

            else if (mIN_AreaNo == 4)
            {
                mV2_AreaOfInterestX = mSCR_JCLevelManager.mV2_Area4_X;
                mV2_AreaOfInterestZ = mSCR_JCLevelManager.mV2_Area4_Z;

                if (gameObject.transform.tag == "AreaFour")
                {
                    if (gameObject.transform.name == "FinalGhostArea1")
                    {
                        mFL_RoamSpeed = mFL_SpeedGhost1;
                        mFL_ChaseSpeed = mFL_ChasingSpeedGhost1;  
                    }

                    if (gameObject.transform.name == "FinalGhostArea2")
                    {
                        mFL_RoamSpeed = mFL_SpeedGhost2;
                        mFL_ChaseSpeed = mFL_ChasingSpeedGhost2;
                    }

                    if (gameObject.transform.name == "FinalGhostArea3")
                    {
                        mFL_RoamSpeed = mFL_SpeedGhost3;
                        mFL_ChaseSpeed = mFL_ChasingSpeedGhost3;
                    }
                }
            }

            mBL_AreaOfInterestSet = true;
        }
    }

    private void AILogic()
    {
        // Chase Distance.
        if (Vector3.Distance(gameObject.transform.position, mGO_PC.transform.position) <= mFL_ChaseDistance)
        {
            SetState(State.Chase);

            if (GetState() == State.Chase)
            {
                CheckPlayerInArea();
                //print("Is PC in area: " + mBL_PCIsInArea);

                if (!mBL_PCIsInArea)
                {
                    SetState(State.Roam);
                }
            }
        }

        else
        {
            SetState(State.Roam);
        }
    }

    protected void CheckPlayerInArea()
    {
        if (mGO_PC.transform.position.x >= mV2_AreaOfInterestX.y && mGO_PC.transform.position.x < mV2_AreaOfInterestX.x)
        {
            if (mGO_PC.transform.position.z >= mV2_AreaOfInterestZ.y && mGO_PC.transform.position.z < mV2_AreaOfInterestZ.x)
            {
                //print("PC Is in Bounds");
                mBL_PCIsInArea = true;
            }

            else
            {
                //print("PC Outside Bounds");
                mBL_PCIsInArea = false;
            }
        }

        else
        {
            //print("PC Outside Bounds");
            mBL_PCIsInArea = false;
        }
    }

    protected void CheckInArea()
    {
        if (mV3_TargetPos.x >= mV2_AreaOfInterestX.y && mV3_TargetPos.x < mV2_AreaOfInterestX.x)
        {
            if (mV3_TargetPos.z >= mV2_AreaOfInterestZ.y && mV3_TargetPos.z < mV2_AreaOfInterestZ.x)
            {
                //print("Is in Bounds");
                mBL_IsInArea = true;
            }

            else
            {
                //print("NPC Outside Bounds");
                mBL_IsInArea = false;
            }
        }

        else
        {
            //print("NPC Outside Bounds");
            mBL_IsInArea = false;
        }
    }

    protected void GoBack()
    {
        Vector3 tV3_OppositeDirection = Vector3.Reflect(mV3_TargetPos, Vector3.back);

        mNMA_NavMeshAgent.SetDestination(tV3_OppositeDirection);
    }

    protected void Roam()
    {
        if (!mBL_RoamReachedDest)
        {
            bool mBL_CanGo = true;

            if (!mBL_IsRoaming)
            {
                Ray tRY_Ray = new Ray((RandomDestination() + new Vector3(0, 20, 0)), Vector3.down);
                RaycastHit tRY_Hit;

                if (Physics.Raycast(tRY_Ray, out tRY_Hit, 150))
                {
                    NavMeshHit tNM_Hit;
                    int tIN_LayerWalkable = 1 << NavMesh.GetAreaFromName("Walkable");

                    CheckInArea();

                    if (NavMesh.SamplePosition(tRY_Hit.point, out tNM_Hit, 1, tIN_LayerWalkable))// && tBL_IsInArea) // tRY_Hit.transform.tag == "Terrain")
                    {
                        mV3_TargetPos = new Vector3(tRY_Hit.point.x, tRY_Hit.point.y + 1f, tRY_Hit.point.z);

                        if (transform.position.y > tNM_Hit.position.y)
                        {
                            if (transform.position.y - tNM_Hit.position.y > 1)
                            {
                                //print("Wrong Height: " + tNM_Hit.position.y + ", Player Height: " + transform.position.y);
                                mBL_CanGo = false;
                            }
                        }

                        else
                        {
                            if (tNM_Hit.position.y - transform.position.y > 3)
                            {
                                //print("Wrong Height: " + tNM_Hit.position.y + ", Player Height: " + transform.position.y);
                                mBL_CanGo = false;
                            }
                        }

                        if (!mBL_IsInArea)
                        {
                            mBL_CanGo = false;
                        }

                        if (mBL_CanGo)
                        {
                            //print("Correct Height: " + tNM_Hit.position.y + ", Player Height: " + transform.position.y);
                            mBL_IsRoaming = true;
                            mNMA_NavMeshAgent.speed = mFL_RoamSpeed;
                            mNMA_NavMeshAgent.SetDestination(mV3_TargetPos);
                        }
                    }
                }
            }

            else
            {
                if (Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z), new Vector2(mV3_TargetPos.x, mV3_TargetPos.z)) < 1f)
                {
                    mBL_RoamReachedDest = true;
                    mBL_IsRoaming = false;
                }
            }
        }

        else
        {
            if (!mBL_RoamTimerSet)
            {
                mFL_RoamTimer = Time.time + (float)mDB_RoamPause;
                mBL_RoamTimerSet = true;
            }

            if (Time.time > mFL_RoamTimer)
            {
                mBL_RoamReachedDest = false;
                mBL_RoamTimerSet = false;
            }
        }
    }


    protected void Chase()
    {
        if (!mBL_ChaseReachedDest)
        {
            if (!mBL_IsChasing)
            {
                mV3_TargetPos = mGO_PC.transform.position;
                mNMA_NavMeshAgent.isStopped = false;
                mNMA_NavMeshAgent.speed = mFL_ChaseSpeed;
                mNMA_NavMeshAgent.SetDestination(mV3_TargetPos);

                mBL_IsChasing = true;

                //print("Chase Destination Set: " + mBL_IsChasing.ToString());
            }

            else if (Vector3.Distance(mV3_TargetPos, gameObject.transform.position) <= mFL_ChaseRange)
            {
                // In Range:
                mNMA_NavMeshAgent.isStopped = true;
                mBL_IsChasing = false;
                mBL_ChaseReachedDest = true;
                //print("NPC Stopping");
            }
        }

        else
        {
            if (!mBL_ChaseTimerSet)
            {
                mFL_ChaseTimer = Time.time + (float)mDB_ChasePause;
                mBL_ChaseTimerSet = true;
            }

            if (Time.time > mFL_ChaseTimer)
            {
                mBL_ChaseReachedDest = false;
                mBL_ChaseTimerSet = false;
            }
        }
    }

    protected void MeleeAttack()
    {
        if (mBL_IsAttacking)
        {
            // Melee Attack Within Range
            if (Vector3.Distance(gameObject.transform.position, mGO_PC.transform.position) > 0.5)
            {
                //Attack:
                //print("Attack within range");
                mBL_IsAttacking = false;
            }

            else
            {
                // Just play Animation:
                //print("Attack within range");
                mBL_IsAttacking = false;
            }
        }

        else
        {
            if (!mBL_AttackTimerSet)
            {
                mFL_AttackTimer = Time.time + (float)mDB_AttackPause;
                mBL_AttackTimerSet = true;
            }

            if (Time.time > mFL_AttackTimer)
            {
                mBL_AttackTimerSet = false;
            }
        }
    }

    // Random Movement:
    protected Vector3 RandomDestination()
    {
        Vector3 tV3_TargetPos = new Vector3();
        float tFL_RandomDistance = Random.Range(mFL_MinRandomMove, mFL_MaxRandomMove);

        tV3_TargetPos = gameObject.transform.position + transform.TransformDirection(Vector3.forward * tFL_RandomDistance) + transform.TransformDirection(Vector3.left * Random.Range(-mFL_MinRandomMove, mFL_MaxRandomMove));
        transform.LookAt(tV3_TargetPos);

        return tV3_TargetPos;
    }

    // Getters & Setters for FSM:
    public void SetState(State vState)
    {
        mPreviuosState = mCurrentState;
        mCurrentState = vState;
    }

    public State GetState()
    {
        return mCurrentState;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            mSCR_Movement.Die();
            //collision.transform.SendMessage("Die");
        }
    }
}
