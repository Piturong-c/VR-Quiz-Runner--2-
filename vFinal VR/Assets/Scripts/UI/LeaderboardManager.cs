using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public int score = 0;
    public int highscore = 0;
    public static LeaderboardManager self;
    public TMP_Text __SCORE;
    public TMP_Text HIGH__SCORE;
    public TMP_Text HIGH___SCORE;
    void Awake()
    {
        if (self == null) self = this;
        __SCORE = GameObject.Find("__SCORE").GetComponent<TMP_Text>();
        if(GameObject.Find("HIGH__SCORE") != null)
        HIGH__SCORE = GameObject.Find("HIGH__SCORE").GetComponent<TMP_Text>();
        if(GameObject.Find("HIGH__SCORE") != null)
        HIGH___SCORE = GameObject.Find("HIGH__SCORE").GetComponent<TMP_Text>();
    }

    void Start()
    {
        highscore = getHighScore();
        HIGH__SCORE.text = $"Your high score is {highscore}.";
    }
    public int getHighScore()
    {
        if (!Player.self.SpeedMode && Player.self != null)
        {
            if (PlayerPrefs.HasKey("HIGHSCORE"))
            {
                Debug.Log("Successfully Get score!");
                return PlayerPrefs.GetInt("HIGHSCORE");
            }
        }
        else
        {
            if (PlayerPrefs.HasKey("HIGHMETER"))
            {
                Debug.Log("Successfully Get score!");
                return PlayerPrefs.GetInt("HIGHMETER");
            }
        }
        Debug.LogWarning("HIGHSCORE hadn't been set!");
        return 0;
    }

    public void setHighScore(int score)
    {
        int highScore = getHighScore();
        if (score >= highScore)
        {
            if (!Player.self.SpeedMode)
                PlayerPrefs.SetInt("HIGHSCORE", score);
            else
                PlayerPrefs.SetInt("HIGHMETER", score);
        }
    }
    
    public void UpdateScore()
    {
        __SCORE.text = $"Score {score}";
        setHighScore(score);
        highscore = getHighScore();
        if (highscore == score)
            __SCORE.text = $"<color=yellow>Score {score}(!)</color>";

        HIGH__SCORE.text = $"High Score {getHighScore()}";
    }
}
