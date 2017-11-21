using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LanternMovement : MonoBehaviour {

    public FlowField flowField;
	
	void Update () {
        if (flowField == null)
        {
            Debug.LogError("No flow field set for " + name);
            return;
        }

        Vector3 forceVector = flowField.GetFlowVectorAt(transform.position);
        //Debug.Log(forceVector*100);
        transform.position += forceVector;
        AddRotation(forceVector);
        return;

	}

    void AddRotation(Vector3 fwdVec)
    {
        Vector3 wobbleVector = Vector3.Cross(Vector3.up, fwdVec);
        float wobbleForce = Mathf.Sin(Time.time * 10) * fwdVec.sqrMagnitude * flowField.wobbleStrength;
        Quaternion wobble = Quaternion.AngleAxis(Mathf.Clamp(wobbleForce,0,10), wobbleVector);

        Quaternion currentRot = Quaternion.LookRotation(flowField.FlattenVector(transform.forward));
        Quaternion desiredRot = Quaternion.LookRotation(wobbleVector);
        Quaternion twist = Quaternion.Slerp(currentRot,desiredRot,flowField.twistStrength);
        transform.rotation = wobble * twist;
    }

}
