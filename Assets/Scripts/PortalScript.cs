using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalScript : MonoBehaviour
{
    Text message;
    GameObject canvas;
    Transform player;
    AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canvas = GameObject.Find("Canvas");
        message = (Resources.Load("Prefabs/Text") as GameObject).GetComponent<Text>();
    }
    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                Singleton.Instance.playerData.leve2Enabled = true;
                message.text = "Loading...";
                Instantiate(message, canvas.transform.position, message.transform.localRotation, canvas.transform);
                //Singleton.ShowUIText("nextlevel", "Loading...", 40, new Vector2(400, 50), canvas.transform);
                //MAKE THE GAMEOBJECT INVISIBLE WITHOUT DEACTIVATING OR DESTROYING IT
                GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().enabled = false;
                Time.timeScale = 0;
                yield return new WaitForSecondsRealtime(3);
                Time.timeScale = 1;
                SceneManager.LoadScene(2);
                break;
            case 2:
                Singleton.Instance.playerData.leve3Enabled = true;
                message.text = "Loading...";
                Instantiate(message, canvas.transform.position, message.transform.localRotation, canvas.transform);
                //Singleton.ShowUIText("endmessage", "In Development...", 40, new Vector2(400, 50), canvas.transform);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().enabled = false;
                Time.timeScale = 0;
                yield return new WaitForSecondsRealtime(3);
                Time.timeScale = 1;
                SceneManager.LoadScene(3);
                break;
            case 3:
                message.text = "YOU WON!";
                Instantiate(message, canvas.transform.position, message.transform.localRotation, canvas.transform);
                //Singleton.ShowUIText("endmessage", "In Development...", 40, new Vector2(400, 50), canvas.transform);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().enabled = false;
                Time.timeScale = 0;
                yield return new WaitForSecondsRealtime(3);
                Time.timeScale = 1;
                SceneManager.LoadScene(0);
                break;
        }
        //EditorApplication.isPlaying = false;
    }

    void Update()
    {
        if((transform.position - player.position).magnitude <= 5){
            if(!audio.isPlaying)
                audio.Play();
        }
        else
            audio.Stop();
    }
}
