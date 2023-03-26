using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallBack : MonoBehaviour
{
    // Start is called before the first frame update
    bool isFirstUpdate = true;
    private void Update()
    {
        if(isFirstUpdate)
        {
            isFirstUpdate= false;
            Loader.LoadCallBack();
        }
    }
}
