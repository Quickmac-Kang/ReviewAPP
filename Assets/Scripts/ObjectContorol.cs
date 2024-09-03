using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContorol: MonoBehaviour, ITransformObject
{

    int objIndex;

    Outline outline;
    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int OnGetIndex()
    {
        //throw new System.NotImplementedException();
        return objIndex;
    }

    public void OnRotationUP(float angle)
    {

        transform.Rotate(transform.up, angle);
        //throw new System.NotImplementedException();
    }

    public void OnSetIndex(int index)
    {
        objIndex = index;
        //throw new System.NotImplementedException();
    }

    public void OnSetPosition(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        //transform.position = position;
        //transform.rotation = rotation;
        //throw new System.NotImplementedException();
    }

    public void OnSelected(bool isSelect)
    {
        outline = GetComponent<Outline>();
        outline.enabled = isSelect;
    }
}
