using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using Unity.Collections;

public class MainFaceManger : MonoBehaviour
{

    public GameObject tapFacialSelectUI;
    public Button selectButton;
    public Button clearButton;
    public Button increseButton;
    public Button decreseButton;
    public Text vertexIndex;
    public GameObject checkObject;
    public List<GameObject> facialObjects;
    //[SerializeField]
    //private FacialFeatures FacialSelect;

    private ARFaceManager afm;
    private ARCoreFaceSubsystem subSys;
    private int verNum = 0;
    private int verCount = 468;
    private GameObject tobj;
    private int selectFeatureIndex = 0;
    private Dictionary<int, GameObject> activeObjects;

    // Start is called before the first frame update
    void Start()
    {
        tapFacialSelectUI.SetActive(false);
        selectButton.onClick.AddListener(OnClickSelect);
        clearButton.onClick.AddListener(OnClickClear);
        afm = GetComponent<ARFaceManager>();
        afm.facesChanged += OnDetectFaceAll;
        subSys = (ARCoreFaceSubsystem)afm.subsystem;

        increseButton.onClick.AddListener(OnClickIncrese);
        decreseButton.onClick.AddListener(OnClickDecrese);
        tobj = Instantiate(checkObject);
        tobj.SetActive(false);

        activeObjects = new Dictionary<int, GameObject>();
    }

    void OnClickIncrese()
    {
        verNum = Mathf.Min(++verNum, verCount - 1);
        vertexIndex.text = verNum.ToString();
    }
    void OnClickDecrese()
    {
        verNum = Mathf.Max(--verNum, 0);
        vertexIndex.text = verNum.ToString();
    }

    void OnDetectFaceAll(ARFacesChangedEventArgs args)
    {
        if (args.updated.Count > 0)
        {
            //int num = int.Parse(vertexIndex.text);
            //Vector3 vertPosiotion = args.updated[0].vertices[num];

            //vertPosiotion = args.updated[0].transform.TransformPoint(vertPosiotion);

            //tobj.SetActive(true);
            //tobj.transform.position = vertPosiotion;

            foreach(KeyValuePair<int, GameObject> i in activeObjects)
            {
                Vector3 vertPosiotion = args.updated[0].vertices[i.Key];
                vertPosiotion = args.updated[0].transform.TransformPoint(vertPosiotion);
                i.Value.SetActive(true);
                i.Value.transform.position = vertPosiotion;
            }

        }
        else if (args.removed.Count > 0)
        {

            foreach (KeyValuePair<int, GameObject> i in activeObjects)
            {
                i.Value.SetActive(false);
            }

        }
    }
    public void OnClickClear()
    {
        foreach(KeyValuePair<int,GameObject> i in activeObjects)
        {
            Destroy(i.Value);
        }
        activeObjects.Clear();
    }

    public void OnClickSelect()
    {
        if(tapFacialSelectUI.activeInHierarchy)
        {
            tapFacialSelectUI.SetActive(false);
            selectButton.GetComponentInChildren<Text>().text = "Select";
            
        }
        else
        {
            tapFacialSelectUI.SetActive(true);
            selectButton.GetComponentInChildren<Text>().text = "Close";
        }
    }

    public void OnClickFacialFeatures(int idx)
    {
        selectFeatureIndex = idx;
    }

    public void OnClickSelectParts(int idx)
    {
        if( 0 == selectFeatureIndex)
        {
            //23 253
            int num = 23;
            GameObject go = Instantiate(facialObjects[idx]);
            if (activeObjects.ContainsKey(num))
            {
                Destroy(activeObjects[num]);
                activeObjects.Remove(num);
                activeObjects.Add(num, go);
            }
            else
                activeObjects.Add(num, go);
            num = 253;
            go = Instantiate(facialObjects[idx]);
            if (activeObjects.ContainsKey(num))
            {
                Destroy(activeObjects[num]);
                activeObjects.Remove(num);
                activeObjects.Add(num, go);
            }
            else
                activeObjects.Add(num, go);
        }
        else if( 1 == selectFeatureIndex)
        {
            //4
            int num = 4;
            GameObject go = Instantiate(facialObjects[idx]);
            if (activeObjects.ContainsKey(num))
            {
                Destroy(activeObjects[num]);
                activeObjects.Remove(num);
                activeObjects.Add(num, go);
            }
            else
                activeObjects.Add(num, go);
        }
        else if ( 2 == selectFeatureIndex)
        {
            int num =11;
            GameObject go = Instantiate(facialObjects[idx]);
            if (activeObjects.ContainsKey(num))
            {
                Destroy(activeObjects[num]);
                activeObjects.Remove(num);
                activeObjects.Add(num, go);
            }
            else
                activeObjects.Add(num, go);
        }
        else if ( 3 == selectFeatureIndex)
        {
            int num = 175;
            GameObject go = Instantiate(facialObjects[idx]);
            if (activeObjects.ContainsKey(num))
            {
                Destroy(activeObjects[num]);
                activeObjects.Remove(num);
                activeObjects.Add(num, go);
            }
            else
                activeObjects.Add(num, go);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    //void OnClickEquipParts( )
}
