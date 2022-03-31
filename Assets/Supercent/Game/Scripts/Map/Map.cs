using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class Map : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera bonusRoadCamera;
    [SerializeField] CinemachineVirtualCamera bonusLastCamera;
    [SerializeField] Vector3 followOffset;
    [SerializeField] float blendSpeed = 1f;

    [Header("Run Road")]
    public Transform runRoad;
    
    [Header("Charge Road")]
    public Transform chargeRoad;

    [Header("Kick Road")]
    public Transform kickRoad;
    public Color startColor, endColor;

    [Header("Bonus Floor")]
    public Transform finishBlocks;

    public Transform chargeEventBox;

    public List<BlockLine> blockLines;
    public List<Block> allBlocks;

    public List<KickFloor> kickFloors = new List<KickFloor>();
    public BonusFloor bonusFloor;
    public float bonusTime = 5f;

    public static float toChargeDistance = 0f;

    [ContextMenu("Run Road Sorting")]
    void RunRoadSorting()
    {
        if (runRoad.childCount > 0)
        {
            Vector3 floorPosition = Vector3.zero;
            for (int i = 0; i < runRoad.childCount; i++)
            {
                Transform floor = runRoad.GetChild(i);
                floor.position = floorPosition;
                floorPosition.z += 10f;
            }

            chargeEventBox.position = new Vector3(0f, 0f, floorPosition.z - 10f);

            if (chargeRoad.childCount > 0)
            {
                for (int i=0; i<chargeRoad.childCount; i++)
                {
                    Transform floor = chargeRoad.GetChild(i);
                    floor.position = floorPosition;
                    floorPosition.z += 10f;
                }

                finishBlocks.transform.position = new Vector3(finishBlocks.transform.position.x,
                                                              finishBlocks.transform.position.y,
                                                              floorPosition.z - 10f);


                if (kickRoad.childCount > 0)
                {
                    floorPosition.z += 5f;
                    
                    for (int i = 0; i < kickRoad.childCount; i++)
                    {
                        Transform floor = kickRoad.GetChild(i);
                        floor.position = floorPosition;
                        floorPosition.z += 20f;
                    }

                    if (bonusFloor)
                    {
                        bonusFloor.transform.position = floorPosition;
                    }
                }
            }
        }
    }

    private void Start()
    {
        float startMultiple = 1f;
        for (int i = 0; i < kickRoad.childCount; i++)
        {
            Transform floor = kickRoad.GetChild(i);

            KickFloor kickFloor = floor.GetComponent<KickFloor>();
            if (kickFloor) {
                kickFloor.meshRenderer.material.color = Color.Lerp(startColor, endColor, i / (float)kickRoad.childCount);
                kickFloor.text.text = "x" + startMultiple.ToString("F1");

                kickFloors.Add(kickFloor);
                startMultiple += 0.5f;
            }
        }


        toChargeDistance = chargeEventBox.position.z;

        GameManager.instance.doKickState += () =>
        {
            bonusRoadCamera.Priority = 11;
        };

        GameManager.instance.doBonusUpdate += () => {
            bool isUpdated = false;
            if (bonusFloor.isChecked)
            {
                bonusLastCamera.Priority = 12;
            }
            else
            {
                Transform target = GetCameraTarget(ref isUpdated);

                if (target)
                {
                    Vector3 targetPosition = target.position + followOffset;

                    bonusRoadCamera.transform.position = Vector3.Lerp(bonusRoadCamera.transform.position,
                                                                      targetPosition,
                                                                      Time.deltaTime * blendSpeed);
                }
            }

            if (isUpdated)
            {
                bonusTime = Mathf.Clamp(bonusTime + 3f, 0f, 5f);
            }

            bonusTime -= Time.deltaTime;

            if (bonusTime <= 0f)
            {
                GameManager.instance.ChangeState(EGameState.CLEAR);
            }
        };
    }

    public Transform GetCameraTarget(ref bool isUpdated) {
        Transform target = null;
        isUpdated = false;

        for (int i = 0; i < kickFloors.Count; i++)
        {
            KickFloor floor = kickFloors[i];

            if (target == null)
            {
                target = floor.transform;
                isUpdated = true;
            }
            else if (floor.isChecked && target.position.z < floor.transform.position.z)
            {
                target = floor.transform;
                isUpdated = true;
            }
        }

        if (bonusFloor.isChecked)
        {
            target = bonusFloor.transform;
        }


        return target;
    }


    public void FeverColor()
    {

    }

}
