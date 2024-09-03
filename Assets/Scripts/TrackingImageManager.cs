using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Threading.Tasks;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TrackingImageManager : MonoBehaviour
{
    public List<string> trackImageName;
    //public List<GameObject> matchingPrefebs;

    public List<string> addressableName;

    private Dictionary<string, GameObject> trackingObjects;
    private Dictionary<string, string> trackingObjectsEx;

    private ARTrackedImageManager imageManager;
    // Start is called before the first frame update

    IEnumerator LoadAsyncAssets(string addressName, Transform parentTransform)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(addressName);

        yield return handle;

        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = handle.Result;
            if(parentTransform != null)
            {
                IAddresableInformation t =  obj.GetComponentInChildren<IAddresableInformation>();
                if(t != null)
                {
                    obj.transform.SetParent(parentTransform);
                    t.OnSetAsyncHandle(handle);
                }
                else
                {
                    Debug.LogWarning($"Load Warning IAddresableInformation is null / AddresableName : { addressName }");
                }
            }
            else
            {
                Debug.LogWarning($"Load Warning parent is null / AddresableName : { addressName }");
            }
        }
        else
        {
            Debug.LogError($"Load Error AddresableName : { addressName }");
        }
    }


    void Start()
    {
        imageManager = GetComponent<ARTrackedImageManager>();

        //if (trackImageName.Count != matchingPrefebs.Count)
        if (trackImageName.Count != addressableName.Count)
            return;
        
        trackingObjects = new Dictionary<string, GameObject>();
        trackingObjectsEx = new Dictionary<string, string>();

        for(int i = 0; i < trackImageName.Count; i++)
        {
            //trackingObjects.Add(trackImageName[i], matchingPrefebs[i]);
            trackingObjectsEx.Add(trackImageName[i], addressableName[i]);
        }
        //imageManager.trackedImagesChanged += OnTrackedImage;
        imageManager.trackedImagesChanged += OnTrackedImageEx;
    }

    public void OnTrackedImageEx(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage timg in args.added)
        {
            string iname = timg.referenceImage.name;
            if (!trackingObjectsEx.ContainsKey(iname))
                continue;
            string name = trackingObjectsEx[iname];

            StartCoroutine(LoadAsyncAssets(name, timg.transform));

/*            AsyncOperationHandle handle;

            Addressables.LoadAssetAsync<GameObject>(name).Completed +=
                (AsyncOperationHandle<GameObject> obj) =>
                {
                    handle = obj;
                    if (null != obj.Result)
                    {
                        if (timg.transform.childCount < 1)
                        {
                            GameObject go = Instantiate(obj.Result, timg.transform.position, timg.transform.rotation);
                            go.transform.SetParent(timg.transform);
                        }
                    }
                };*/
        }
        foreach (ARTrackedImage timg in args.updated)
        {
            if (timg.transform.childCount > 0)
            {
                Transform child = timg.transform.GetChild(0);
                child.position = timg.transform.position;
                child.rotation = timg.transform.rotation;
            }



            //for ( int i = 0; i < timg.transform.childCount; i++)
            //{
            //    Transform child = timg.transform.GetChild(i);
            //    child.position = timg.transform.position;
            //    child.rotation = timg.transform.rotation;

            //    if(timg.trackingState == TrackingState.None || timg.trackingState == TrackingState.Limited)
            //    {
            //        child.gameObject.SetActive(false);
            //    }
            //    else
            //    {
            //        child.gameObject.SetActive(true);
            //    }

            //}
        }
        foreach (ARTrackedImage timg in args.removed)
        {
            if (timg.transform.childCount > 0)
            {
                Transform child = timg.transform.GetChild(0);
                IAddresableInformation t =  child.gameObject.GetComponentInChildren<IAddresableInformation>();
                if(t != null)
                {
                    Addressables.Release(t.OnGetAsyncHandle());
                }
                //Destroy(child.gameObject);
             
            }
        }
    }

    public void OnTrackedImage(ARTrackedImagesChangedEventArgs args)
    {
        foreach(ARTrackedImage timg in args.added)
        {
            string iname = timg.referenceImage.name;
            if (!trackingObjects.ContainsKey(iname))
                continue;
            GameObject prefeb = trackingObjects[iname];
            if(prefeb != null)
            {
                if(timg.transform.childCount < 1)
                {
                    GameObject go = Instantiate(prefeb, timg.transform.position, timg.transform.rotation);
                    go.transform.SetParent(timg.transform);
                }
            }
        }
        foreach(ARTrackedImage timg in args.updated)
        {
            if(timg.transform.childCount > 0)
            {
                Transform child = timg.transform.GetChild(0);
                child.position = timg.transform.position;
                child.rotation = timg.transform.rotation;
            }



            //for ( int i = 0; i < timg.transform.childCount; i++)
            //{
            //    Transform child = timg.transform.GetChild(i);
            //    child.position = timg.transform.position;
            //    child.rotation = timg.transform.rotation;

            //    if(timg.trackingState == TrackingState.None || timg.trackingState == TrackingState.Limited)
            //    {
            //        child.gameObject.SetActive(false);
            //    }
            //    else
            //    {
            //        child.gameObject.SetActive(true);
            //    }

            //}
        }
        foreach (ARTrackedImage timg in args.removed)
        {
            if (timg.transform.childCount > 0)
            {
                Transform child = timg.transform.GetChild(0);       
                Destroy(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
