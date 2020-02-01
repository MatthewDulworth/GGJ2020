using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public PlayerInfo[] players;
    public ControlScheme[] inputs;
    //The names of all stages supporting said number of players
    public string[] stages;
    //Same size as string[] stages and stores whether or not stages have been visted or not
    public bool[] stagesVisited;
    public Gun[] guns;

    private static int NUM_CONTROL_TYPES = 5;
    // Start is called before the first frame update
    void Start()
    {
        stagesVisited = new bool[stages.Length];
        //For debugging. Ensures a certain stage if opened in editor will always be the first stage to come up
        if(!PlayerPrefs.GetString("DebuggingStage").Equals("null"))
        {
            VisitAllButOneStage((PlayerPrefs.GetString("DebuggingStage")));
        }
        PlayerPrefs.SetString("DebuggingStage", "null");

        CreateControlSchemes();
        inputs = gameObject.GetComponents<ControlScheme>();
    }
    private void CreateControlSchemes()
    {
        for (int i = 0; i < NUM_CONTROL_TYPES; i++)
        {
            ControlScheme added = gameObject.AddComponent<ControlScheme>();
            switch (i)
            {
                case 0:
                    added.SetControlScheme(ControlScheme.Controller.keyboard);
                    break;
                case 1:
                    added.SetControlScheme(ControlScheme.Controller.contr0);
                    break;
                case 2:
                    added.SetControlScheme(ControlScheme.Controller.contr1);
                    break;
                case 3:
                    added.SetControlScheme(ControlScheme.Controller.contr2);
                    break;
                case 4:
                    added.SetControlScheme(ControlScheme.Controller.contr3);
                    break;
                default:
                    added.SetControlScheme(ControlScheme.Controller.keyboard);
                    break;
            }
            print("Successfully set up controller " + added.SubmitAxis);
        }
    }
    public ControlScheme[] GetInputs()
    {
        return inputs;
    }
    //Used for NextStageButton() in ButtonController. Will ensure no stage is played twice
    public bool HasStageBeenVisited(int index)
    {
        return stagesVisited[index];
    }
    //Sets all stages in bool[] stagesVisted false
    public void ResetStagesVisted()
    {
        for(int i = 0; i < stagesVisited.Length; i++)
        {
            stagesVisited[i] = false;
        }
    }
    //Use for debugging 
    public void VisitAllButOneStage(string name)
    {
        for (int i = 0; i < stages.Length; i++)
        {
            if (!stages[i].Equals(name))
            {
                stagesVisited[i] = true;
            }
        }
    }
    //Use for debugging
    public void VisitStage(int index)
    {
        stagesVisited[index] = true;
    }
}
