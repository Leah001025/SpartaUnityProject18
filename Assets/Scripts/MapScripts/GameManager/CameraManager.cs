using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Camera minimapCamera;

    [SerializeField] GameObject player;
    [SerializeField] float cameraMoveSpeed = 5;
    private Vector3 cameraPosition;//ī�޶� z��
    public Camera mainCamera;
    public Vector2 center;
    public Vector2 mapSize;//���� ũ��
    private float cameraWidth;
    private float cameraHeight;

    private void Awake()
    {
        instance = this;
    }

    void LimitCameraArea()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, player.transform.position + cameraPosition, Time.deltaTime * cameraMoveSpeed);//ī�޶� ĳ���͸� �ε巴�� ����ٴ�

        float lx = mapSize.x - cameraWidth;//���� ���� ũ�⿡�� ī�޶��� ���� ����
        float clampX = Mathf.Clamp(mainCamera.transform.position.x, -lx + center.x, lx + center.x);//ī�޶��� ���� �̵� ���� Ư�� ������ ���� (��������, �ּڰ�, �ִ밪)

        float ly = mapSize.y - cameraHeight;//���� ���� ũ�⿡�� ī�޶��� ���� ����
        float clampY = Mathf.Clamp(mainCamera.transform.position.y, -ly + center.y, ly + center.y);//ī�޶��� ���� �̵� ���� Ư�� ������ ���� (��������, �ּڰ�, �ִ밪)

        mainCamera.transform.position = new Vector3(clampX, clampY, mainCamera.transform.position.z);//ī�޶��� ��ġ ����
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = new Vector3(0, 0, mainCamera.transform.position.z);//z�� ��ġ ����

        cameraHeight = mainCamera.orthographicSize;//orthographicSize : 2D ���ӿ��� ī�޶��� ���� / 2
        cameraWidth = cameraHeight * Screen.width / Screen.height;//ī�޶��� ������ ����ؼ� ī�޶��� ���̸� ����
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LimitCameraArea();
    }

}
