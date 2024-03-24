using UnityEngine;
using UnityEngine.UI;

public class Jumping_UI_Manager : MonoBehaviour
{
    public GameObject gameManager;
    public Text scoreText;

    void Update()
    {
        scoreText.text = gameManager.GetComponent<Jumping_GameManager>().score.ToString() + " 점";
    }
}
