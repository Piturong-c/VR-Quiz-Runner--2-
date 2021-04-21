using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public int timer = 30; //
    public int quizDuration = 5;
    public int quizTime = 10; //
    public float max = 3;
    public float dmax = 0.5f; //increase difficult over
    public float mmax = 10; // max difficult
    public static QuizManager self;

    void Awake()
    {
        self = this;
    }

    public Answer Quiz()
    {
        int a = Random.Range(1,(int)max);
        int b = Random.Range(1,(int)max);
        int c = Random.Range(1,(int)max);
        string o1;
        string o2;
        int root = Random.Range(0,6);
        int answer = 0;
        switch (root)
        {
            case 0:
                o1 = "+"; o2 = "+";
                answer = a + b + c;
                break;
            case 1:
                o1 = "+"; o2 = "-";
                answer = a + b - c;
                break;
            case 2:
                o1 = "-"; o2 = "-";
                answer = a - b - c;
                break;
            case 3:
                o1 = "+"; o2 = "*";
                answer = a + b * c;
                break;
            case 4:
                o1 = "*"; o2 = "+";
                answer = a * b + c;
                break;
            case 5:
                o1 = "*"; o2 = "-";
                answer = a * b - c;
                break;
            case 6:
                o1 = "-"; o2 = "*";
                answer = a - b * c;
                break;
            default:
                o1 = "+"; o2 = "+";
                answer = a + b + c;
                break;
        }
        
        string text = "";

        if (o1 == "*")
            text = string.Format(" ( {0} {3} {1} ) {4} {2} ",a,b,c,o1,o2);
        else if(o2 == "*")
            text = string.Format(" {0} {3} ( {1} {4} {2} ) ",a,b,c,o1,o2);
        else
            text = string.Format(" {0} {3} {1} {4} {2} ",a,b,c,o1,o2);
        max += dmax;
        max = Mathf.Clamp(max, 0, mmax);
        return new Answer(answer, text);
    }
}

public class Answer
{
    public int rightAnswer;
    public int[] answers = new int[3]; // change number here 3 = 3 answer
    public string text;
    public Answer(int rightAnswer, string text)
    {
        this.rightAnswer = rightAnswer;
        this.text = text.Replace('*','x');
        answers[0] = rightAnswer + Random.Range(-9, 9);
        answers[1] = rightAnswer;
        answers[2] = rightAnswer + Random.Range(-9, 9);
        //answers[3] = rightAnswer + Random.Range(-9, 9);
        
        if (answers[0] == answers[2])
        {
            answers[0] = rightAnswer + Random.Range(-9, 9);
            answers[2] = rightAnswer + Random.Range(-9, 9);
        }

        if (answers[0] == answers[1])
        {
            answers[0] = rightAnswer + Random.Range(-9, 9);
        }

        if (answers[1] == answers[2])
        {
            answers[0] = rightAnswer + Random.Range(-9, 9);
        }
        Shuffle(answers);
    }

    public static void Shuffle<T>(T[] arrays)
    {
        for (int t = 0; t < arrays.Length; t++ )
        {
            T tmp = arrays[t];
            int r = Random.Range(t, arrays.Length);
            arrays[t] = arrays[r];
            arrays[r] = tmp;
        }
    }
    
}
