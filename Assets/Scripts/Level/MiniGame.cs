using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniGame : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform pointer;
    [SerializeField] private float pointerSpeed = 5.0f;
    [SerializeField] private float pointerAmplitude = 2.5f;

    [SerializeField] private List<float> hitRanges;
    [SerializeField] private List<float> hitMultipliers;


    private float timer;

    #endregion



    #region Unity lifecycle

    private void Update()
    {
        timer += Time.deltaTime * pointerSpeed;
        Vector3 localPosition = pointer.localPosition;
        pointer.localPosition = new Vector3(Mathf.Sin(timer) * pointerAmplitude, localPosition.y, localPosition.z);
    }

    #endregion



    #region Methods

    public void Initialize()
    {
        timer = Random.Range(-10.0f, 10.0f);

        //ToDo set bar scales dependent on hitRanges
    }


    public float EvaluateInteraction()
    {
        float pointerDiff = Mathf.Abs(pointer.localPosition.x / pointerAmplitude);

        for (int i = 0; i < hitRanges.Count; i++)
        {
            if (pointerDiff <= hitRanges[i])
            {
                return hitMultipliers[i];
            }
        }

        return 1.0f;
    }

    #endregion
}
