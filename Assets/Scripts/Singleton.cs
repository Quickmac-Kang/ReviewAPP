using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if( instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }
    protected virtual void Awake()
    {
        //print("Awake : " + typeof(T).Name);
        if(transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else 
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
