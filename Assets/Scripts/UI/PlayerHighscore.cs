using TMPro;
using UnityEngine;

public class PlayerHighscore : MonoBehaviour
{
    [SerializeField] PlayerVariablesSave playerVariablesSave;
    TMP_Text highscore;

    // Start is called before the first frame update
    void Start()
    {
        highscore = GetComponent<TMP_Text>();
        highscore.SetText(playerVariablesSave.highscore.ToString("0"));
    }

}
