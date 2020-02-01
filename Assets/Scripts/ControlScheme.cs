using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScheme : MonoBehaviour
{
    private string horizontalAxis;
    private string verticalAxis;
    private string jumpAxis;
    private string rollAxis;
    private string attackAxis;
    private string submitAxis;
    private string cancelAxis;

    public string HorizontalAxis { get => horizontalAxis; set => horizontalAxis = value; }
    public string VerticalAxis { get => verticalAxis; set => verticalAxis = value; }
    public string JumpAxis { get => jumpAxis; set => jumpAxis = value; }
    public string SubmitAxis { get => submitAxis; set => submitAxis = value; }
    public string CancelAxis { get => cancelAxis; set => cancelAxis = value; }
    public string RollAxis { get => rollAxis; set => rollAxis = value; }
    public string AttackAxis { get => attackAxis; set => attackAxis = value; }

    public enum Controller : int
    {
        contr0 = 0, contr1 = 1, contr2 = 2, contr3 = 3, keyboard = 4
    }
    public ControlScheme(Controller controller)
    {
        SetControlScheme(controller);
    }
    public void SetControlScheme(Controller controller)
    {
        string inputAxisName = "";

        HorizontalAxis = inputAxisName + "Horizontal";
        VerticalAxis = inputAxisName + "Vertical";
        JumpAxis = inputAxisName + "Vertical";
        SubmitAxis = inputAxisName + "Submit";
        CancelAxis = inputAxisName + "Cancel";
    }
    public float HorizontalInput()
    {
        return Input.GetAxis(HorizontalAxis);
    }
    public float VerticalInput()
    {
        return Input.GetAxis(VerticalAxis);
    }
    public bool JumpPressed()
    {
        return Input.GetAxis(JumpAxis) > 0;
    }
    public bool RollPressed()
    {
        return Input.GetAxis(RollAxis) > 0;
    }
    public bool SubmitPressed()
    {
        return Input.GetAxis(SubmitAxis) > 0;
    }
    public bool CancelPressed()
    {
        return Input.GetAxis(CancelAxis) > 0;
    }
}
