using UnityEngine;

public class Pointer : MonoBehaviour
{
    private Camera _camera;
    private LineRenderer _line;
    private MeshRenderer _mesh;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _camera = Camera.main;
        _line.positionCount = 2;
    }

    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(transform.position, ray.direction * 100, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            _line.SetPosition(0, transform.position + Vector3.forward);
            _line.SetPosition(1, hit.point);

            if (_mesh == null)
                _mesh = hit.collider.GetComponent<MeshRenderer>();

            ChangeMaterialColor(_mesh, Color.red);
        }
        else if (_mesh != null)
        {
            ChangeMaterialColor(_mesh, Color.white);

            _mesh = null;
        }
    }

    private void ChangeMaterialColor(MeshRenderer mesh, Color color)
    {
        mesh.material.color = color;
    }
}
