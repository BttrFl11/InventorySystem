using UnityEngine;
using System.Collections;

public class AnimCurve : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _multY;
    [SerializeField] private float _speed;

    private int _index;

    private void Update()
    {
        Move();

        if ((transform.position - new Vector3(0, _curve.keys[_index].value * _multY, 0)).magnitude <= 0.1f)
        {
            _index++;

            if (_index == _curve.length)
                _index = 0;
        }
    }

    private void Move()
    {
        transform.Translate((new Vector3(0, _curve.keys[_index].value * _multY, 0) - transform.position) * Time.deltaTime * _speed);
    }
}
