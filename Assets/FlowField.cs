using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FlowField : MonoBehaviour {
    //public Transform[] lanterns;

    [Header("Size")]
    public Vector2 dimensions = new Vector2(10, 10);
    [Header("Strength")]
    public float maxForce = 0.1f;
    public float wobbleStrength = 500;
    public float twistStrength = 0.005f;
    public float spiralDistance = 100f;
    [Header("Noise and Waves")]
    [Range(0.01f,10)]
    public float noiseScale = 0.1f;
    [Range(0,1)]
    public float noiseAngleStrength = 0.35f;
    public float waveInterval = 0.7f;
    //public float waveScale = 10;
    public float waveForceOffset = 1.5f;
    [Header("Debug Settings")]
    public bool drawDebug = true;
    public float resolution = 100;
    [Range(0,100)]
    public float debugVectorScale = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (drawDebug)
            DrawField();
    }
#endif

    void DrawField()
    {
        for (float x = dimensions.x*-0.5f; x < dimensions.x * 0.5f; x += dimensions.x / resolution)
        {
            for (float y = dimensions.y * -0.5f; y < dimensions.y * 0.5f; y += dimensions.y / resolution)
            {
                Vector3 targetPos = transform.position + new Vector3(x, 0, y);
                //Vector3 vec = ;
                Debug.DrawRay(targetPos, GetFlowVectorAt(targetPos)*debugVectorScale, Color.yellow);
            }
        }
    }

    public Vector3 GetFlowVectorAt(Vector3 pos)
    {
        var toCenter = FlattenVector(transform.position - pos);
        //Make floating objects spiral around center as we approach it
        var spiralForce = FlattenVector(Vector3.Cross(toCenter, transform.up));

        var noise = Mathf.PerlinNoise(pos.x, pos.z);
        Quaternion noisyRot = Quaternion.AngleAxis(Mathf.Lerp(-noiseAngleStrength*180, noiseAngleStrength*180, noise), transform.up);

        float t = Mathf.Clamp(Mathf.Pow((1 / toCenter.sqrMagnitude) * spiralDistance,0.5f),0,2);
        var finalForce = Vector3.LerpUnclamped(Vector3.Normalize(toCenter), Vector3.Normalize(spiralForce), t).normalized * GetForce();
        //probably don't need another flattenVector here
        return noisyRot * FlattenVector(finalForce);
    }

    float GetForce()
    {
        //return max * (Mathf.Sin(1 / (toCenter.sqrMagnitude * waveScale)) + 1.5f);

        return maxForce * (Mathf.Sin(Time.time*waveInterval)*(1-waveForceOffset) + waveForceOffset);
    }

    public Vector3 FlattenVector(Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }
}
