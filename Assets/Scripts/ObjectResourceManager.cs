using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ObjectResourceManager : MonoBehaviour , IAddresableInformation
{
    AsyncOperationHandle ableHandle;

    public AsyncOperationHandle OnGetAsyncHandle()
    {
        return ableHandle;
    }

    public void OnSetAsyncHandle(AsyncOperationHandle handle)
    {
        ableHandle = handle;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
