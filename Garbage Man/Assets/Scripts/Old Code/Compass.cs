using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public RawImage compassImg;
    public Transform playerPos;

    private void Update()
    {
        compassImg.uvRect = new Rect(playerPos.localEulerAngles.y / 360f, 0f, 1f, 1f);
    }
}
