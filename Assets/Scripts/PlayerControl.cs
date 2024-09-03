using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class PlayerControl : MonoBehaviour
public class PlayerControl : Singleton<PlayerControl>
{
    public Button btnReload;
    public Button btnLeftTurn;
    public Button btnRightTurn;
    public Button btnShoot;


    // �� �Ҵ��� ���ο����� ����
    public float move { get; private set; } // ������ ������ �Է°�
    public float rotate { get; private set; } // ������ ȸ�� �Է°�
    public bool fire { get; private set; } // ������ �߻� �Է°�
    public bool reload { get; private set; } // ������ ������ �Է°�

    ButtonHandler handleBtnLeft = null;
    ButtonHandler handleBtnRight = null;

    PlayerMovement playerMovement = null;

    protected override void Awake()
    {
        base.Awake();

        btnReload.onClick.AddListener(OnBtnReloadClick);
        handleBtnLeft = btnLeftTurn.GetComponent<ButtonHandler>();
        handleBtnRight = btnRightTurn.GetComponent<ButtonHandler>();
        btnShoot.onClick.AddListener(OnBtnShootClick);
        playerMovement = GetComponent<PlayerMovement>();

        move = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnBtnShootClick()
    {
        print("OnBtnShootClick : " + typeof(PlayerControl).Name);
        playerMovement.OnShoot();
    }

    private void OnBtnReloadClick()
    {
        playerMovement.OnReload();
    }

    // Update is called once per frame
    void Update()
    {
        if (handleBtnLeft.isButtonPressed == true)
        {
            rotate = -1f;
            move = 0.2f;
        }
        else if (handleBtnRight.isButtonPressed == true)
        { 
            rotate = 1f;
            move = -0.2f;
        }
        else
        {
            rotate = 0.0f;
            move = 0.0f;
        }
            
    }
}
