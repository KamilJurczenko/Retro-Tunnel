using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] GameObject gameStartUI;
    [SerializeField] GameObject[] inGameUI;
    // Start is called before the first frame update
    void Start()
    {
        if (ButtonHandler.startGameButtonCalled)
        {
            foreach(GameObject go in inGameUI)
            {
                go.SetActive(true);
            }
            gameStartUI.SetActive(false);
        }
    }

}
