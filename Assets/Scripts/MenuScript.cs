using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    void Start()
    {
		// string json = JsonUtility.ToJson(new PlayerData {
		// 	ball2Enabled = false,
		// 	ball3Enabled = true,
		// 	currentSprite = "none",
		// 	leve2Enabled = false,
		// 	leve3Enabled = true,
		// 	playerChart = new List<PlayerData.Player>{
		// 		new PlayerData.Player("some player", 21, true, 32.2f, 43.2f, 1),
		// 	}
		// }, true);

		// print (json);

        Singleton.Instance.BinaryLoad();

        //buttons = GetComponents<GameObject> ();
        GameObject scoresPanel = transform.Find("ScoresPanel").gameObject;

        GameObject playOptionsPanel = transform.Find("PlayOptions").gameObject;
        Button playButton = playOptionsPanel.transform.Find("Play").GetComponent<Button>();
        Button ChooseLevelButton = playOptionsPanel.transform.Find("ChooseLevel").GetComponent<Button>();
        Button loadGameButton = playOptionsPanel.transform.Find("Load").GetComponent<Button>();
        Button exitPlayOptions = playOptionsPanel.transform.Find("Back").GetComponent<Button>();

        GameObject levelPanel = playOptionsPanel.transform.Find("LevelPanel").gameObject;
        Button level1Button = levelPanel.transform.Find("Level1").GetComponent<Button>();
        Button level2Button = levelPanel.transform.Find("Level2").GetComponent<Button>();
        Button level3Button = levelPanel.transform.Find("Level3").GetComponent<Button>();
        Button exitLevelOptions = levelPanel.transform.Find("Back").GetComponent<Button>();

        GameObject ballOptionsPanel = transform.Find("BallOptions").gameObject;
        Button bronzeBall = ballOptionsPanel.transform.Find("Bronze").GetComponent<Button>();
        Button silverBall = ballOptionsPanel.transform.Find("Silver").GetComponent<Button>();
        Button goldBall = ballOptionsPanel.transform.Find("Gold").GetComponent<Button>();
        Button exitBallOptions = ballOptionsPanel.transform.Find("BackBallOptions").GetComponent<Button>();

        scoresPanel.SetActive(false);
        playOptionsPanel.SetActive(false);
        levelPanel.SetActive(false);
        ballOptionsPanel.SetActive(false);

        transform.Find("Start").GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("CHOOSING LEVEL");
            if(Singleton.Instance.currentPlayer.posX == 0 && Singleton.Instance.currentPlayer.posY == 0)
                loadGameButton.interactable = false;
            playOptionsPanel.SetActive(true);
        });

        transform.Find("Scores").GetComponent<Button>().onClick.AddListener(delegate
            {
                Debug.Log("Loading Scores");
                scoresPanel.SetActive(true);
            });

        transform.Find("Pick").GetComponent<Button>().onClick.AddListener(() =>
        {
            if(!Singleton.Instance.playerData.ball2Enabled)
                silverBall.interactable = false;
            if(!Singleton.Instance.playerData.ball3Enabled)
                goldBall.interactable = false;

            print(Singleton.Instance.playerData.currentSprite);
            switch (Singleton.Instance.playerData.currentSprite)
            {
                case "bronze":
                    bronzeBall.transform.Find("Text").GetComponent<Text>().color = Color.blue;
                    break;
                case "silver":
                    silverBall.transform.Find("Text").GetComponent<Text>().color = Color.blue;
                    break;
                case "gold":
                    goldBall.transform.Find("Text").GetComponent<Text>().color = Color.blue;
                    break;
                default:
                    bronzeBall.transform.Find("Text").GetComponent<Text>().color = Color.blue;
                    break;
            }

            ballOptionsPanel.SetActive(true);

        });

        transform.Find("Exit").GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("EXITED");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        });

        playButton.onClick.AddListener(() =>
        {
            Singleton.Instance.currentPlayer = new PlayerData.Player();
            SceneManager.LoadScene(1);
        });

        loadGameButton.onClick.AddListener(() => {
            //MAKE SURE EVERYTHING IS LOADED
            Singleton.Instance.BinaryLoad();
            SceneManager.LoadScene(Singleton.Instance.currentPlayer.level);
            //PLAYER POSITION LOADED IN CAMERA
            Singleton.Instance.shouldLoad = true;
        });

        ChooseLevelButton.onClick.AddListener(() =>
        {
            if(!Singleton.Instance.playerData.leve2Enabled)
                level2Button.interactable = false;
            if(!Singleton.Instance.playerData.leve3Enabled)
                level3Button.interactable = false;

            levelPanel.SetActive(true);
        });

        exitPlayOptions.onClick.AddListener(() =>
        {
            playOptionsPanel.SetActive(false);
        });

        level1Button.onClick.AddListener(() =>
        {
            Singleton.Instance.currentPlayer = new PlayerData.Player();
            SceneManager.LoadScene(1);
        });

        level2Button.onClick.AddListener(() =>
        {
            Singleton.Instance.currentPlayer = new PlayerData.Player();
            SceneManager.LoadScene(2);
        });

        level3Button.onClick.AddListener(() =>
        {
            Singleton.Instance.currentPlayer = new PlayerData.Player();
            SceneManager.LoadScene(3);
        });

        exitLevelOptions.onClick.AddListener(() =>
        {
            levelPanel.SetActive(false);
        });

        bronzeBall.onClick.AddListener(() =>
        {
            Singleton.Instance.playerData.currentSprite = "bronze";

            bronzeBall.transform.Find("Text").GetComponent<Text>().color = Color.blue;
            silverBall.transform.Find("Text").GetComponent<Text>().color = Color.white;
            goldBall.transform.Find("Text").GetComponent<Text>().color = new Color(255, 255, 255);

            Singleton.Instance.BinarySave();


        });
        silverBall.onClick.AddListener(() =>
        {
            Singleton.Instance.playerData.currentSprite = "silver";

            bronzeBall.transform.Find("Text").GetComponent<Text>().color = new Color(255, 255, 255);
            silverBall.transform.Find("Text").GetComponent<Text>().color = Color.blue;
            goldBall.transform.Find("Text").GetComponent<Text>().color = new Color(255, 255, 255);

            Singleton.Instance.BinarySave();

        });
        goldBall.onClick.AddListener(() =>
        {
            Singleton.Instance.playerData.currentSprite = "gold";

            bronzeBall.transform.Find("Text").GetComponent<Text>().color = new Color(255, 255, 255);
            silverBall.transform.Find("Text").GetComponent<Text>().color = new Color(255, 255, 255);
            goldBall.transform.Find("Text").GetComponent<Text>().color = Color.blue;

            Singleton.Instance.BinarySave();

        });

        exitBallOptions.onClick.AddListener(() =>
        {
            ballOptionsPanel.SetActive(false);
        });

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }
    }
}
