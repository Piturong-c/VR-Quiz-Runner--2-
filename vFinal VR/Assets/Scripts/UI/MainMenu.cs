using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public bool isVR = true;
    public enum CameraState
    {
        QUIT = 0,
        SETTING = 1,
        PLAY = 2,
        LEADERBOARD = 3,
        NONE = -1
    }

    public CameraState state;
    public Vector3[] positions;
    public Vector3[] rotations;

    public float dt = 1f;

    private int previousState = 0;
    public Button LEADERBOARD_ANNOUNCE;
    public Button LEADERBOARD_BACK;
    public GameObject QUIT;
    public Button QUIT_YES;
    public Button QUIT_NO;
    public Button START_GAME;
    public Button LEADERBOARD;
    public Button NORMAL_MODE;
    public Button ZEN_MODE;
    public Button QUITGAME;
    public GameObject SETTING;
    public Vector3 START_GAME_POSITION;
    public Vector3 LEADERBOARD_POSITION;
    public Vector3 QUITGAME_POSITION;
    public Transform movePoint;

    void Awake()
    {
        PlayerPrefs.SetInt("HP",Player.maxPlayerHealth);
        PlayerPrefs.SetInt("SCORE",0);
        PlayerPrefs.SetInt("M",-5);
        if (PlayerPrefs.HasKey("HIGHSCORE"))
            if(GameObject.Find("MY_HIGHSCORE") != null)
            GameObject.Find("MY_HIGHSCORE").GetComponent<TMP_Text>().text = $"Your high score is {PlayerPrefs.GetInt("HIGHSCORE")}";
        if (PlayerPrefs.HasKey("HIGHMETER"))
        {
            if(GameObject.Find("MY_HIGHMETER") != null)
             GameObject.Find("MY_HIGHMETER").GetComponent<TMP_Text>().text =
                $"Your high meter is {PlayerPrefs.GetInt("HIGHMETER")}";
        }
        if(GameObject.Find("LEADERBOARD_ANNOUNCE") != null)
        LEADERBOARD_ANNOUNCE = GameObject.Find("LEADERBOARD_ANNOUNCE").GetComponent<Button>();
        if(GameObject.Find("LEADERBOARD_BACK") != null)
        LEADERBOARD_BACK = GameObject.Find("LEADERBOARD_BACK").GetComponent<Button>();

        if(GameObject.Find("QUIT") != null) QUIT = GameObject.Find("QUIT");
        if(GameObject.Find("QUIT_YES") != null) QUIT_YES = GameObject.Find("QUIT_YES").GetComponent<Button>();
        if(GameObject.Find("QUIT_NO") != null) QUIT_NO = GameObject.Find("QUIT_NO").GetComponent<Button>();
        if(GameObject.Find("START_GAME") != null) START_GAME = GameObject.Find("START_GAME").GetComponent<Button>();
        if(GameObject.Find("NORMAL_MODE") != null) NORMAL_MODE = GameObject.Find("NORMAL_MODE").GetComponent<Button>();
        if(GameObject.Find("ZEN_MODE") != null) ZEN_MODE = GameObject.Find("ZEN_MODE").GetComponent<Button>();
        if(GameObject.Find("LEADERBOARD") != null) LEADERBOARD = GameObject.Find("LEADERBOARD").GetComponent<Button>();
        if(GameObject.Find("QUITGAME") != null) QUITGAME = GameObject.Find("QUITGAME").GetComponent<Button>();
        START_GAME_POSITION = START_GAME.transform.position;
        if(GameObject.Find("LEADERBOARD") != null)
        LEADERBOARD_POSITION = LEADERBOARD.transform.position;
        QUITGAME_POSITION = QUITGAME.transform.position;
        if(GameObject.Find("UI_SETTING") != null)
        SETTING = GameObject.Find("UI_SETTING");
    }

    void Start()
    {
        if(GameObject.Find("LEADERBOARD_ANNOUNCE") != null)
        LEADERBOARD_ANNOUNCE.onClick.AddListener(() =>  SoundManager.self.PlayAudio("click_wood"));
        if(GameObject.Find("LEADERBOARD_BACK") != null)
        LEADERBOARD_BACK.onClick.AddListener(() =>
        {
            SoundManager.self.PlayAudio("click_cancel");
            state = CameraState.PLAY;
        });
        if(GameObject.Find("QUIT_NO") != null) 
        QUIT_NO.onClick.AddListener(() =>
        {
            QUIT.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InOutElastic);
            state = CameraState.PLAY;
        });
        if(GameObject.Find("QUIT_YES") != null)
        QUIT_YES.onClick.AddListener(() => Application.Quit());
        LEADERBOARD.onClick.AddListener(() =>
        {
            SoundManager.self.PlayAudio("click_wood");
            state = CameraState.LEADERBOARD;
        });

        QUITGAME.onClick.AddListener(() =>
        {
            SoundManager.self.PlayAudio("click_wood");
            state = CameraState.QUIT;
        });
        if (!isVR)
        {
            START_GAME.onClick.AddListener(() =>
            {
                StartCoroutine(Delay(0.5f, () =>
                {
                    START_GAME.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InOutElastic);
                    LEADERBOARD.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InOutElastic);
                    QUITGAME.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InOutElastic);
                    NORMAL_MODE.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InOutElastic);
                    ZEN_MODE.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InOutElastic);
                }));
            });
            NORMAL_MODE.onClick.AddListener(() => OnPlay());
            ZEN_MODE.onClick.AddListener(() => OnPlayZen());
        }
        else
        {
            START_GAME.onClick.AddListener(() => OnPlay());
            ZEN_MODE.onClick.AddListener(() => OnPlayZen());
        }
    }


    public void OnPlay()
    {
        transform.DOMove(movePoint.position, 5f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            if(!isVR)
                SceneManager.LoadScene("SampleSceneNoVR");
            else SceneManager.LoadScene("SampleScene");
            Debug.Log("Load gameplay.");
        });
        SoundManager.self.PlayAudio("click_collect");
        StartCoroutine(HideMenu(0f,0.5f));
        StartCoroutine(Delay(3f, () => SoundManager.self.PlayAudio("walk")));
    }
    public void OnPlayZen()
    {
        transform.DOMove(movePoint.position, 5f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            if(!isVR)
                SceneManager.LoadScene("ZenModeSceneNoVR");
            else SceneManager.LoadScene("ZenModeScene");
            Debug.Log("Load gameplay.");
        });
        SoundManager.self.PlayAudio("click_collect");
        StartCoroutine(HideMenu(0f,0.5f));
        StartCoroutine(Delay(3f, () => SoundManager.self.PlayAudio("walk")));
    }

    void Update()
    {
        if (previousState != (int)state && (state == CameraState.QUIT || state == CameraState.SETTING || state == CameraState.PLAY || state == CameraState.LEADERBOARD))
        {
            OnStateChanged();
            previousState = (int) state;
        }

        if (Input.GetKeyDown((KeyCode.F1)))
        {
            state = CameraState.SETTING;
            SETTING.transform.DOScale(Vector3.one, .25f).SetDelay(2.5f).SetEase(Ease.InOutCubic);
        }
        
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            OVRManager.display.RecenterPose();
        }
    }

    public void OnStateChanged()
    {
        Debug.LogWarning("State changed!");
        transform.DOMove(positions[(int) state], dt).SetEase(Ease.InOutCubic);
        transform.DORotate(rotations[(int) state], dt).SetEase(Ease.InOutCubic);
        if (state == CameraState.PLAY)
        {
            StartCoroutine(ShowMenu(1f, 1.5f));
        }
        else
        {
            StartCoroutine(HideMenu(0f, 1.5f));
        }

        if (state == CameraState.QUIT)
        {
            StartCoroutine(Delay(2f, () => QUIT.transform.DOScale(Vector3.one*2.5f, 0.25f).SetEase(Ease.InOutElastic)));
        }
    }

    public IEnumerator ShowMenu(float waitTime, float duration)
    {
        yield return new WaitForSeconds(waitTime);
        START_GAME.transform.DOScale(Vector3.one, duration).SetEase(Ease.InOutElastic);
        LEADERBOARD.transform.DOScale(Vector3.one, duration).SetEase(Ease.InOutElastic);
        QUITGAME.transform.DOScale(Vector3.one, duration).SetEase(Ease.InOutElastic);
    }

    public IEnumerator HideMenu(float waitTime, float duration)
    {
        yield return new WaitForSeconds(waitTime);
        START_GAME.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InOutElastic);
        LEADERBOARD.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InOutElastic);
        QUITGAME.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InOutElastic);
        ZEN_MODE.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InOutElastic);
        NORMAL_MODE.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InOutElastic);
    }

    public IEnumerator Delay(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action.Invoke();
    }
}
