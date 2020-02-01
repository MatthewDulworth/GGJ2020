using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Stretch: private int score;
    private int lives;
    private int comboCounter;

    //Stretch: public int Score { get => score; private set => score = value; }
    public int Lives { get => lives; private set => lives = value; }
    public int ComboCounter { get => comboCounter; private set => comboCounter = value; }


    // Start is called before the first frame update
    void Start()
    {
        
    }
}
