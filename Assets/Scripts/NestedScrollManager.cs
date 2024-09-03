using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NestedScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Transform contentTR;
    public Slider tapSlider;
    public RectTransform[] btnRect, btnImageRect;


    //public Scrollbar scrollbar; //inspacter 참조  
    private Scrollbar scrollbar;  // 코드 레벨 참조.


    const int SIZE = 4;
    float[] pos = new float[SIZE];
    float distance, curPos, targetPos;
    bool isDrag = false;
    int targetIndex = 0;


    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        //GameManager.Instance.Test();

        // 코드 참조 형태로 테스트를 위한 코드. 
        Transform child = transform.Find("Scrollbar Horizontal");
        scrollbar = child.GetComponent<Scrollbar>();
        //scrollbar = GetComponentInChildren<Scrollbar>();

        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++)
            pos[i] = distance * i;
    }

    float SetPos()
    {
        for (int i = 0; i < SIZE; i++)
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        return 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        curPos = SetPos();
    }

    public void OnDrag(PointerEventData eventData)
    {

        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        isDrag = false;
        targetPos = SetPos();
        print(curPos + " / " + targetPos + " / " + targetIndex);


        if (curPos == targetPos)
        {
            if (eventData.delta.x > 15 && curPos - distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;
            }
            else if (eventData.delta.x < -15 && curPos + distance <= 1.01f)
            {
                ++targetIndex;
                targetPos = curPos + distance;
            }
        }

        for (int i = 0; i < SIZE; i++)
        {
            if (contentTR.GetChild(i).GetComponent<ScrollScript>() && curPos != pos[i] && targetPos == pos[i])
            {
                contentTR.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1;
            }
        }

        //throw new System.NotImplementedException();
    }
    // Update is called once per frame
    void Update()
    {
        tapSlider.value = scrollbar.value;

        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 10.0f * Time.deltaTime);

            for (int i = 0; i < SIZE; i++)
            {
                btnRect[i].sizeDelta = new Vector2(i == targetIndex ? 360 : 180, btnRect[i].sizeDelta.y);
            }
        }

        if (Time.time < 0.1f)
            return;

        for (int i = 0; i < SIZE; i++)
        {
            Vector3 btnTargetPos = btnRect[i].anchoredPosition3D;
            Vector3 btnTargetScale = Vector3.one;
            bool textActive = false;

            if (i == targetIndex)
            {
                btnTargetPos.y = -0.23f;
                btnTargetScale = new Vector3(1.2f, 1.2f, 1);
                textActive = true;
            }
            btnImageRect[i].anchoredPosition3D = Vector3.Lerp(btnImageRect[i].anchoredPosition3D, btnTargetPos, 15.0f * Time.deltaTime);
            btnImageRect[i].localScale = Vector3.Lerp(btnImageRect[i].localScale, btnTargetScale, 15.0f * Time.deltaTime);
            btnImageRect[i].transform.GetChild(0).gameObject.SetActive(textActive);
        }
    }

    public void TabClick(int n)
    {
        targetIndex = n;
        targetPos = pos[n];
    }

    public void SceneLoad(string sceneName)
    {
        //GameManager.Instance.SceneLoad(sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
