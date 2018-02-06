using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class Game
{
    public const int MENU_INDEX = 0;
    public const int LEVEL_1_INDEX = 1;
    public const int LEVEL_2_INDEX = 2;
    public const int LEVEL_3_INDEX = 3;

    public const string GET_ALL_PLAYERS = "getAllPlayers";
    public const string GET_PLAYER = "getPlayer";
    public const string NEW_PLAYER = "newPlayer";

    //public delegate void GetPlayers(List<PlayerData.Player> players);
    public delegate void Executor<T>(T param);
    public delegate void Executor();
    const string BASE_URL = "http://127.0.0.1:8084/FallBallRS/api";


    static UnityWebRequest request;
    public static IEnumerator doGet(string url, Executor onLoad, Executor<List<PlayerData.Player>> onFinish)
    {
        onLoad();
        request = UnityWebRequest.Get(string.Format("{0}/{1}", BASE_URL, url));
        yield return request.Send();
        string json = "{ \"values\": " + request.downloadHandler.text + " }";
        Debug.Log(json);
        var obj = JsonUtility.FromJson<Wrapper<List<PlayerData.Player>>>(json);
        onFinish(obj.values);
    }

    /*public static IEnumerator doGet(){
        string requestURL = string.Format("{0}/{1}", BASE_URL, GET_ALL_PLAYERS);
        Debug.Log(requestURL);
        request = UnityWebRequest.Get(requestURL);
        yield return request.Send();
        Debug.Log(request.downloadHandler.text);
    }*/

    public static IEnumerator doPost(string url, PlayerData.Player body, Executor onLoad = null, Executor onFinish = null)
    {
        if(onLoad != null) onLoad();
        string json = JsonUtility.ToJson(body);
        Debug.Log(json);
        //FOR SOME REASON IN ORDER TO MAKE A POST / PUT REQUEST WITH A JSON AS BODY WE HAVE TO USE PUT THEN IF POST CHANGE THE METHOD MANUALLY
        //IT SEEMS TO BE BETTER IF IT IS SENT AS RAW BYTES RATHER THAN A STRING
        request = UnityWebRequest.Put(string.Format("{0}/{1}", BASE_URL, url), Encoding.UTF8.GetBytes(json));
        request.SetRequestHeader("Content-Type", "application/json");
        request.method = UnityWebRequest.kHttpVerbPOST;
        //request.uploadHandler.contentType = "application/json";
        yield return request.Send();
        Debug.Log(request.responseCode + " " + request.error + " " + request.downloadHandler.text);
        if(onFinish != null) onFinish();
    }

    struct Wrapper<T>
    {
        public T values;
    }

}