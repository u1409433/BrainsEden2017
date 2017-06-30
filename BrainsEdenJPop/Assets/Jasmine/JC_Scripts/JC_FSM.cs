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
        Chase//,
        //Attack
    }

    // Getters, Setters variable for FSM:
    private State mStartState;
    private State mCurrentState;
    private State mPreviuosState;

    // Find PC:
    protected GameObject mGO_PC;
    public GameObject mPF_TestObj;

    // Navmesh:
    protected NavMeshAgent mNMA_NavMeshAgent;
    public float mFL_Speed;

    // Movement Variables:
    protected Vector3 mV3_TargetPos;
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

    // Attack:
    protected bool mBL_IsAttacking;
    protected bool mBL_AttackTimerSet = false;
    protected float mFL_AttackTimer;
    public double mDB_AttackPause;

    public float mFL_AttackRange;
    public float mFL_AttackDistance;

    // Use this for initialization
    void Start()
    {
        mNMA_NavMeshAgent = GetComponent<NavMeshAgent>();
        mGO_PC = GameObject.FindGameObjectWithTag("Player");

        SetState(State.Roam);
    }

    // Update is called once per frame
    void Update()
    {
        ApplyFSM();

        AILogic();
    }

    protected void ApplyFSM()
    {
        switch (GetState())
        {
            case State.Idle: /*Do nothing*/; break;
            case State.Roam: Roam(); break;
            case State.Chase: Chase(); break;
                //case State.Attack:  /*MeleeAttack()*/; break;
        }
    }

    private void AILogic()
    {
        // Chase Distance.
        if (Vector3.Distance(gameObject.transform.position, mGO_PC.transform.position) <= mFL_ChaseDistance)
        {
            SetState(State.Chase);
        }

        else
        {
            SetState(State.Roam);
        }

        // Attack Distance:
        //if (Vector3.Distance(gameObject.transform.position, mGO_PC.transform.position) <= mFL_AttackDistance)
        //{
        //    SetState(State.Attack);
        //}

        //else
        //{
        //    if (Vector3.Distance(gameObject.transform.position, mGO_PC.transform.position) <= mFL_ChaseDistance)
        //    {
        //        SetState(State.Chase);
        //    }

        //    else
        //    {
        //        SetState(State.Roam);
        //    }
        //}
    }

    protected void Roam()
    {
        if (!mBL_RoamReachedDest)
        {
            if (!mBL_IsRoaming)
            {
                mV3_StartPos = gameObject.transform.position;
                mV3_TargetPos = RandomDestination();

                Ray tRY_Ray = new Ray((RandomDestination() + new Vector3(0, 20, 0)), Vector3.down);
                RaycastHit tRY_Hit;

                if (Physics.Raycast(tRY_Ray, out tRY_Hit, 150))
                {
                    //NavMeshHit tNM_Hit;

                    if (tRY_Hit.transform.tag == "Terrain")
                    {
                        mV3_TargetPos = new Vector3(tRY_Hit.point.x, tRY_Hit.point.y + 1, tRY_Hit.point.z);
                        mBL_IsRoaming = true;

                        // Testing:
                        // Instantiate(mPF_TestObj, mV3_TargetPos, transform.rotation);

                        mNMA_NavMeshAgent.speed = mFL_Speed;
                        mNMA_NavMeshAgent.SetDestination(mV3_TargetPos);
                    }

                    else
                    {
                        mV3_TargetPos = RandomDestination();
                    }
                }
            }

            else if (Vector3.Distance(transform.position, mV3_TargetPos) < 0.5f)
            {
                mBL_RoamReachedDest = true;
                mBL_IsRoaming = false;
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
        mV3_TargetPos = mGO_PC.transform.position;

        if (Vector3.Distance(mV3_TargetPos, gameObject.transform.position) <= mFL_ChaseRange)
        {
            // In Range:
            mNMA_NavMeshAgent.isStopped = true;
        }

        else
        {
            mV3_TargetPos = mGO_PC.transform.position;
            mNMA_NavMeshAgent.isStopped = false;
            mNMA_NavMeshAgent.SetDestination(mV3_TargetPos);
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
                print("Attack within range");
                mBL_IsAttacking = false;
            }

            else
            {
                // Just play Animation:
                print("Attack within range");
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

    void OnCollisionEnter(Collision vColl)
    {
        if (vColl.transform.tag == "Player")
        {
            // Do the Spawning.
        }
    }
}
