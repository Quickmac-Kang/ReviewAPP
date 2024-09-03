using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public interface ITransformObject{
    // Start is called before the first frame update
    void OnRotationUP(float angle);
    void OnSetPosition(Vector3 position, Quaternion rotation);
    void OnSetIndex(int index);
    int OnGetIndex();
    void OnSelected(bool isSelect);
}

public interface IAddresableInformation
{
    AsyncOperationHandle OnGetAsyncHandle();
    void OnSetAsyncHandle(AsyncOperationHandle handle);
}
