using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    TMP_Text playerScoreText;

    Animator animator;

    private int scoreCached;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerScoreText = GetComponent<TMP_Text>();
        playerScoreText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        int scoreInteger = (int)PlayerController.currentScore;
        if (scoreInteger != scoreCached)
        {
            scoreCached = scoreInteger;
            playerScoreText.SetText(PlayerController.currentScore.ToString("0"));
        }
    }

    public void OnNewStage()
    {
        //Debug.Log("New Stage Player Score");
        animator.Play("scoreText_NewStage",-1,0f);
    }

    public void OnTutorialEnd()
    {
        //Debug.Log("After Tutorial Player Score");
        animator.Play("scoreText_AfterTutorial");
    }
    /*public void OnExitNewStage()
    {
        onNewStageBool = false;
    }*/
}
