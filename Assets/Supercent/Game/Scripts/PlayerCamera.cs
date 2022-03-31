using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class PlayerCamera : MonoBehaviour
{
    [SerializeField] public CinemachineVirtualCamera runCamera;
    [SerializeField] public CinemachineVirtualCamera feverCamera;
    [SerializeField] Vector3 minOffset, maxOffset;
    [SerializeField] float minHeight, maxHeight;

    CinemachineTransposer body;
    CinemachineComposer aim;



    private void Start() {
        body = runCamera.GetCinemachineComponent<CinemachineTransposer>();
        aim = runCamera.GetCinemachineComponent<CinemachineComposer>();
    }


    

    public void SetCameraOffset(float stackHeight) {
        float percent = (stackHeight - minHeight) / (maxHeight - minHeight);
        percent = Mathf.Clamp(percent, 0f, 1f);

        //body.m_FollowOffset.x = X;
        body.m_FollowOffset.y = Mathf.Lerp(body.m_FollowOffset.y, minOffset.y + (maxOffset.y - minOffset.y) * percent, Time.deltaTime);
        body.m_FollowOffset.z = Mathf.Lerp(body.m_FollowOffset.z, minOffset.z + (maxOffset.z - minOffset.z) * percent, Time.deltaTime);
    }
}
