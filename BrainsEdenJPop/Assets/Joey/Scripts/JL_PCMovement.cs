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

    public bool BL_BobRight = true;

    // Use this for initialization
    void Start()
    {
        Agent_PC = gameObject.GetComponent<NavMeshAgent>();

        Cam_Main = GameObject.Find("Main Camera").GetComponent<Camera>();

        SC_LevelManager = GameObject.Find("LevelManager").GetComponent<JL_LevelManager>();
        SC_AudioManager = GameObject.Find("AudioManager").GetComponent<JL_AudioManager>();

        FL_Speed = Agent_PC.speed;
        FL_Speed += 1.5f;

    }

    // Update is called once per frame
    void Update()
    {
        MouseInput();

        if (BL_Carrying) Agent_PC.speed = FL_Speed / 2;
        else Agent_PC.speed = FL_Speed;

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
        if (Vector3.Distance(transform.position, Agent_PC.destination) > 1f)
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
        transform.position = new Vector3(70, 7, 42);
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
            Vector3 temp = vCollided.GetComponent<JL_Cone>().V3_DodgePoint;
            Dodge(temp);
        }
    }
}
