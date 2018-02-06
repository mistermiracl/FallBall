using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HighScoresScript : MonoBehaviour
{
    string scores;
    Text highScores;
    Image local;
    Image global;

    private Color baseButtonColor;
    void Start()
    {
        scores = "";
        local = transform.Find("Local").GetComponent<Image>();
        global = transform.Find("Global").GetComponent<Image>();
        this.baseButtonColor = local.color;
        //highScores = transform.Find("Scores").GetComponent<Text>();
        highScores = transform.Find("ScrollView").Find("Viewport").Find("Content").GetComponent<Text>();
        //Singleton.Instance.BinaryLoad();
        //&& Singleton.Instance.playerData.playerChart.Last().playerScore > 0 OLD BUG
        // if (Singleton.Instance.playerData.playerChart.Count > 0)
        // {
        //     foreach (PlayerData.Player player in Singleton.Instance.playerData.playerChart.OrderByDescending(player => player.playerScore).Take(8))
        //     {
        //         if (player.playerScore > 0)
        //             scores += string.Format("Name: {0} - Score: {1}\n", player.playerName, player.playerScore);
        //     }
        //     highScores.text = scores;
        // }
        // else
        // {
        //     highScores.text = "There are no high scores yet";
        // }
        LoadLocal();
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }

    public void LoadLocal()
    {
        local.color = Color.black;
        global.color = this.baseButtonColor;
        scores = "\n";
        if (Singleton.Instance.playerData.playerChart.Count > 0)
        {
            foreach (PlayerData.Player player in Singleton.Instance.playerData.playerChart.OrderByDescending(player => player.playerScore).Take(8))
            {
                if (player.playerScore > 0)
                    scores += string.Format("Name: {0} - Score: {1}\n", player.playerName, player.playerScore);
            }
            highScores.text = scores;
        }
        else
        {
            highScores.text = "\nThere are no Local high scores yet";
        }
    }

    public void LoadGlobal()
    {
        //print("Hello?");
        // Game.doGet(Game.GET_ALL_PLAYERS, delegate(List<PlayerData.Player> players){
        //     print("executing");
        //     foreach (PlayerData.Player p in players){
        //         print(p.playerName);
        //     }
        // });
        global.color = Color.black;
        local.color = this.baseButtonColor;
        scores = "\n";
        StartCoroutine(Game.doGet(Game.GET_ALL_PLAYERS,
        delegate
        {
            highScores.text = "\nLoading...";
        },
        delegate (List<PlayerData.Player> players)
        {
            if (players.Count > 0)
            {
                foreach (var p in players)
                    scores += string.Format("Name: {0} - Score: {1}\n", p.playerName, p.playerScore);
                highScores.text = scores;
            }
            else
            {
                highScores.text = "\nThere are no Global high scores yet";
            }
        }));
    }
}
