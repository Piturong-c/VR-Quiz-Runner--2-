using TMPro;
using UnityEngine;

public class AnswerPath : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            string pathAnswer = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text;
            player.CompareAnswer(pathAnswer);
        }
    }
}
