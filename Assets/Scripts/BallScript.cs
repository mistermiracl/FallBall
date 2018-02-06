using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallScript : MonoBehaviour
{
    const float JUMP_FORCE = 10f;
    const float BASE_SPEED = 10f;
    GameObject canvas;
    Rigidbody2D playerbody;
    GameObject panel;
    GameObject retryPanel;
    InputField inputName;
    Text lostMessage;

    Button submitButton;
    void Start()
    {

        playerbody = GetComponent<Rigidbody2D>();
        canvas = GameObject.Find("Canvas");
        panel = canvas.transform.Find("Panel").gameObject;
        submitButton = panel.transform.Find("Button").GetComponent<Button>();
        retryPanel = canvas.transform.Find("RetryPanel").gameObject;
        inputName = panel.transform.Find("InputField").GetComponent<InputField>();
        lostMessage = ((GameObject)Resources.Load("Prefabs/Text")).GetComponent<Text>();
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(string.Format("Sprites/{0}", Singleton.Instance.playerData.currentSprite));
        panel.SetActive(false);
    }

    void Update()
    {
        playerbody.velocity = new Vector2(Input.GetAxis("Horizontal") * Singleton.Instance.playerSpeed, playerbody.velocity.y);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -6.98f, 6.89f), transform.position.y, 0);

        if (Physics2D.IsTouchingLayers(playerbody.GetComponent<Collider2D>(), LayerMask.GetMask("Floor")))
        {
            Debug.Log("TOUCHING GROUND");
            if (Input.GetKeyDown(KeyCode.Space) && Singleton.Instance.keysEnabled)
                playerbody.velocity = new Vector2(playerbody.velocity.x, JUMP_FORCE);
        }
    }

    IEnumerator OnBecameInvisible()
    {
        yield return new WaitForSeconds(2); ;
        if (!GetComponent<Renderer>().isVisible)
        {
            //SAVE CURRENT SCORE AND PLAYER NAME
            // Singleton.Instance.playerChart.Add(new PlayerData("some player", Singleton.Instance.totalScore));
            // Singleton.Instance.BinarySave();

            //ScoreScript.points *= 0;
            Debug.Log("NOT VISIBLE");
            //Singleton.ShowUIText("lostmessage", "YOU LOST...", 40, new Vector2(400, 50), canvas.transform);
            Text instatiated = Instantiate(lostMessage, canvas.transform.position, lostMessage.transform.localRotation, canvas.transform);
            lostMessage.text = "AWW, YOU LOST";
            lostMessage.alignment = TextAnchor.MiddleCenter;
            //PAUSE GAME
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(3);//this does not depend on time scale
            Debug.Log("EXITING");
            Destroy(instatiated.gameObject);

            //Singleton.Instance.BinaryLoad();

            if (Singleton.Instance.playerData.playerChart.Count > 0)
            {
                if (Singleton.Instance.currentPlayer.playerScore >
                Singleton.Instance.playerData.playerChart.OrderByDescending(player => player.playerScore).Take(8).Last().playerScore)
                    panel.SetActive(true);
                else
                {
                    Singleton.Instance.BinarySave();
                    //RESET VALUES 
                    //ResetValues();

                    retryPanel.SetActive(true);
                }
            }
            else
            {
                if (Singleton.Instance.currentPlayer.playerScore > 0)
                    panel.SetActive(true);
                else
                    retryPanel.SetActive(true);
            }
        }
    }


    public void Submit()
    {
        submitButton.interactable = false;
        //SAVE CURRENT SCORE AND PLAYER NAME
        Singleton.Instance.currentPlayer.playerName = string.IsNullOrEmpty(inputName.text) ? "Some player" : inputName.text;
        Singleton.Instance.BinarySave();
        //DONT FORGET THE IENUMATOR METHOD TO ACCESS THE WEB NEEDS TO BE WRAPPED INSIDE A COROUTINE
        StartCoroutine(Game.doPost(Game.NEW_PLAYER, Singleton.Instance.currentPlayer, null, delegate
        {
            print("Player Sent!");
            retryPanel.SetActive(true);
            panel.SetActive(false);
        }));
    }

    public void Retry()
    {
        ResetValues();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        ResetValues();
        SceneManager.LoadScene(Game.MENU_INDEX);
    }

    void ResetValues()
    {
        Time.timeScale = 1;
        Singleton.Instance.currentPlayer.playerScore = 0;
        Singleton.Instance.playerSpeed = BASE_SPEED;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManagerScript.PlayAudio();
    }

}





