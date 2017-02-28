using UnityEngine;
using System.Collections;

public class csPixelCamera : MonoBehaviour
{
    [SerializeField]
    Camera m_camera;

    void Awake()
    {
        m_camera.orthographic = true;
        m_camera.orthographicSize = (Screen.height / 2f);
    }
}
