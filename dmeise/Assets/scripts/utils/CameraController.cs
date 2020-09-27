using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera cam;
    private Vector2 xyScale;
    private Vector3 prevMousePos;
    private Vector2 halfScreenSize;
    private bool _moveEnabled;
    private Vector3 _velocity;
    private float damping = 5;
    private Vector3 _target;
    private Vector3 _offset;
    [SerializeField] private Bounds bounds;

    // Start is called before the first frame update
    void Start() {
        Vector3 curPos = transform.position;
        _offset = Vector3.zero; //prevMousePos - curPos;
        _target = curPos + _offset;
        cam = GetComponent<Camera>();
        float aspect = Screen.width / (float)Screen.height;
        float cameraOrthographicSize = cam.orthographicSize;
        xyScale = 2f * new Vector2(aspect * cameraOrthographicSize / Screen.width, cameraOrthographicSize / Screen.height);
        prevMousePos = new Vector3(Input.mousePosition.x * xyScale.x, Input.mousePosition.y * xyScale.y, curPos.z);
        halfScreenSize = new Vector2(cameraOrthographicSize * aspect, cameraOrthographicSize);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);
    }

    // Update is called once per frame
    void Update() {
        if (!_moveEnabled) {
            _moveEnabled = Input.GetMouseButtonDown(1);
        }

        if (_moveEnabled) {
            _moveEnabled = !Input.GetMouseButtonUp(1);
        }
        Vector3 mousePosition = new Vector3(
            Input.mousePosition.x * xyScale.x,
            Input.mousePosition.y * xyScale.y,
            transform.position.z);

        if (_moveEnabled) {
            _velocity = prevMousePos - mousePosition;
        } else {
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, Time.deltaTime * damping);
        }
        Vector3 targetWithoutOffset = _target - _offset + _velocity;
        float clampedX = Mathf.Clamp(targetWithoutOffset.x, bounds.min.x + halfScreenSize.x, bounds.max.x - halfScreenSize.x);
        float clampedY = Mathf.Clamp(targetWithoutOffset.y, bounds.min.y + halfScreenSize.y, bounds.max.y - halfScreenSize.y);
        _target = new Vector3(clampedX, clampedY, targetWithoutOffset.z) + _offset;
        prevMousePos = mousePosition;
        transform.position = _target - _offset;
    }
}
