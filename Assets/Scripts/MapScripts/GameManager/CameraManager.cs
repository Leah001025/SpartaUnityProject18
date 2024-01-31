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
    public Vector2 mapSize;//맵의 크기
    private float cameraWidth;
    private float cameraHeight;

    private void Awake()
    {
        instance = this;
    }

    void LimitCameraArea()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, player.transform.position + cameraPosition, Time.deltaTime * cameraMoveSpeed);//카메라가 캐릭터를 부드럽게 따라다님

        float lx = mapSize.x - cameraWidth;//맵의 가로 크기에서 카메라의 넓이 빼기
        float clampX = Mathf.Clamp(mainCamera.transform.position.x, -lx + center.x, lx + center.x);//카메라의 가로 이동 값을 특정 범위로 고정 (시작지점, 최솟값, 최대값)

        float ly = mapSize.y - cameraHeight;//맵의 세로 크기에서 카메라의 높이 빼기
        float clampY = Mathf.Clamp(mainCamera.transform.position.y, -ly + center.y, ly + center.y);//카메라의 세로 이동 값을 특정 범위로 고정 (시작지점, 최솟값, 최대값)

        mainCamera.transform.position = new Vector3(clampX, clampY, mainCamera.transform.position.z);//카메라의 위치 지정
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = new Vector3(0, 0, mainCamera.transform.position.z);//z축 위치 고정

        cameraHeight = mainCamera.orthographicSize;//orthographicSize : 2D 게임에서 카메라의 높이 / 2
        cameraWidth = cameraHeight * Screen.width / Screen.height;//카메라의 비율을 계산해서 카메라의 넓이를 구함
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LimitCameraArea();
    }

}
