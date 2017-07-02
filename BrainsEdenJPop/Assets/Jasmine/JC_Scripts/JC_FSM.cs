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
        Chase
    }

    // Getters, Setters variable for FSM:
    private State mStartState;
    private State mCurrentState;
    private State mPreviuosState;

    // Find PC:
    protected GameObject mGO_PC;
    public GameObject mPF_TestObjOK;
    public GameObject mPF_TestObjNotOK;

    // Navmesh:
    protected NavMeshAgent mNMA_NavMeshAgent;
    protected Vector3 mV3_NextDest;
    public float mFL_Speed;
    public float mFL_FinalSpeed;

    [HideInInspector]
    public int mIN_AreaNo;

    // Movement Variables:
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
    public double mDB_AttackPause;

    public float mFL_AttackRange;
    public float mFL_AttackDistance;

    // Blocking Movement for Chase and Roam:
    bool mBL_AreaOfInterestSet = false;
    Vector2 mV2_AreaOfInterestX;
    Vector2 mV2_AreaOfInterestZ;
    JC_LevelManager mSCR_JCLevelManager;

    // Use this for initialization
    void Start()
    {
        mNMA_NavMeshAgent = GetComponent<NavMeshAgent>();
        mGO_PC = GameObject.FindGameObjectWithTag("Player");
        mSCR_JCLevelManager = GameObject.Find("LevelManager").GetComponent<JC_LevelManager>();

        if (mSCR_JCLevelManager == null)
        {
            print("No Level Manager");
        }

        SetState(State.Chase);
        //SetState(State.Roam);
    }

    // Update is called once per frame
    void Update()
    {
        //print("Area No: " + mIN_AreaNo);

        ApplyAreasOfInterest();

        CheckInArea();
        print(CheckInArea().ToString());

        ApplyFSM();

        AILogic();

        if (mCurrentState == State.Chase)
        {
            GameObject.Find("AudioManager").GetComponent<JL_AudioManager>().ReceiveGhostInfo(Vector3.Distance(transform.position, mV3_TargetPos));
            GameObject.Find("LevelManager").GetComponent<JC_LevelManager>().IN_ChasingGhosts++;
        }
    }

    protected void ApplyFSM()
    {
        switch (GetState())
        {
            case State.Idle: /*Do nothing*/; break;
            case State.Roam: Roam(); break;
            case State.Chase: Chase(); break;
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
            }

            else if (mIN_AreaNo == 2)
            {
                mV2_AreaOfInterestX = mSCR_JCLevelManager.mV2_Area2_X;
                mV2_AreaOfInterestZ = mSCR_JCLevelManager.mV2_Area2_Z;
            }

            else if (mIN_AreaNo == 3)
            {
                mV2_AreaOfInterestX = mSCR_JCLevelManager.mV2_Area3_X;
                mV2_AreaOfInterestZ = mSCR_JCLevelManager.mV2_Area3_Z;
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
                //if (!(transform.position.x > mV2_AreaOfInterestX.y && transform.position.x < mV2_AreaOfInterestZ.x))
                //{
                //    if (!(transform.position.z > mV2_AreaOfInterestZ.y && transform.position.z > mV2_AreaOfInterestZ.x))
                //    {
                //        SetState(State.Roam); 
                //    }
                //} 
            }
        }

        else
        {
            SetState(State.Roam);
        }
    }

    protected bool CheckInArea()
    {
        bool tBL_IsInArea = true;

        if (transform.position.x > mV2_AreaOfInterestX.y && transform.position.x < mV2_AreaOfInterestX.x)
        {
            if (transform.position.z > mV2_AreaOfInterestZ.y && transform.position.z < mV2_AreaOfInterestZ.x)
            {
                print("Is in Bounds");
                tBL_IsInArea = true;
            }

            else
            {
                print("NPC Outside Bounds");
                tBL_IsInArea = false;
            }
        }

        return tBL_IsInArea;
    }
    
#if !UNITY_EDITOR
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

                    if (NavMesh.SamplePosition(tRY_Hit.point, out tNM_Hit, 1, tIN_LayerWalkable))// && tRY_Hit.transform.tag == "Terrain")
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

                        if (mBL_CanGo)
                        {
                            //print("Correct Height: " + tNM_Hit.position.y + ", Player Height: " + transform.position.y);
                            mBL_IsRoaming = true;
                            mNMA_NavMeshAgent.speed = mFL_Speed;
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
#else

    protected void Roam()
    {
        if (!mBL_RoamReachedDest)
        {
            bool mBL_CanGo = true;

            if (!mBL_IsRoaming)
            {
                Ray tRY_Ray = new Ray((RandomDestination() + new Vector3(0, 20, 0)), Vector3.down);
                RaycastHit tRY_Hit;

                if (Physics.Raycast(tRY_Ray, out tRY_Hit, 15000))
                {
                    NavMeshHit tNM_Hit;
                    int tIN_LayerWalkable = 1 << NavMesh.GetAreaFromName("Walkable");

                    if (NavMesh.SamplePosition(tRY_Hit.point, out tNM_Hit, 1, tIN_LayerWalkable))// && tRY_Hit.transform.tag == "Terrain")
                    {
                        mV3_TargetPos = new Vector3(tRY_Hit.point.x, tRY_Hit.point.y + 1f, tRY_Hit.point.z);

                        // Check if within the right Area:
                        // Check if Right Height:
                        if (transform.position.y > tNM_Hit.position.y)
                        {
                            print("Wrong Height: " + tNM_Hit.position.y + ", Player Height: " + transform.position.y);

                            if (transform.position.y - tNM_Hit.position.y > 1)
                            {
                                print("Wrong Height: " + tNM_Hit.position.y + ", Player Height: " + transform.position.y);
                                mBL_CanGo = false;
                            }
                        }

                        else if (!(transform.position.y > tNM_Hit.position.y))
                        {
                            if (tNM_Hit.position.y - transform.position.y > 3)
                            {
                                print("Wrong Height: " + tNM_Hit.position.y + ", Player Height: " + transform.position.y);
                                mBL_CanGo = false;
                            }
                        }
                                                
                        if (CheckInArea() == false)
                        {
                            print("Roam: Outside bounds");
                            mBL_CanGo = false;
                        }
                        
                        if (mBL_CanGo)
                        {
                            //print("Correct Height: " + tNM_Hit.position.y + ", Player Height: " + transform.position.y);
                            mBL_IsRoaming = true;

                            //if (true)
                            //{
                                mNMA_NavMeshAgent.speed = mFL_Speed;
                            //}

                            mNMA_NavMeshAgent.SetDestination(mV3_TargetPos);
                        }
                    }
                }

                else
                {
                    Debug.LogError("Raycast did not hit anything. Will retry next frame.");
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
#endif

    protected void Chase()
    {
        if (!mBL_ChaseReachedDest)
        {
            if (!mBL_IsChasing)
            {
                mV3_TargetPos = mGO_PC.transform.position;
                mNMA_NavMeshAgent.isStopped = false;
                mNMA_NavMeshAgent.SetDestination(mV3_TargetPos);

                mBL_IsChasing = true;

                print("Chase Destination Set: " + mBL_IsChasing.ToString());
            }

            else if (Vector3.Distance(mV3_TargetPos, gameObject.transform.position) <= mFL_ChaseRange)
            {
                // In Range:
                mNMA_NavMeshAgent.isStopped = true;
                mBL_IsChasing = false;
                mBL_ChaseReachedDest = true;
                print("NPC Stopping");
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

        // Areas of Interest:
        //Area 1:
        //if ()
        //{

        //}
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
}
