using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Camera minimapCamera;

    [SerializeField] GameObject player;
    [SerializeField] float cameraMoveSpeed = 5;
    private Vector3 cameraPosition;//카메라 z값
    public Camera mainCamera;
    public Vector2 center;
    public Vector2 mapSize;
    private float cameraWidth;
    private float cameraHeight;

    private void Awake()
    {
        instance = this;
    }

    void LimitCameraArea()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, player.transform.position + cameraPosition, Time.deltaTime * cameraMoveSpeed);

        float lx = mapSize.x - cameraWidth;
        float clampX = Mathf.Clamp(mainCamera.transform.position.x, -lx + center.x, lx + center.x);

        float ly = mapSize.y - cameraHeight;
        float clampY = Mathf.Clamp(mainCamera.transform.position.y, -ly + center.y, ly + center.y);

        mainCamera.transform.position = new Vector3(clampX, clampY, mainCamera.transform.position.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = new Vector3(0, 0, mainCamera.transform.position.z);

        cameraHeight = mainCamera.orthographicSize;
        cameraWidth = cameraHeight * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LimitCameraArea();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }
}
