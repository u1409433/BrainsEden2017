using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class JL_PCMovement : MonoBehaviour
{
    public NavMeshAgent Agent_PC;
    public float FL_Speed;

    public Camera Cam_Main;

    public JL_LevelManager SC_LevelManager;
    
    public bool BL_Carrying;

    // Use this for initialization
    void Start()
    {
        Agent_PC = gameObject.GetComponent<NavMeshAgent>();

        Cam_Main = GameObject.Find("Main Camera").GetComponent<Camera>();

        SC_LevelManager = GameObject.Find("LevelManager").GetComponent<JL_LevelManager>();

        FL_Speed = Agent_PC.speed;
    }

    // Update is called once per frame
    void Update()
    {
        MouseInput();

        ButtonInput();

        if (BL_Carrying) Agent_PC.speed = FL_Speed / 2;
        else Agent_PC.speed = FL_Speed;
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
}
