using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour
{
    float direction = 1;
    public int targ_ = 60;
    public int vsync_ = 1;
    //GameObject go;
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        Debug.Log("vsync: " + QualitySettings.vSyncCount + "framerate: " + Application.targetFrameRate);
    }

    // Update is called once per frame
    void Update()
    {
         if (transform.position.x <= -6)
        { direction = 1; }
        else if (transform.position.x >= 2)
        { direction = -1; }
 
        transform.Translate(direction * Time.deltaTime, 0, 0);
 
        if(Application.targetFrameRate != targ_)
        {
            Application.targetFrameRate = targ_;
            Debug.Log("vsync: " + QualitySettings.vSyncCount + "framerate: " + Application.targetFrameRate);
        }
 
        if (QualitySettings.vSyncCount != vsync_)
        {
            QualitySettings.vSyncCount = vsync_;
            Debug.Log("vsync: " + QualitySettings.vSyncCount + "framerate: " + Application.targetFrameRate);
        }
    }
}
