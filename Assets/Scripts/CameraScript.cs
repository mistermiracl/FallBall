using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
    //const int PIXELS_TO_UNITS = 30;
    Vector2 desiredRes = new Vector2(1024f, 768f);
    float camera_speed;
    Transform player;
    Collider2D cameraCollider;
    Collider2D endCollider;
    GameObject pausePanel;
    GameObject canvas;
    void Start()
    {
        // var res = Screen.currentResolution;
        // float desiredRatio = desiredRes.x / desiredRes.y;
        //float currentRatio = (float)Screen.width / (float)Screen.height;

        // if(currentRatio >= desiredRatio)
        //     Camera.main.orthographicSize = desiredRes.y / 4 / PIXELS_TO_UNITS;
        // else
        // {
        //     float differenceInSize = desiredRatio / currentRatio;
        //     Camera.main.orthographicSize = desiredRes.y / 4 / PIXELS_TO_UNITS * differenceInSize;
        // }
        //Screen.SetResolution(1366, 768, true);

        //NIIIICE
#if !UNITY_EDITOR
        //Camera.main.orthographicSize = (float)(desiredRes.y / 2) / 95f;
        //var coll = Camera.main.GetComponent<BoxCollider2D>();
        //coll.size = new Vector2(coll.size.x, (Camera.main.orthographicSize * 2) + 2f);
#endif

        player = GameObject.FindGameObjectWithTag("Player").transform;
        cameraCollider = GetComponent<Collider2D>();
        endCollider = GameObject.Find("end").GetComponent<Collider2D>();
        canvas = GameObject.Find("Canvas");
        pausePanel = canvas.transform.Find("PausePanel").gameObject;
        pausePanel.SetActive(false);

        if (Singleton.Instance.shouldLoad)
        {
            player.transform.position = new Vector2(Singleton.Instance.currentPlayer.posX, Singleton.Instance.currentPlayer.posY);
            transform.position = new Vector3(transform.position.x, Singleton.Instance.currentPlayer.posY, -10);
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        // else
        // {
        //     //RESET PREVIOUSLY LOADED PLAYER VALUES, THIS IS BUGGY SINCE IT RESETS THE SCORE AS WELL
        //     if(Singleton.Instance.currentPlayer.playerName != "")
        //         Singleton.Instance.currentPlayer = new PlayerData.Player();
        // }

    }
    void Update()
    {
        if (!Physics2D.IsTouching(cameraCollider, endCollider))
        {//IF IT ISNT TOUCHING THE END GAMOBJECT
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 1:
                    camera_speed = 2f * Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, transform.position.y - camera_speed, -10);
                    break;
                case 2:
                    camera_speed = 2.2f * Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, transform.position.y + camera_speed, -10);
                    break;
                case 3:
                    camera_speed = 2.4f * Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, transform.position.y - camera_speed, -10);
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            bool otherActivated = false;
            for (int i = 0; i < canvas.transform.childCount; i++)
            {
                if (canvas.transform.GetChild(i).name == pausePanel.name)
                    break;
                else if (canvas.transform.GetChild(i).gameObject.activeSelf && canvas.transform.GetChild(i).name.ToLower().Contains("panel"))
                    otherActivated = true;
            }

            if (!otherActivated)
            {

                if (pausePanel.activeSelf)
                {
                    Time.timeScale = 1;
                    pausePanel.SetActive(false);
                }
                else
                {
                    if (Time.timeScale != 0)
                    {
                        Time.timeScale = 0;
                        pausePanel.SetActive(true);
                    }
                }
            }
        }
    }

    public void Exit()
    {
        Singleton.Instance.currentPlayer.playerScore = 0;
        Singleton.Instance.playerSpeed = 10f;
        Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }

    public void SaveGame()
    {

        Singleton.Instance.currentPlayer.level = SceneManager.GetActiveScene().buildIndex;
        Singleton.Instance.currentPlayer.posX = player.position.x;
        Singleton.Instance.currentPlayer.posY = player.position.y;
        Singleton.Instance.BinarySave();


        Singleton.Instance.currentPlayer.playerScore = 0;
        Singleton.Instance.playerSpeed = 10f;
        Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }
}


