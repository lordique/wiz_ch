using System.Net;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Collections.Generic;
using UnityEngine;


public class InitResponse
{
    public string game_id { get; set; }
    public string _id { get; set; }
    public string status { get; set; } // "new game started"
}

public class AIMoveResponse
{
    // curl -X POST --header "Content-Type: application/json" -d '{"game_id":"5eb057373c58dc0014d75b89"}' 'http://chess-api-chess.herokuapp.com/api/v1/chess/one/move/ai'
    public string from { get; set; }  // "a1"
    public string to { get; set; }
    public string _id { get; set; }
    public string status { get; set; } // "AI moved!"
}

public class PlayerMoveResponse
{
    // curl -X POST --header "Content-Type: application/json" -d '{"game_id":"5eb057373c58dc0014d75b89","from":"b7","to":"b5"}' 'http://chess-api-chess.herokuapp.com/api/v1/chess/one/move/player'
    public string _id { get; set; }
    public string status { get; set; } //  "status": "figure moved", else "error: invalid move!"

}


public class GetMoveOptionsResponse
{
    // curl -X POST --header "Content-Type: application/json" -d '{"game_id":"5eb057373c58dc0014d75b89","position":"b7"}' 'http://chess-api-chess.herokuapp.com/api/v1/chess/one/moves'
    public string _id { get; set; }
    public List<string> moves { get; set; }
}

public class ChessAPI
{
    private string URL = "http://chess-api-chess.herokuapp.com/api/v1/chess/one";
    private string game_id;
    private ASCIIEncoding encoding = new ASCIIEncoding();

    public ChessAPI() {
        game_id = InitGame();
    }
    private string InitGame()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
        {
            // Deserialization from JSON  
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(InitResponse));
            InitResponse info = (InitResponse)deserializer.ReadObject(ms);
            Debug.Log("GameId: " + info.game_id);
            // TODO: validate status
            return info.game_id;
        };
    }

    public AIMoveResponse AiMove()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL+"/move/ai");
        request.Method = "POST";

        byte[] byte1 = encoding.GetBytes("game_id=" + game_id);

        // Set the content type of the data being posted.
        request.ContentType = "application/x-www-form-urlencoded";

        // Set the content length of the string being posted.
        request.ContentLength = byte1.Length;
        Stream newStream = request.GetRequestStream();

        newStream.Write(byte1, 0, byte1.Length);
        newStream.Close();

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
        {
            // Deserialization from JSON  
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(AIMoveResponse));
            AIMoveResponse info = (AIMoveResponse)deserializer.ReadObject(ms);
            // TODO: status validation
            Console.Write("Move: " + info.from + " to " + info.to);
            return info;
        };
    }

    public PlayerMoveResponse PlayerMove(string fromPosition, string toPosition)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL+"/move/player");
        request.Method = "POST";

        byte[] byte1 = encoding.GetBytes("game_id=" + game_id + "&from=" + fromPosition + "&to=" + toPosition);

        // Set the content type of the data being posted.
        request.ContentType = "application/x-www-form-urlencoded";

        // Set the content length of the string being posted.
        request.ContentLength = byte1.Length;
        Stream newStream = request.GetRequestStream();

        newStream.Write(byte1, 0, byte1.Length);

        newStream.Close();

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
        {
            // Deserialization from JSON  
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(PlayerMoveResponse));
            // TODO: status validation
            PlayerMoveResponse info = (PlayerMoveResponse)deserializer.ReadObject(ms);
            return info;
        };
    }

    public GetMoveOptionsResponse GetMoveOptions(string fromPosition)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL + "/moves");
        request.Method = "POST";
        byte[] byte1 = encoding.GetBytes("game_id=" + game_id +"&position=" + fromPosition);

        // Set the content type of the data being posted.
        request.ContentType = "application/x-www-form-urlencoded";

        // Set the content length of the string being posted.
        request.ContentLength = byte1.Length;
        Stream newStream = request.GetRequestStream();

        newStream.Write(byte1, 0, byte1.Length);
        newStream.Close();

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
        {
            // Deserialization from JSON  
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(GetMoveOptionsResponse));
            // TODO: status validation
            GetMoveOptionsResponse info = (GetMoveOptionsResponse)deserializer.ReadObject(ms);
            Debug.Log(info);
            return info;
        };
    }
}


