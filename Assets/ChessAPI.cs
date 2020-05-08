using UnityEngine;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class AIMoveResponse
{
    public string from { get; set; }
    public string to { get; set; }
}

public class ChessAPI : MonoBehaviour
{
    private string stockfishPath = "C:\\Users\\lesfo\\Downloads\\stockfish-11-win\\stockfish-11-win\\Windows\\stockfish_20011801_x64";
    private Process stockfishProcess;
    private string moves = "";
    private string position_command = "position startpos moves ";
    private string go_command = "go";
    private bool passNextOutput = false;
    private ChessBoard chessboard;

    public ChessAPI(ChessBoard chessBoard) {
         InitGame();
         chessboard = chessBoard;
    }

    private void SendLine(string command)
    {
        stockfishProcess.StandardInput.WriteLine(command);
        stockfishProcess.StandardInput.Flush();
    }

    private void OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        string text = e.Data;
        UnityEngine.Debug.Log("[UCI] " + text);
        if (text.Contains("bestmove"))
        {
            UnityEngine.Debug.Log("detected ai move!");
            ProcessAiMove(text);
        }
    }
    private void InitGame()
    {
        ProcessStartInfo si = new ProcessStartInfo()
        {
            FileName = stockfishPath,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        };

        stockfishProcess = new Process();
        stockfishProcess.StartInfo = si;

        stockfishProcess.OutputDataReceived += new DataReceivedEventHandler(OutputDataReceived);

        stockfishProcess.Start();
        stockfishProcess.BeginErrorReadLine();
        stockfishProcess.BeginOutputReadLine();

        SendLine("uci");
        SendLine("isready");
        SendLine("ucinewgame");
    }

    public void RequestAiMove()
    {
        string line = position_command + moves;
        SendLine(line);
        passNextOutput = true;
        SendLine(go_command);

    }

    public void ProcessAiMove(string engineOutput)
    {
        passNextOutput = false;
        string[] outputSplit = engineOutput.Split(null);
        string nextMove = outputSplit[1];
        AIMoveResponse move = new AIMoveResponse();
        move.from = nextMove.Substring(0, 2);
        move.to = nextMove.Substring(2);
        AddMove(move.from, move.to);
        // signal move to board.
        chessboard.ProcessAIMove(move.from, move.to);
    }

    public void AddMove(string fromPosition, string toPosition)
    {
        moves = moves + " " + fromPosition + toPosition;
    }


    void OnDestroy()
    {
        stockfishProcess.Close();
    }
}


