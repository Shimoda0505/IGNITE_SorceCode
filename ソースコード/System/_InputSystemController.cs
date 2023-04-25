using UnityEngine;
using UnityEngine.InputSystem;/*�yInputSystem��Editor�g�����K�v�ł��z*/

//���͌Ăяo���p
public struct _InputSystemController
{

    #region �X�e�B�b�N�̓��͗�(�ϐ�)

    //�E�X�e�B�b�N�̓��͗�
    private Vector2 rightStickValue;
    public Vector2 RightStickValue()//�E�X�e�B�b�N�̉������͂�ԋp
    {
        return rightStickValue;
    }

    //���X�e�B�b�N�̓��͗�
    private Vector2 leftStickValue;
    public Vector2 LeftStickValue()//���X�e�B�b�N�̉������͂�ԋp
    {
        return leftStickValue;
    }

    #endregion


    #region �X�e�B�b�N

    public bool RightStick()//�E�X�e�B�b�N
    {
        if (Mathf.Abs(Gamepad.current.rightStick.ReadValue().x) + Mathf.Abs(Gamepad.current.rightStick.ReadValue().y) >= 0.1f)
        {
            rightStickValue = Gamepad.current.rightStick.ReadValue();

            return true;
        }

        return false;
    }

    public bool LeftStick()//���X�e�B�b�N
    {
        if (Mathf.Abs(Gamepad.current.leftStick.ReadValue().x) + Mathf.Abs(Gamepad.current.leftStick.ReadValue().y) >= 0.1f)
        {
            leftStickValue = Gamepad.current.leftStick.ReadValue();

            return true;
        }

        return false;
    }

    #endregion


    #region R3L3(�������Ƃ�)

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


    #region R3L3(�������Ƃ�)
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



    #region YXBA�{�^��(�����Ă��)
    public bool ButtonNorth()//Y�{�^��
    {
        if (Gamepad.current.buttonNorth.isPressed)
        {
            return true;
        }

        return false;
    }

    public bool ButtonSouth()//A�{�^��
    {
        if (Gamepad.current.buttonSouth.isPressed)
        {
            return true;
        }

        return false;
    }

    public bool ButtonEast()//B�{�^��
    {
        if (Gamepad.current.buttonEast.isPressed)
        {
            return true;
        }

        return false;
    }

    public bool ButtonWest()//X�{�^��
    {
        if (Gamepad.current.buttonWest.isPressed)
        {
            return true;
        }

        return false;
    }
    #endregion


    #region YXBA�{�^��(��������)
    public bool ButtonNorthDown()//Y�{�^��
    {
        if (Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool ButtonSouthDown()//A�{�^��
    {
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool ButtonEastDown()//B�{�^��
    {
        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool ButtonWestDown()//X�{�^��
    {
        if (Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }
    #endregion


    #region YXBA�{�^��(��������)
    public bool ButtonNorthUp()//Y�{�^��
    {
        if (Gamepad.current.buttonNorth.wasReleasedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool ButtonSouthUp()//X�{�^��
    {
        if (Gamepad.current.buttonSouth.wasReleasedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool ButtonEastUp()//B�{�^��
    {
        if (Gamepad.current.buttonEast.wasReleasedThisFrame)
        {
            return true;
        }

        return false;
    }

    public bool ButtonWestUp()//A�{�^��
    {
        if (Gamepad.current.buttonWest.wasReleasedThisFrame)
        {
            return true;
        }

        return false;
    }
    #endregion


    #region LR��
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


    #region LR��
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


    #region LR��
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


    #region LR��(�������Ƃ�)

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


    #region LR��(��������)

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


    #region �X�^�[�g,�I�v�V����

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


    #region �U��

    public void ControllerVibretion(float LV, float RV)
    {
        Gamepad.current.SetMotorSpeeds(LV, RV);
    }

    #endregion


    #region All�{�^��
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

