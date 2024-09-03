using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObejectManager : MonoBehaviour
{
    public GameObject indiObject;
    public GameObject modelObject;
    public GameObject selectModelUI;
    
    public Button btnCreateEditToggle;
    public Button btnSelectModel;
    public Button btnPlaneVisibel;
    
    public Text debugText;
    public Text debugText2;

    public List<string> kitchenNames;
    public List<GameObject> kitchenObjects;


    //GameObject referObject = null;
    GameObject indicatorObject;
    ARRaycastManager arRayManager;

    Dictionary<string, GameObject> kitchenModels;

    //생성된 오브젝트를 참조하는 딕셔너리. 
    Dictionary<int, GameObject> activeObjcets;



    static int createObjectKey = 1; //생성된 오브젝트의 고유키 1부터 부여하도록한다.
    int selectCreateModelIndex; //List<string> kitchenNames 에 대응되는 인덱스 
    
    int curActiveObjectKey = 0; //  Dictionary<int, GameObject>() 에 대응하는 키.

    bool isCreate = false;

    private ARPlaneManager arPlaneManager;

    bool isPlaneVisible = true;

    private void Awake()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        kitchenModels = new Dictionary<string, GameObject>();
        activeObjcets = new Dictionary<int, GameObject>();

        for (int i = 0; i < kitchenNames.Count; i++)
        {
            kitchenModels.Add(kitchenNames[i], kitchenObjects[i]);
        }

        OnCreateEditToggle();

        indicatorObject = Instantiate(indiObject);
        indicatorObject.SetActive(false);
        arRayManager = GetComponent<ARRaycastManager>();

        selectCreateModelIndex = 6;


        selectModelUI.SetActive(false);

        btnSelectModel.onClick.AddListener(OnClickSelectButton);
        btnPlaneVisibel.onClick.AddListener(OnClickPlaneVisible);

    }

    public void OnClickPlaneVisible()
    {
        Text text = btnPlaneVisibel.GetComponentInChildren<Text>();
        if (isPlaneVisible)
        {
            text.text = "Plane" + "\n" + "On";
            isPlaneVisible = false;
        }
        else
        {
            text.text = "Plane" + "\n" + "Off";
            isPlaneVisible = true;
        }
        SetPlaneVisibility(isPlaneVisible);

    }

    //AR Plane 메터리얼 알파 값을 이용해 보이게 또는 안보이게 처리. 
    //* 새로 생성되는 Plane은 적용되지 않는다. 
    public void SetPlaneVisibility(bool isVisible)
    {
        // AR Plane Manager에서 감지된 모든 평면에 접근합니다.
        foreach (ARPlane plane in arPlaneManager.trackables)
        {
            // 각 평면의 Mesh Renderer를 가져옵니다.
            MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();

            // Mesh Renderer의 Material의 색상을 가져옵니다.
            Color color = meshRenderer.material.color;

            // isVisible 파라미터에 따라 알파값을 설정합니다.
            color.a = isVisible ? 0.5f : 0f;  // 불투명하게 만들거나 완전 투명하게 만듭니다.

            // 변경된 색상을 다시 Material에 설정합니다.
            meshRenderer.material.color = color;
        }
    }

    

    public void OnClickSelectButton()
    {
        if (selectModelUI.activeInHierarchy)
        {
            selectModelUI.SetActive(false);
            btnSelectModel.GetComponentInChildren<Text>().text = "Select";
        }
        else
        {
            selectModelUI.SetActive(true);
            btnSelectModel.GetComponentInChildren<Text>().text = "Close";
        }
    }

    public void SetModelIdex(int idx)
    {
        if (kitchenNames.Count > idx && idx >= 0)
            selectCreateModelIndex = idx;
        OnClickSelectButton();
    }

    string GetModelsIndexToString(int idx)
    {
        string str = null;
        if (kitchenNames.Count > idx && idx >= 0)
            str = kitchenNames[idx];
        else
            str = null;
        return str;
    }

    public void OnCreateEditToggle()
    {
        if (isCreate)
        {
            btnCreateEditToggle.GetComponentInChildren<Text>().text = "Create";
            isCreate = false;
            debugText.text = "수정 모드 사용중.";
        }
        else
        {
            btnCreateEditToggle.GetComponentInChildren<Text>().text = "Edit";
            isCreate = true; 
            debugText.text = "생성 모드 사용중.";

            foreach( KeyValuePair<int, GameObject> item in activeObjcets )
            {
                //item.Key
                ITransformObject t = item.Value.GetComponentInChildren<ITransformObject>();
                t.OnSelected(false);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
        DetectGround();

        if (!(indicatorObject.activeInHierarchy && Input.touchCount > 0))
        {
            return;
        }


        Touch touch = Input.GetTouch(0);

        if (touch.position.y < 300) // UI 부분은 터치 감지가 안되도록 설정. (참고: 화면 왼쪽하단이 0,0 이다.)
            return;

        //모델 선택 UI가 활성화 상태에서는 터치 동작이 작동되지 않도록 한다.
        if (selectModelUI.activeInHierarchy) 
            return;

        // 생성모드와 편집모드를 분리.
        if (isCreate)
        {
            
            if (touch.phase == TouchPhase.Began)
            {
                //레이로 생성지역에 기존 생성된 오브젝트가 있는지 확인하고 있다면, 생성하지 않는다.
                RaycastHit hits;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hits, Mathf.Infinity))
                {
                    ITransformObject itObj = hits.collider.GetComponent<ITransformObject>();
                    if (itObj != null) 
                        return;
                }

                string key = GetModelsIndexToString(selectCreateModelIndex);
                GameObject tobj = Instantiate(kitchenModels[key], indicatorObject.transform.position, indicatorObject.transform.rotation);
                tobj.GetComponentInChildren<ITransformObject>().OnSetIndex(createObjectKey);
                activeObjcets.Add(createObjectKey, tobj);
                curActiveObjectKey = createObjectKey; // 마지막에 생성한 오브젝트 키로 active 오브젝트로 설정.
                createObjectKey++;
            }
        }
        else
        {
            if(touch.phase == TouchPhase.Began)
            {
                // 2가지 고려사항. 
                // curActiveObjectKey 와 다른 오브젝트를 선택한 경우 curActiveObjectKey를 선택한 오브젝트로 변경한다.
                // 레이에 오브젝트가 선택되지 않았다면, curActiveObjectKey의 오브젝트를 이동시킨다.

                RaycastHit hits;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hits, Mathf.Infinity))
                {
                    //Plane 위에 오브젝트가 생성되므로 Ray 충돌은 감지된다, 감지된 오브젝트가 어떤것인지 판별해줘야 한다. 
                    //interface를 이용하여 오브젝트를 구별해낸다.
                    ITransformObject itObj = hits.collider.GetComponent<ITransformObject>();
                    if (itObj != null)
                    {
                        if (curActiveObjectKey != itObj.OnGetIndex())
                        {
                            //기존의 선택된 오브젝트의 아웃라인을 비활성화 시킨다.
                            ITransformObject t = activeObjcets[curActiveObjectKey].GetComponentInChildren<ITransformObject>();
                            t.OnSelected(false);

                            //새로 변경된 오브젝트의 인덱스를 액티브 인덱스로하고, 아웃라인을 활성화 시킨다.
                            curActiveObjectKey = itObj.OnGetIndex();
                            itObj.OnSelected(true); // 아웃라인 활성화
                        }
                        else
                        {
                            itObj.OnSelected(true); // 아웃라인 활성화
                        }
                        debugText2.text = "터치2 : " + ((int)touch.position.x).ToString() + " / " + ((int)touch.position.y).ToString() + " / Idx: " + itObj.OnGetIndex().ToString();
                    }
                    else //
                    {
                        itObj = activeObjcets[curActiveObjectKey].GetComponentInChildren<ITransformObject>();
                        debugText2.text = "터치3 : " + ((int)touch.position.x).ToString() + " / " + ((int)touch.position.y).ToString() + " / Idx: " + itObj.OnGetIndex().ToString();
                        itObj.OnSetPosition(indicatorObject.transform.position, indicatorObject.transform.rotation);
                    }
                }
                

            }
            else if(touch.phase == TouchPhase.Moved)
            {
                ITransformObject itObj = activeObjcets[curActiveObjectKey].GetComponentInChildren<ITransformObject>();
                Vector3 deltaPos = touch.deltaPosition;
                itObj.OnRotationUP(deltaPos.x * -1.0f);
            }
        }


    }

    void DetectGround()
    {
        Vector2 screenCenterPos = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();

        if(arRayManager.Raycast(screenCenterPos, hitInfos, TrackableType.Planes))
        {
            indicatorObject.SetActive(true);

            indicatorObject.transform.position = hitInfos[0].pose.position;
            indicatorObject.transform.rotation = hitInfos[0].pose.rotation;

            indicatorObject.transform.position += indicatorObject.transform.up * 0.01f;
        }
        else
        {
            indicatorObject.SetActive(false);
        }
    }
}
