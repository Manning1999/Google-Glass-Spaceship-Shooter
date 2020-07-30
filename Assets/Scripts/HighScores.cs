using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScores : MonoBehaviour
{

    private List<int> highscores = new List<int>();


    private void AddScore(int newScore)
    {

        highscores.Add(newScore);
        highscores.Sort();

        if(highscores.Capacity > 5) highscores.Remove(5);

    }

    public void SaveHighscores()
    {

    }

    public void LoadHighscores()
    {

    }
}
