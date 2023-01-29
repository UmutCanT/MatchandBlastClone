using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    [SerializeField] Transform camTransform;
    float camOffSet = 1f;

    public void changeCamPosition(int x, int y)
    {
        camTransform.position = new Vector3(x * .5f, y * .5f + camOffSet, camTransform.position.z);
    }

   
}
