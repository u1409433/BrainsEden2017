using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class JL_LevelManager : MonoBehaviour
{
    public List<GameObject> LS_GO_Interactables;
    // Use this for initialization

    void Start()
    {
        SetupLists();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetupLists()
    {
        LS_GO_Interactables = new List<GameObject>();

        foreach (GameObject interactable in GameObject.FindGameObjectsWithTag("Interactable"))
        {
            LS_GO_Interactables.Add(interactable);
        }
    }

    public void WinState()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void LoseState()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }
}
