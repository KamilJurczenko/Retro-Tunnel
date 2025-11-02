using UnityEngine;

public class PlayerVariablesSave : MonoBehaviour
{
    //[SerializeField] private TutorialManager tutorialManager;

    public static int floorBouncesCounter = 0;
    public static bool tutorialPlayed = false;

    public float highscore = 0;
    // Start is called before the first frame update
    void Awake()
    {
        //PlayerPrefs.DeleteAll();
        LoadPlayerVariables();
        Debug.Log("Tutorial Played: " + tutorialPlayed);
        if (!tutorialPlayed || GameManager.pT)
        {
            //tutorialManager.enabled = true;
            floorBouncesCounter = 0;
            tutorialPlayed = false;
        }
    }
    public void SavePlayerVariables()
    {
        if(PlayerController.currentScore > highscore)
        {
            highscore = PlayerController.currentScore;
            PlayerPrefs.SetFloat("highscore", highscore);
        }
    }
    public void LoadPlayerVariables()
    {
        tutorialPlayed = PlayerPrefs.GetInt("tutorialPlayed") == 0 ? false : true;
        highscore = PlayerPrefs.GetFloat("highscore");
    }
}
