using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class JL_UIManager : MonoBehaviour
{
    public Image ShrineRed;
    public Image ShrineGreen;
    public Image ShrineBlue;

    public string ST_Minutes;
    public string ST_Seconds;

    public float timer = 300;

    public Text UI_Time;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        ST_Minutes = Mathf.Floor(timer / 60).ToString("00");
        ST_Seconds = (timer % 60).ToString("00");

        UI_Time.text = ST_Minutes + " : " + ST_Seconds;

        if (timer <= 0)
        {
            GameObject.Find("LevelManager").GetComponent<JL_LevelManager>().LoseState();
        }
    }

    public void StartButton()
    {
        GameObject tAudio = GameObject.Find("Audio");
        Destroy(tAudio);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void PlaceRelic(int vShrine)
    {
        switch (vShrine)
        {
            case 1:
                ShrineRed.color = Color.red;
                break;
            case 2:
                ShrineGreen.color = Color.green;
                break;
            case 3:
                ShrineBlue.color = Color.blue;
                break;
            default:
                break;
        }
    }

    public void ExitButton()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
