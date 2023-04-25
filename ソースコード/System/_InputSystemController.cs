using UnityEngine;
using UnityEngine.InputSystem;/*【InputSystemのEditor拡張が必要です】*/

//入力呼び出し用
public struct _InputSystemController
{

    #region スティックの入力量(変数)

    //右スティックの入力量
    private Vector2 rightStickValue;
    public Vector2 RightStickValue()//右スティックの横軸入力を返却
    {
        return rightStickValue;
    }

    //左スティックの入力量
    private Vector2 leftStickValue;
    public Vector2 LeftStickValue()//左スティックの横軸入力を返却
    {
        return leftStickValue;
    }

    #endregion


    #region スティック

    public bool RightStick()//右スティック
    {
        if (Mathf.Abs(Gamepad.current.rightStick.ReadValue().x) + Mathf.Abs(Gamepad.current.rightStick.ReadValue().y) >= 0.1f)
        {
            rightStickValue = Gamepad.current.rightStick.ReadValue();

            return true;
        }

        return false;
    }

    public bool LeftStick()//左スティック
    {
        if (Mathf.Abs(Gamepad.current.leftStick.ReadValue().x) + Mathf.Abs(Gamepad.current.leftStick.ReadValue().y) >= 0.1f)
        {
            leftStickValue = Gamepad.current.leftStick.ReadValue();

            return true;
        }

        return false;
    }

    #endregion


    #region R3L3(押したとき)

    public bool Right3Down()
    {
        if (Gamepad.current.rightStickButton.wasPressedThisFrame)
        {
            return true;
        }
        return false;
    }

    public bool Left3Down()
    {
        if (Gamepad.current.leftStickButton.wasPressedThisFrame)
        {
            return true;
        }
        return false;
    }


    #endregion


    #region R3L3(離したとき)
    public bool Right3Up()
    {
        if (Gamepad.current.rightStickButton.wasReleasedThisFrame)
        {
            return true;
        }
        return false;
    }

    public bool Left3Up()
    {
        if (Gamepad.current.leftStickButton.wasReleasedThisFrame)
        {
            return true;
        }
        return false;
    }

    #endregion



    #region YXBAボタン(押してる間)
    public bool ButtonNorth()//Yボタン
    {
        if (Gamepad.current.buttonNorth.isPressed)
        {
            return true;
        }

        return false;
    }

    public bool ButtonSouth()//Aボタン
    {
        if (Gamepad.current.buttonSouth.isPressed)
        {
            return true;
        }

        return false;
    }

    public bool ButtonEast()//Bボタン
    {
        if (Gamepad.current.buttonEast.isPressed)
        {
            return true;
        }

        return false;
    }

    public bool ButtonWest()//Xボタン
    {
        if (Gamepad.current.buttonWest.isPressed)
        {
            return true;
        }

        return false;
    }
    #endregion


    #region YXBAボタン(押した時)
    public bool ButtonNorthDown()//Yボタン
    {
        if (Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool ButtonSouthDown()//Aボタン
    {
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool ButtonEastDown()//Bボタン
    {
        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool ButtonWestDown()//Xボタン
    {
        if (Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }
    #endregion


    #region YXBAボタン(離した時)
    public bool ButtonNorthUp()//Yボタン
    {
        if (Gamepad.current.buttonNorth.wasReleasedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool ButtonSouthUp()//Xボタン
    {
        if (Gamepad.current.buttonSouth.wasReleasedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool ButtonEastUp()//Bボタン
    {
        if (Gamepad.current.buttonEast.wasReleasedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool ButtonWestUp()//Aボタン
    {
        if (Gamepad.current.buttonWest.wasReleasedThisFrame)
        {
            return true;
        }

        return false;
    }
    #endregion


    #region LR上
    public bool LeftShoulder()
    {
        if (Gamepad.current.leftShoulder.ReadValue() >= 0.1f)
        {
            return true;
        }
        return false;
    }

    public bool RightShoulder()
    {
        if (Gamepad.current.rightShoulder.ReadValue() >= 0.1f)
        {
            return true;
        }
        return false;
    }
    #endregion


    #region LR上
    public bool LeftShoulderDown()
    {
        if (Gamepad.current.leftShoulder.wasPressedThisFrame)
        {
            return true;
        }
        return false;
    }

    public bool RightShoulderDown()
    {
        if (Gamepad.current.rightShoulder.wasPressedThisFrame)
        {
            return true;
        }
        return false;
    }
    #endregion


    #region LR下
    public bool LeftTrigger()
    {
        if (Gamepad.current.leftTrigger.ReadValue() >= 0.1f)
        {
            return true;
        }
        return false;
    }

    public bool RightTrigger()
    {
        if (Gamepad.current.rightTrigger.ReadValue() >= 0.1f)
        {
            return true;
        }
        return false;
    }

    #endregion


    #region LR下(押したとき)

    public bool LeftTriggerDown()
    {
        if (Gamepad.current.leftTrigger.wasPressedThisFrame)
        {
            return true;
        }
        return false;
    }

    public bool RightTriggerDown()
    {
        if (Gamepad.current.rightTrigger.wasPressedThisFrame)
        {
            return true;
        }
        return false;
    }

    #endregion


    #region LR下(離した時)

    public bool LeftTriggerUp()
    {
        if (Gamepad.current.leftTrigger.wasReleasedThisFrame)
        {
            return true;
        }
        return false;
    }

    public bool RightTriggerUp()
    {
        if (Gamepad.current.rightTrigger.wasReleasedThisFrame)
        {
            return true;
        }
        return false;
    }

    #endregion


    #region スタート,オプション

    public bool StartButton()
    {
        if (Gamepad.current.startButton.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool StartButtonUp()
    {
        if(Gamepad.current.startButton.wasReleasedThisFrame)
        {
            return true;
        }
        return false;
    }

    public bool SelectButton()
    {
        if (Gamepad.current.selectButton.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }


    #endregion


    #region 振動

    public void ControllerVibretion(float LV, float RV)
    {
        Gamepad.current.SetMotorSpeeds(LV, RV);
    }

    #endregion


    #region Allボタン
    public bool AnyButton()
    {
        if (Gamepad.current.startButton.wasPressedThisFrame || Gamepad.current.selectButton.wasPressedThisFrame || Gamepad.current.leftTrigger.wasPressedThisFrame || Gamepad.current.rightTrigger.wasPressedThisFrame ||
            Gamepad.current.buttonNorth.wasPressedThisFrame || Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current.buttonEast.wasPressedThisFrame || Gamepad.current.buttonWest.wasPressedThisFrame ||
            Gamepad.current.rightStickButton.wasPressedThisFrame || Gamepad.current.leftStickButton.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }
    #endregion


}

