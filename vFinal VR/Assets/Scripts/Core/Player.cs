using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 10; // Unit per sec
    public float acc = 0.01f; // Unit per sec2
    public float acc2; // Unit per sec2 //dupe5
    public float maxSpeed = 50;
    public int meters;
    public TMP_Text meterText;
    public LayerMask groundMask;
    private bool _newRoad;
    private int _roadIdx;
    private int _prevValue;
    public float timer;
    private float totalTime;

    public TMP_Text QUIZ_WARNING,GAME_OVER;
    public GameObject QUIZ, CONGRATS;
    public Image BACKGROUND;
    public Button EXIT,TRY_AGAIN;
    public bool isTimeout = false;
    public bool isQuizDurationTimeout = false;
    public bool isQuizTimeout = false;
    public bool SpeedMode = false;
    public int playerHealth = 3; // for zenmode
    public static int maxPlayerHealth = 99; //Death to tirgger change
    private Answer answer;
    public static Player self;
    public bool isPlayerDead = false;
    private Rigidbody rb;
    public Image SCREEN;
    public TMP_Text SCORE;
    public bool isVR = true;
    public Transform VRReference;
    
    private void Awake()
    {
        self = this;
        QUIZ_WARNING = GameObject.Find("QUIZ_WARNING").GetComponent<TMP_Text>();
        QUIZ = GameObject.Find("QUIZ");
        GAME_OVER = GameObject.Find("GAME_OVER").GetComponent<TMP_Text>();
        CONGRATS = GameObject.Find("CONGRATS");
        EXIT = GameObject.Find("EXIT").GetComponent<Button>();
        TRY_AGAIN = GameObject.Find("TRY_AGAIN").GetComponent<Button>();
        //BACKGROUND = GameObject.Find("BACKGROUND").GetComponent<Image>();

        /* EXIT GAME */
        EXIT.onClick.AddListener(() => Application.Quit());
        /* RELOAD SCENE */
        TRY_AGAIN.onClick.AddListener(() =>
        {
            Debug.LogWarning("Load Scene!");
            SceneManager.LoadScene(0);
        });

        rb = GetComponent<Rigidbody>();
        SCREEN = GameObject.Find("SCREEN").GetComponent<Image>();
        SCORE = GameObject.Find("T_SCORE").GetComponent<TMP_Text>();
        speed = PlayerPrefs.GetFloat("UI_SPEED");
        maxSpeed = PlayerPrefs.GetFloat("UI_MAX_SPEED");
        acc = PlayerPrefs.GetFloat("UI_ACC");
    }
    public float updateDYevery = 1f;
    public float updateDYTimer = 0;
    private void Start()
    {
       playerHealth = getTemporary("HP",maxPlayerHealth);
       meters = getTemporary("M",-5);
       LeaderboardManager.self.score = getTemporary("SCORE",0);
       updateDYTimer = updateDYevery;
        totalTime = QuizManager.self.timer + QuizManager.self.quizDuration + QuizManager.self.quizTime;
        timer = totalTime;
        SCREEN.DOFade(0f, 3.5f).SetEase( Ease.InOutFlash ).OnComplete(() => SCREEN.gameObject.SetActive(false));
    }

    private float escTimer = 1f;
    void Update()
    {
        if (!isPlayerDead)
        {
            UpdatePlayerMovement();
            if (!SpeedMode)
            {
                timer -= Time.deltaTime;
            }
        }
        if (OVRInput.Get(OVRInput.Button.Two)) //Dupe
        {
            escTimer -= Time.deltaTime; // 0.01f x 100 frames = 1
            if (escTimer <= 0)
            {
                if (SpeedMode)
                {
                    setTemporary("HP",maxPlayerHealth);
                    setTemporary("M",-5);
                }
                SceneManager.LoadScene(isVR ? 0 : 3);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return) || OVRInput.GetUp(OVRInput.Button.Two))
            {
                if (SpeedMode)
                {
                    setTemporary("HP",maxPlayerHealth);
                    setTemporary("M",-5);
                    SceneManager.LoadScene(isVR ? 5 : 4);
                }else
                    SceneManager.LoadScene(isVR ? 1 : 2);
            }

            if (OVRInput.Get(OVRInput.Button.Two))
            {
                escTimer -= Time.deltaTime; // 0.01f x 100 frames = 1
                if (escTimer <= 0)
                {
                    if (SpeedMode)
                    {
                        setTemporary("HP",maxPlayerHealth);
                        setTemporary("M",-5);
                    }
                    SceneManager.LoadScene(isVR ? 0 : 3);
                }
            }
            else
            {
                escTimer = 1f;
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SpeedMode)
                {
                    setTemporary("HP",maxPlayerHealth);
                    setTemporary("M",-5);
                }
                SceneManager.LoadScene(isVR ? 0 : 3);
            }
        }
    }

    void FixedUpdate()
    {
        UpdateMeterUI();
        UpdateCurrentRoadId();
        if (timer <= totalTime - QuizManager.self.timer)
        {
            if (!isTimeout)
            {
                QUIZ_WARNING.text = "<b>" + QuizManager.self.timer +
                                    " second(s) have passed</b>\nGet ready for quiz time!";
                QUIZ_WARNING.DOFade(1, 1f * totalTime * 0.1f).SetDelay(5f * totalTime * 0.1f).OnComplete(() => StartCoroutine(OnCompleteHandler()));
                isTimeout = true;
            }
        }
        if (timer <= totalTime - QuizManager.self.timer - QuizManager.self.quizDuration)
        {
            if (!isQuizDurationTimeout)
            {
                answer = QuizManager.self.Quiz();
                //Debug.Log("Text: " + answer.text);
                //Debug.Log("Answer: " + answer.rightAnswer + " , List: " + answer.getListAnswers());
                QUIZ.transform.GetChild(0).GetComponent<TMP_Text>().text = answer.text;
                QUIZ.transform.DOScale(Vector3.one, .5f * totalTime * 0.1f).SetDelay(5f * totalTime * 0.1f).SetEase(Ease.InOutQuint).OnComplete(() => StartCoroutine(OnCompleteHandler()));
                isQuizDurationTimeout = true;
            }
        }
        if (timer <= 0)
        {
            if (!isQuizTimeout)
            {
                /*QUIZ_WARNING.text = "ANSWER!";
                QUIZ_WARNING.DOFade(1, 1f * totalTime * 0.1f).SetDelay(5f * totalTime * 0.1f).OnComplete(() => StartCoroutine(OnCompleteHandler()));*/
                isQuizTimeout = true;
            }
        }
    }

    public IEnumerator OnCompleteHandler()
    {
        yield return new WaitForSeconds(3f);
        QUIZ_WARNING.DOFade(0, 1f);
        QUIZ.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuint);
        yield return null;
    }

    private void UpdateCurrentRoadId()
    {
        /* Ceiling value of the X axis to get current running id */
        _prevValue = Mathf.CeilToInt((transform.position.x - 5) / 10.0f);

        if (_prevValue != _roadIdx && !_newRoad)
        {
            OnRoadEntered();
            _newRoad = true;
        }
        else
            _newRoad = false;
        _roadIdx = Mathf.CeilToInt((transform.position.x - 5) / 10.0f); //ตัดเศษ
    }

    private bool canJump = true;
    public float dy = 0;
    public float dY = 0;
    public float DY = 0;

    public bool isVRJump = false;
    public bool isVRGround = true;
    
    public float vrJumpUpDetect = -1f;
    public float vrJumpDownDetect = 1f;
    public float playerVRPosition = 0;
    public Vector2 maxMinPosition = new Vector2(-4,4);
    private void UpdatePlayerMovement()
    {
        if (isVR)
        {
            dY = 10 * VRReference.localPosition.y; //คูณ 10 เพื่อให้ผลต่างชัดเจนขึ้น
            DY = dy - dY; //ผลต่างของแกน Y , diff
            if (updateDYTimer <= 0)
            {
                dy = 10 * VRReference.localPosition.y; //ตรวจจับความเร็ว vr ทุก (ตัวแปร 0.25) 250 milli sec โดยเอ่าผลต่างของแกน y ก่อนหน้า
                updateDYTimer = updateDYevery; //0.25
            }
            else
            {
                updateDYTimer -= Time.deltaTime; //deltaTime 60fps =
            }
            
            if (!isVRJump && IsGrounded() && OVRInput.Get((OVRInput.Button.PrimaryThumbstickUp))) //Dupe3 + Mod
            {
                canJump = false;
                rb.AddForce(Vector3.up * 2.5f, ForceMode.Impulse);
                if (acc <= 0.5) //dupe7
                {
                    acc2 = acc*0.1f;
                    acc += acc2;
                }
            }
            if (isVRGround && OVRInput.Get((OVRInput.Button.PrimaryThumbstickDown))) //Dupe4 + Mod
            {
                rb.AddForce(-Vector3.up * 4.5f, ForceMode.Impulse);
                isVRGround = false;
            }
            // Jump for only one frame
            if (DY <= vrJumpUpDetect)
            {
                if (!isVRJump && IsGrounded() && VRReference.localPosition.y*10f > 8f)
                {
                    if (acc <= 0.5) //dupe6
                    {
                        acc2 = acc*0.1f;
                        acc += acc2;
                    }
                    Debug.Log("Jump!");
                    //SoundManager.self.PlayAudio("jump");
                    canJump = false;
                    //StartCoroutine(Jump());
                    rb.AddForce(Vector3.up * 9.5f, ForceMode.Impulse); //impulse ความเร็วเริ่มต้นสูง
                    Debug.Log("Jump! " + " at " + gameObject.name);
                }

                isVRJump = true;
            }
            else
            {
                isVRJump = false;
            }


            if (DY >= vrJumpDownDetect)
            {
                if (isVRGround)
                {
                    Debug.Log("Crouch! " + " at " + gameObject.name);
                    rb.AddForce(-Vector3.up * 12f, ForceMode.Impulse);
                }
                isVRGround = false;
            }
            else
            {
                isVRGround = true;
            }
        }
        
        if(SpeedMode)
        {
            transform.Translate(speed * Time.deltaTime + acc, 0, 0);
            //speed += acc; // accelerate if it is in ZenMode
        }
        else
        {
            transform.Translate(speed * Time.deltaTime,0,0); //ขยับวัตถุ
        }

        speed = Mathf.Clamp(speed, 0, maxSpeed);
        if (!isVR)
        {
            float horizontal = Input.GetAxis("Horizontal");
            transform.Translate(0, 0, -10 * horizontal * Time.deltaTime);
        }
        else
        {
            /*float horizontal = Input.GetAxis("Horizontal");
            transform.Translate(0, 0, -10 * horizontal * Time.deltaTime);*/
            
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft))
            {
                playerVRPosition += 10f * Time.deltaTime;
            }
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
            {
                playerVRPosition -= 10f * Time.deltaTime;
            }
            
            transform.position = new Vector3(transform.position.x, transform.position.y, playerVRPosition + 10 * VRReference.localPosition.z); //VR Left Right
        }
        transform.position = new Vector3(transform.position.x, transform.position.y,
            Mathf.Clamp(transform.position.z, maxMinPosition.x, maxMinPosition.y)); //ระยะ -4 , 4
        if (!isVR)
        {
            if ((Input.GetKeyDown(KeyCode.Space)) && IsGrounded())
            {
                //SoundManager.self.PlayAudio("jump");
                canJump = false;
                //StartCoroutine(Jump());
                Debug.Log("Jump! " + " at " + gameObject.name);
                rb.AddForce(Vector3.up * 7.5f, ForceMode.Impulse);
            }

            if ((Input.GetKeyDown(KeyCode.LeftControl)) && !IsGrounded())
            {
                //StartCoroutine(Jump());
                Debug.Log("Crouch! " + " at " + gameObject.name);
                rb.AddForce(-Vector3.up * 15f, ForceMode.Impulse); //VR กระโดด ลง
            }
        }

        if (IsGrounded())
        {
            if (canJump)
            {
                SoundManager.self.PlayAudio("landing");
                canJump = false;
            }
        }
        else
        {
            canJump = true;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.5f, groundMask);
    }

    private void OnRoadEntered()
    {
        Manager.self.envTile--;
        //GenerateCoin();
        if (Manager.self.envTile <= 0)
        {
            Manager.self.env = (Manager.Environment) (UnityEngine.Random.Range(0, 2));
            Manager.self.envTile = Manager.self.maxTile;
        }

        if (timer <= 0)
        {
            MapGenerator.self.GenerateQuiz(answer);
            timer = totalTime;
            isTimeout = false;
            isQuizDurationTimeout = false;
            isQuizTimeout = false;
        }
        else
        {
            /* Generate new road */
            Manager.self.PublishRoad();
            MapGenerator.self.GenerateRoad();
        }

        // all road ids
        foreach (Road road in FindObjectsOfType<Road>())
        {
            // gen 6 road
            if (road.id == _roadIdx + 6)
            {
                road.Obstate();
                foreach (Coin coin in FindObjectsOfType<Coin>())
                {
                    if (coin.transform.position.x - transform.position.x <= -50)
                    {
                        coin.CheckObstate();
                    }
                }
            }
            //destroy old road
            if (road.transform.position.x - transform.position.x <= -50)
            {
                Destroy(road.gameObject);
            }
        }
    }

    private void UpdateMeterUI()
    {
        meters = getTemporary("M",-5) + (int) transform.position.x - 5; /* Start counting meters based on the X axis  */
        meterText.text = $"{meters}m";
        int score = LeaderboardManager.self.score;
        if (SpeedMode){
            SCORE.text = $"<color=red>Health {playerHealth}</color>  <color=yellow>Meter {meters}</color>";
        }
        else
        {
            SCORE.text = $"<color=red>Health {playerHealth}</color>  <color=yellow>SCORE {score}</color>";
        }
    }

    public void CompareAnswer(string pathAnswer)
    {
        if (answer.rightAnswer.ToString() == pathAnswer)
        {
            LeaderboardManager.self.score++;
            PlayerPrefs.SetInt("SCORE",LeaderboardManager.self.score);
            //SoundManager.self.PlayAudio("correct");
            SCORE.transform.DOScale(new Vector3(1.25f,1.25f,1.25f), .25f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                SCORE.transform.DOScale(new Vector3(1, 1, 1), .25f).SetEase(Ease.InExpo);
            });
            CONGRATS.transform.DOScale(new Vector3(1,1,1), .25f).SetEase(Ease.InOutElastic);
            CONGRATS.transform.DOScale(new Vector3(0, 0, 0), .5f).SetDelay(1f).SetEase(Ease.InOutElastic);
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        if (playerHealth <= 1)
        {
            rb.isKinematic = true;
            Camera.main.DOFieldOfView(70, 5f).SetEase(Ease.InOutSine);
            GAME_OVER.transform.DOScale(new Vector3(1, 1, 1), .25f).SetEase(Ease.InOutElastic);
            TRY_AGAIN.transform.DOScale(new Vector3(1, 1, 1), .25f).SetEase(Ease.InOutElastic);
            EXIT.transform.DOScale(new Vector3(1, 1, 1), .25f).SetEase(Ease.InOutElastic);
            QUIZ_WARNING.transform.DOScale(Vector3.zero, .25f).SetEase(Ease.InOutElastic);
            QUIZ.transform.DOScale(Vector3.zero, .25f).SetEase(Ease.InOutElastic);
            BACKGROUND.DOColor(new Color(0, 0, 0, .5f), .25f).SetEase(Ease.InOutElastic);
            CONGRATS.transform.DOScale(Vector3.zero, .25f).SetEase(Ease.InOutElastic);
            SCORE.transform.DOScale(Vector3.zero, .25f).SetEase(Ease.InOutElastic);
            isPlayerDead = true;

            PlayerPrefs.SetInt("SCORE", 0);
            setTemporary("HP",maxPlayerHealth);

            if(SpeedMode) LeaderboardManager.self.score = meters;
            LeaderboardManager.self.UpdateScore();
        }
        else
        {
            playerHealth -= 1;
            //StartCoroutine(CameraShake(1.25f, 2f));
        }
        //SoundManager.self.PlayAudio("die");
    }


    public static int getTemporary(String key, int defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key,defaultValue);
        }
        return PlayerPrefs.GetInt(key);
    }

    public static void setTemporary(String key,int v)
    {
        PlayerPrefs.SetInt(key,v);
    }
}
