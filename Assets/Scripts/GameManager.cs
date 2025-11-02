using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Used for debug purposes
    [SerializeField] bool permanentSpeedIncrease; // Increases Player Speed permanently until max Value reached
    [SerializeField] bool newStageSpeedIncrease; // Increases Player Speed on new Stage/breaks
    [SerializeField] bool spawnObstacles;
    [SerializeField] bool obstacleCollider;
    [SerializeField] bool forceTutorialStart;
    [SerializeField] int startStage; // 1-4
    [SerializeField] bool dontDie;

    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] GameObject[] hiddenOnPauseObjects;
    [SerializeField] GameObject darkenScreen;
    [SerializeField] GameObject isPausedObject;
    [SerializeField] TutorialManager tutorialManager;

    public static bool pSI;
    public static bool nSSI;
    public static bool sO;
    public static bool oC;
    public static bool pT;
    public static int sS;
    public static bool dD;
    public static bool playMusic;
    public static bool playSoundFX;

    public static bool gamePaused;
    public static bool gameStarted;
    public static bool gameOver;
    public static bool gameIsRestarting;

    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.HasKey("musicOn"))
            playMusic = PlayerPrefs.GetInt("musicOn") == 0 ? false : true;
        else
            playMusic = true;
        if (PlayerPrefs.HasKey("soundOn"))
            playSoundFX = PlayerPrefs.GetInt("soundOn") == 0 ? false : true;
        else
            playSoundFX = true;

        sS = startStage;
        pSI = permanentSpeedIncrease;
        nSSI = newStageSpeedIncrease;
        sO = spawnObstacles;
        oC = obstacleCollider;
        pT = forceTutorialStart;
        gameStarted = false;
        gameOver = false;
        dD = dontDie;

        if (ButtonHandler.startGameButtonCalled)
            gameIsRestarting = true;
    }
    // Called when Camera at Player
    public void OnGameStart()
    {
        if (!PlayerVariablesSave.tutorialPlayed)
        {
            tutorialManager.enabled = true;
        }
    }
    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
        backgroundMusic.Pause();
        darkenScreen.SetActive(true);
        isPausedObject.SetActive(true);
        foreach (GameObject go in hiddenOnPauseObjects)
            go.SetActive(false);
    }

    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        if(playMusic)
            backgroundMusic.Play();
        darkenScreen.SetActive(false);
        isPausedObject.SetActive(false);
        foreach (GameObject go in hiddenOnPauseObjects)
            go.SetActive(true);
    }
}
