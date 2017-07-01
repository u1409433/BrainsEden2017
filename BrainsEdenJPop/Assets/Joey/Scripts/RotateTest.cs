using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    public bool BL_BobRight;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BL_BobRight)
        {
            gameObject.transform.Rotate(new Vector3(0, 0, -0.5f));
            if (transform.rotation.eulerAngles.z <= -10)
            {
                BL_BobRight = false;
            }
        }
        else
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 0.5f));
            if (transform.rotation.eulerAngles.z >= 10)
            {
                BL_BobRight = true;
            }
        }
    }
}
