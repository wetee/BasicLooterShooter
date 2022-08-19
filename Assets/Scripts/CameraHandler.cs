using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraHandler : MonoBehaviour{

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float orthographicSize;
    private float targetOrthographicSize;

    private void Start() {      
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    private void Update() {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(x, y).normalized;
        float cameraSpeed = 20f;

        transform.position += cameraSpeed * Time.deltaTime * moveDirection;
    }
    private void HandleZoom() {
        float zoomAmount = -2f;
        float minOrthographicSize = 10f;
        float maxOrthographicSize = 30f;

        targetOrthographicSize += Input.mouseScrollDelta.y * zoomAmount;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }
}
