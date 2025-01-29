using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public bool keyFound;

    private void Awake()
    {
        //if (Instance != null)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
