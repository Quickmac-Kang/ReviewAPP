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

    //������ ������Ʈ�� �����ϴ� ��ųʸ�. 
    Dictionary<int, GameObject> activeObjcets;



    static int createObjectKey = 1; //������ ������Ʈ�� ����Ű 1���� �ο��ϵ����Ѵ�.
    int selectCreateModelIndex; //List<string> kitchenNames �� �����Ǵ� �ε��� 
    
    int curActiveObjectKey = 0; //  Dictionary<int, GameObject>() �� �����ϴ� Ű.

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

    //AR Plane ���͸��� ���� ���� �̿��� ���̰� �Ǵ� �Ⱥ��̰� ó��. 
    //* ���� �����Ǵ� Plane�� ������� �ʴ´�. 
    public void SetPlaneVisibility(bool isVisible)
    {
        // AR Plane Manager���� ������ ��� ��鿡 �����մϴ�.
        foreach (ARPlane plane in arPlaneManager.trackables)
        {
            // �� ����� Mesh Renderer�� �����ɴϴ�.
            MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();

            // Mesh Renderer�� Material�� ������ �����ɴϴ�.
            Color color = meshRenderer.material.color;

            // isVisible �Ķ���Ϳ� ���� ���İ��� �����մϴ�.
            color.a = isVisible ? 0.5f : 0f;  // �������ϰ� ����ų� ���� �����ϰ� ����ϴ�.

            // ����� ������ �ٽ� Material�� �����մϴ�.
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
            debugText.text = "���� ��� �����.";
        }
        else
        {
            btnCreateEditToggle.GetComponentInChildren<Text>().text = "Edit";
            isCreate = true; 
            debugText.text = "���� ��� �����.";

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

        if (touch.position.y < 300) // UI �κ��� ��ġ ������ �ȵǵ��� ����. (����: ȭ�� �����ϴ��� 0,0 �̴�.)
            return;

        //�� ���� UI�� Ȱ��ȭ ���¿����� ��ġ ������ �۵����� �ʵ��� �Ѵ�.
        if (selectModelUI.activeInHierarchy) 
            return;

        // �������� ������带 �и�.
        if (isCreate)
        {
            
            if (touch.phase == TouchPhase.Began)
            {
                //���̷� ���������� ���� ������ ������Ʈ�� �ִ��� Ȯ���ϰ� �ִٸ�, �������� �ʴ´�.
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
                curActiveObjectKey = createObjectKey; // �������� ������ ������Ʈ Ű�� active ������Ʈ�� ����.
                createObjectKey++;
            }
        }
        else
        {
            if(touch.phase == TouchPhase.Began)
            {
                // 2���� �������. 
                // curActiveObjectKey �� �ٸ� ������Ʈ�� ������ ��� curActiveObjectKey�� ������ ������Ʈ�� �����Ѵ�.
                // ���̿� ������Ʈ�� ���õ��� �ʾҴٸ�, curActiveObjectKey�� ������Ʈ�� �̵���Ų��.

                RaycastHit hits;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hits, Mathf.Infinity))
                {
                    //Plane ���� ������Ʈ�� �����ǹǷ� Ray �浹�� �����ȴ�, ������ ������Ʈ�� ������� �Ǻ������ �Ѵ�. 
                    //interface�� �̿��Ͽ� ������Ʈ�� �����س���.
                    ITransformObject itObj = hits.collider.GetComponent<ITransformObject>();
                    if (itObj != null)
                    {
                        if (curActiveObjectKey != itObj.OnGetIndex())
                        {
                            //������ ���õ� ������Ʈ�� �ƿ������� ��Ȱ��ȭ ��Ų��.
                            ITransformObject t = activeObjcets[curActiveObjectKey].GetComponentInChildren<ITransformObject>();
                            t.OnSelected(false);

                            //���� ����� ������Ʈ�� �ε����� ��Ƽ�� �ε������ϰ�, �ƿ������� Ȱ��ȭ ��Ų��.
                            curActiveObjectKey = itObj.OnGetIndex();
                            itObj.OnSelected(true); // �ƿ����� Ȱ��ȭ
                        }
                        else
                        {
                            itObj.OnSelected(true); // �ƿ����� Ȱ��ȭ
                        }
                        debugText2.text = "��ġ2 : " + ((int)touch.position.x).ToString() + " / " + ((int)touch.position.y).ToString() + " / Idx: " + itObj.OnGetIndex().ToString();
                    }
                    else //
                    {
                        itObj = activeObjcets[curActiveObjectKey].GetComponentInChildren<ITransformObject>();
                        debugText2.text = "��ġ3 : " + ((int)touch.position.x).ToString() + " / " + ((int)touch.position.y).ToString() + " / Idx: " + itObj.OnGetIndex().ToString();
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
