using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArrowEffect : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    [SerializeField] private Transform pointC;
    [SerializeField] private GameObject prefab;

    [SerializeField] private float lerpSpeed = 0.5f;
    [SerializeField][Range(2, 10)] private int numPoints = 5;

    private float[] interpolateAmounts;
    [SerializeField] private Transform[] points;
    private bool isDisabled;
    private bool _points;

    private void Start()
    {
        _points = this.transform.GetChild(0).gameObject.activeSelf;
        interpolateAmounts = new float[numPoints];
        points = new Transform[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            interpolateAmounts[i] = i / (float)(numPoints - 1);
            points[i] = Instantiate(prefab.transform);
            points[i].SetParent(this.transform.GetChild(0));
            //points[i] = Instantiate(pointAB_BC, Vector3.zero, Quaternion.identity);
        }

    }

    private void Update()
    {
        _points = this.transform.GetChild(0).gameObject.activeSelf;
        if (_points)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointC.position = mousePosition;

            for (int i = 0; i < numPoints; i++)
            {
                interpolateAmounts[i] = (interpolateAmounts[i] + lerpSpeed * Time.deltaTime) % 1f;
                points[i].position = QuadraticLerp(pointA.position, pointB.position, pointC.position, interpolateAmounts[i]);
            }
        }
    }

    private Vector2 QuadraticLerp(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 ab = Vector2.Lerp(a, b, t);
        Vector2 bc = Vector2.Lerp(b, c, t);

        return Vector2.Lerp(ab, bc, t);
    }
}
