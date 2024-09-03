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


    // 값 할당은 내부에서만 가능
    public float move { get; private set; } // 감지된 움직임 입력값
    public float rotate { get; private set; } // 감지된 회전 입력값
    public bool fire { get; private set; } // 감지된 발사 입력값
    public bool reload { get; private set; } // 감지된 재장전 입력값

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
