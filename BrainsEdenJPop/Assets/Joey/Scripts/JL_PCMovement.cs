using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class JL_PCMovement : MonoBehaviour
{
    public NavMeshAgent Agent_PC;
    public float FL_Speed;

    public Camera Cam_Main;

    private JL_LevelManager SC_LevelManager;
    private JL_AudioManager SC_AudioManager;
    
    public bool BL_Carrying;
    public bool BL_Dying;

    private Vector3 V3_DeathRot = new Vector3(0,0,-90);
    private Vector3 V3_SpawnPoint;
    private Quaternion QU_SpawnRotation;
    private Vector3 V3_CurrentRot;

    // Use this for initialization
    void Start()
    {
        Agent_PC = gameObject.GetComponent<NavMeshAgent>();

        Cam_Main = GameObject.Find("Main Camera").GetComponent<Camera>();

        SC_LevelManager = GameObject.Find("LevelManager").GetComponent<JL_LevelManager>();
        SC_AudioManager = GameObject.Find("AudioManager").GetComponent<JL_AudioManager>();

        FL_Speed = Agent_PC.speed;
        FL_Speed += 1.5f;

        V3_SpawnPoint = transform.position;
        QU_SpawnRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        if (BL_Dying)
        {
            V3_CurrentRot = new Vector3(Mathf.LerpAngle(V3_CurrentRot.x, V3_DeathRot.x, Time.deltaTime),
                                        Mathf.LerpAngle(V3_CurrentRot.y, V3_DeathRot.y, Time.deltaTime),
                                        Mathf.LerpAngle(V3_CurrentRot.z, V3_DeathRot.z, Time.deltaTime));

            transform.eulerAngles = V3_CurrentRot;
        }
        else
        {
            MouseInput();

            if (BL_Carrying) Agent_PC.speed = FL_Speed / 2;
            else Agent_PC.speed = FL_Speed;
        }

        FootSteps();
    }

    void MouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit RayHit;
            Ray Ray = Cam_Main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(Ray, out RayHit, 100f))
            {
                if (RayHit.transform.tag == "Terrain")
                {
                    Agent_PC.SetDestination(RayHit.point);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit RayHit;
            Ray Ray = Cam_Main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(Ray, out RayHit, 100f))
            {
                if (RayHit.transform.tag == "Interactable" && Vector3.Distance(RayHit.transform.position, transform.position) < 3f)
                {
                    if (RayHit.transform.name == "Relic" && BL_Carrying)
                    {
                        Debug.Log("You're already holding a relic!");
                    }
                    else
                    {
                        RayHit.transform.SendMessage("Interact");
                    }
                }
                else if (RayHit.transform.tag == "Terrain" && Vector3.Distance(RayHit.point, transform.position) < 3f)
                {
                    transform.Find("Relic").GetComponent<JL_Interactable>().BL_Carried = false;
                    transform.Find("Relic").transform.position = RayHit.point;
                    transform.Find("Relic").SetParent(null);
                    BL_Carrying = false;
                }
            }
        }
    }

    void FootSteps()
    {
        //If im walking 
        if (Vector3.Distance(transform.position, Agent_PC.destination) > 2f)
        {
            if (BL_Carrying)
            {
                SC_AudioManager.SwitchFootsteps("Slow");
            }
            else
            {
                SC_AudioManager.SwitchFootsteps("Fast");
            }
        }
        else
        {
            SC_AudioManager.SwitchFootsteps("Stop");
        }
    }

    public void Die()
    {
        Debug.Log("Die");
        //Start the lerp to my death position
        V3_CurrentRot = transform.rotation.eulerAngles;
        BL_Dying = true;
        Agent_PC.SetDestination(transform.position);

        Invoke("Respawn", 5);

        //transform.position = new Vector3(70, 7, 42);
        SC_AudioManager.PlaySound("PainLow");
    }

    void Dodge(Vector3 vV3_SpawnPoint)
    {
        transform.position = vV3_SpawnPoint;
        SC_AudioManager.PlaySound("PainLow");
    }

    public void OnTriggerEnter(Collider vCollided)
    {
        if (vCollided.transform.name == "Cone")
        {
            //Vector3 temp = vCollided.GetComponent<JL_Cone>().V3_DodgePoint;
            if (!BL_Dying)
            {
                Die();
                if (transform.Find("Relic") != null)
                {
                    transform.Find("Relic").transform.SetParent(null);
                    transform.Find("Relic").GetComponent<JL_Interactable>().BL_Carried = false;
                    BL_Carrying = false;
                }
            }
        }
        else if (vCollided.transform.name == "WinState")
        {
            SC_LevelManager.WinState();
        }
    }

    public void Respawn()
    {
        transform.position = V3_SpawnPoint;
        transform.rotation = QU_SpawnRotation;
        BL_Dying = false;
        Agent_PC.SetDestination(transform.position);
    }
}
