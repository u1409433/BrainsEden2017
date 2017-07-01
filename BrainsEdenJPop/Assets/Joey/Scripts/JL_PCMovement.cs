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

        ButtonInput();

        if (BL_Carrying) Agent_PC.speed = FL_Speed / 2;
        else Agent_PC.speed = FL_Speed;

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

        //Debug.Log("Bob");
        //if (BL_BobRight)
        //{
        //    gameObject.transform.Rotate(new Vector3(0, 0, -0.5f));
        //    if (transform.rotation.eulerAngles.z <= -10)
        //    {
        //        BL_BobRight = false;
        //    }
        //}
        //else
        //{
        //    gameObject.transform.Rotate(new Vector3(0, 0, 0.5f));
        //    if (transform.rotation.eulerAngles.z >= 10)
        //    {
        //        BL_BobRight = true;
        //    }
        //}


        //}
        //else
        //{
        //    Debug.Log("Do Not Bob");
        //    if (transform.rotation.eulerAngles.z <= -0.5f)
        //    {
        //        transform.Rotate(0, 0, 0.5f);
        //    }
        //    else if (transform.rotation.eulerAngles.z >= 0.5f)
        //    {
        //        transform.Rotate(0, 0, -0.5f);
        //    }
        //    else Debug.Log("I'm centered");
        //}*/
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
                    RayHit.transform.SendMessage("Interact");
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

    void ButtonInput()
    {
        //Pickup an object
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F");

            float tFL_Close = 1000;
            GameObject tGO_Close = null;

            foreach (GameObject Interactable in SC_LevelManager.LS_GO_Interactables)
            {
                if (Vector3.Distance(Interactable.transform.position, gameObject.transform.position) < tFL_Close)
                {
                    tFL_Close = Vector3.Distance(Interactable.transform.position, gameObject.transform.position);
                    tGO_Close = Interactable;
                }
            }

            tGO_Close.SendMessage("Interact");
        }

        //Put down an object
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (BL_Carrying)
            {
                //Put the object down on a shrine or just on the ground
            }
            else
            {
                Debug.Log("I am not carrying anything");
            }
        }
    }

    public void Die()
    {
        transform.position = new Vector3(70, 7, 42);
        Debug.Log("I Died!");
        //Play "Pop sounds" and start base ambience
        //HERE
        SC_AudioManager.PlaySound("PainLow");
    }

    void Dodge(Vector3 vV3_SpawnPoint)
    {
        transform.position = vV3_SpawnPoint;
        SC_AudioManager.PlaySound("PainLow");
        Debug.Log("I Dodged!");
    }

    public void OnTriggerEnter(Collider vCollided)
    {
        Debug.Log("Collision");
        if (vCollided.transform.name == "Cone")
        {
            Vector3 temp = vCollided.GetComponent<JL_Cone>().V3_DodgePoint;
            Dodge(temp);
        }
    }
}
