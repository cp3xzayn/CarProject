using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Car : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    [SerializeField] float maxMotorTorque = 1000;
    [SerializeField] float maxSteeringAngle = 40;

    [SerializeField] WheelCollider wheelFL;
    [SerializeField] WheelCollider wheelFR;
    [SerializeField] WheelCollider wheelBL;
    [SerializeField] WheelCollider wheelBR;

    [SerializeField] Transform wheelFLTrans;
    [SerializeField] Transform wheelFRTrans;
    [SerializeField] Transform wheelBLTrans;
    [SerializeField] Transform wheelBRTrans;

    void Update()
    {
        //wheelcolliderの回転速度に合わせてタイヤモデルを回転させる
        wheelFLTrans.Rotate(wheelFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        wheelFRTrans.Rotate(wheelFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        wheelBLTrans.Rotate(wheelBL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        wheelBRTrans.Rotate(wheelBR.rpm / 60 * 360 * Time.deltaTime, 0, 0);

        //wheelcolliderの角度に合わせてタイヤモデルを回転する（フロントのみ）
        wheelFLTrans.localEulerAngles = new Vector3(wheelFLTrans.localEulerAngles.x, wheelFL.steerAngle - wheelFLTrans.localEulerAngles.z, wheelFLTrans.localEulerAngles.z);
        wheelFRTrans.localEulerAngles = new Vector3(wheelFRTrans.localEulerAngles.x, wheelFR.steerAngle - wheelFRTrans.localEulerAngles.z, wheelFRTrans.localEulerAngles.z);

    }

    public void FixedUpdate()
    {
        float motor = -maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
    }
}

[Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; //駆動輪か?
    public bool steering; //ハンドル操作をしたときに角度が変わるか？
}
