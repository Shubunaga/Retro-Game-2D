using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialogTrigger : MonoBehaviour
{
    [TextArea(3,10)]
    public string[] sentences;
    private UIDialog manager;
    
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<UIDialog>().DisplayControls(sentences);
    }
}
