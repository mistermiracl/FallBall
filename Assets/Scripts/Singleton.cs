using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class Singleton : MonoBehaviour
{
    private string path;
    [HideInInspector] public PlayerData.Player currentPlayer = new PlayerData.Player();
    [HideInInspector] public bool keysEnabled = true;
    public bool shouldLoad = false;
    public float playerSpeed = 10f;
    [HideInInspector] public PlayerData playerData;// = new PlayerData();
    private static Singleton instance;

    private Singleton()
    {
    }

    [HideInInspector]
    public static Singleton Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogWarning("CREANDO SINGLETON");
                GameObject owner = new GameObject("SingletonHolder");
                owner.AddComponent<Singleton>();//WHEN WE ADD SINGLETON TO THE GAMEOBJECT IS AWAKE METHOD IS EXECUTED GIVING INSTANCE A VALUE
            }
            Debug.Log(instance == null ? "is null" : "using Singleton");//THIS WILL BE PRINTED WHENEVER THE SINGLETON IS ACCESED, IT CAN APPEAR MULTIPLE TIMES SINCE IT CAN HAVE MULTIPLE SOURCES
            return instance;
        }
    }
    void Awake()
    {
        path = Application.persistentDataPath + "/playerinfo.info";
        print(path);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.LogWarning("Destroying Singleton");
            Destroy(this);
        }
    }
    public bool BinarySave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        PlayerData data;

        if (File.Exists(path))
        {
            using (FileStream fs = File.Open(path, FileMode.Open))//WE ADD THE CURRENT NEW SCORES TO THEN NEWLY CREATED SCORES
            {
                data = (PlayerData)bf.Deserialize(fs);
                //data.PlayerChart.ForEach(p => p.lastPlayer = false);
                
                //SET ALL OTHER PLAYER TO NOT BE LAST PLAYER BUT THE LAST ONE
                for (int i = 0; i < data.playerChart.Count; i++)
                {
                    PlayerData.Player p = data.playerChart[i];
                    p.lastPlayer = false;
                    data.playerChart[i] = p;
                }

                this.currentPlayer.lastPlayer = true;
                data.playerChart.Add(this.currentPlayer);

                this.playerData.playerChart = data.playerChart;
            }
            using (FileStream fs2 = File.Create(path))
            {
                bf.Serialize(fs2, this.playerData);
                Debug.Log(path);
                Debug.Log("Saving data to already created file");
                //this.playerChart.Clear();
            }
            return true;
        }
        else
        {
            using (FileStream fs = File.Create(path))
            {
                //INSTATIATE SINCE THERE IS NO DATA YET
                //playerData = new PlayerData();
                this.playerData.playerChart.Add(this.currentPlayer);
                bf.Serialize(fs, this.playerData);
                Debug.Log(path);
                Debug.Log("Creating save file");
            }
            return true;
        }
    }
    public bool BinaryLoad()
    {
        if (File.Exists(path))
        {
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                this.playerData = (PlayerData)bf.Deserialize(fs);
                bool found = this.playerData.playerChart.Where(p => p.lastPlayer == true).Count() > 0;
                if (found)
                {
                    this.currentPlayer = this.playerData.playerChart.Where(p => p.lastPlayer == true).First();
                    //this.currentPlayer = this.playerData.PlayerChart.Last();
                }

                //print(this.currentPlayer.posX + " " + this.currentPlayer.posY);
                Debug.Log("Loading player data");
            }
            return true;
        }
        else
        {
            //INSTATIATE TO GET DEFAULT VALUES, THIS IS THE ONLY TIME PLAYER DATA IS INSTATITED SINCE FROM THIS POINTS IT'S ALWAYS GONNA BE LOADED
            this.playerData = new PlayerData();
            Debug.LogWarning("No existen datos que cargar");
            return false;
        }
    }



    [System.Obsolete("Method ShowUIText is deprecated, use Instatiate(with a prefab) instead")]
    public static void ShowUIText(string name, string message, int fontsize, Vector2 dimensions, Transform parent)
    {
        GameObject nullgameobject = null;

        if (nullgameobject != null)
        {
            return;
        }
        else
        {
            nullgameobject = new GameObject(name);
            Text finalmessage = nullgameobject.AddComponent<Text>();
            finalmessage.text = message;
            finalmessage.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            finalmessage.fontSize = fontsize;
            nullgameobject.GetComponent<RectTransform>().sizeDelta = dimensions;

            nullgameobject.transform.SetParent(parent);
            nullgameobject.transform.position = parent.position;
            Debug.Log(finalmessage.text + " " + finalmessage.canvas.name);
        }
    }
}

[Serializable]
public class PlayerData
{
    [Serializable]
    public struct Player
    {   
        public int playerId;
        public string playerName;
        public float playerScore;
        public bool lastPlayer;
        public float posX;
        public float posY;
        public int level;

        public Player(string playerName, float playerScore, bool lastPlayer, float posX, float posY, int level, int playerId = 0)
        {
            this.playerId = playerId;
            this.playerName = playerName;
            this.playerScore = playerScore;
            this.lastPlayer = lastPlayer;
            this.posX = posX;
            this.posY = posY;
            this.level = level;
        }
    }

	public List<Player> playerChart; //{ get; set; }//UNITY CANNOT SERIALIZE PROPERTIES FOR SOME UNKNOWN REASON, CHECK DOCS
	public bool leve2Enabled;// { get; set; }
	public bool leve3Enabled;// { get; set; }
	public bool ball2Enabled;// { get; set; }
	public bool ball3Enabled;// { get; set; }
	public string currentSprite;// { get; set; }
    public PlayerData()
    {
        this.playerChart = new List<Player>();//INITIALIZE ATTRIBUTES FOR DEFAULT BEHAVIOUR
        this.currentSprite = "bronze";
    }
}