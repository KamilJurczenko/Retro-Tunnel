using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject startUI;
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource UIHitFX;
    [SerializeField] SoundManager soundManager;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject gameoverPanel;
    [SerializeField] Image musicButtonColor;
    [SerializeField] Image soundFXButtonColor;

    [SerializeField] Color inactiveColor;
    [SerializeField] Color activeColor;

    public static bool startGameButtonCalled = false;
    public static bool pauseButtonCalled;
    public static bool restartButtonCalled = false;

    private void Awake()
    {
        pauseButtonCalled = false;
    }
    private void Start()
    {
        soundFXButtonColor.color = SetButtonState(GameManager.playSoundFX);
        musicButtonColor.color = SetButtonState(GameManager.playMusic);
    }
    public void OnStartGameButtonEvent()
    {
        startGameButtonCalled = true;
        // Disable Start Screen UI
        startUI.SetActive(false);
        if(GameManager.playMusic)
            backgroundMusic.Play();
        soundManager.StartCoroutine(soundManager.ChangeVolumeCoroutine());
        if(GameManager.playSoundFX)
            UIHitFX.Play();
        // Move Camera To Player
        StartCoroutine(cameraController.LerpToPlayer());
    }
    public void OnPauseButtonEvent()
    {
        pauseButtonCalled = true;
        gameManager.PauseGame();
    }
    public void OnResumeButtonEvent()
    {
        gameManager.ResumeGame();
    }
    public void OnQuitButtonEvent()
    {
        startGameButtonCalled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnLeaderBoardButtonEvent()
    {

    }
    public void OnMusicButtonEvent()
    {
        if (GameManager.playSoundFX)
            UIHitFX.Play();
        int val;
        if (GameManager.playMusic == false)
        {
            val = 1;
        }
        else
        {
            val = 0;
        }
        PlayerPrefs.SetInt("musicOn",val);
        GameManager.playMusic = val == 0 ? false : true;
        // change button State
        musicButtonColor.color = SetButtonState(GameManager.playMusic);
    }
    public void OnSoundFXButtonEvent()
    {
        if (GameManager.playSoundFX)
            UIHitFX.Play();
        int val;
        if (GameManager.playSoundFX == false)
        {
            val = 1;
        }
        else
        {
            val = 0;
        }
        PlayerPrefs.SetInt("soundOn", val);
        GameManager.playSoundFX = val == 0 ? false : true;
        // change button State
        soundFXButtonColor.color = SetButtonState(GameManager.playSoundFX);
    }
    private Color SetButtonState(bool state)
    {
        Color clr;
        if (state == true)
        {
            clr = activeColor;
        }
        else
        {
            clr = inactiveColor;
        }
        return clr;
    }
    public void OnRestartNoAdButtonEvent()
    {
        restartButtonCalled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnRestartAdButtonEvent()
    {
        gameoverPanel.SetActive(false);
    }
}
