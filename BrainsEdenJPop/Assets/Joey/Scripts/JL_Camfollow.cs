using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JL_Camfollow : MonoBehaviour
{
    public GameObject PC;
    // Use this for initialization
    void Start()
    {
        PC = GameObject.Find("PC");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(PC.transform.position.x, 20, PC.transform.position.z);
    }
}
