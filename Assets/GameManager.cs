using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private Camera _camera;

    [SerializeField]
    private float zoomStep;
    [SerializeField]
    private float minZoom;
    [SerializeField]
    private float maxZoom;

    private Vector3 dragOrigin;

    private void Awake() {
        _camera = Camera.main;
    }

    void Update() {
        PanCamera();
        Zoom();
        
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0f) {
            
        }
    }
    
    private void Zoom() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f) {
            float newSize;
            if (scroll > 0f) {
                newSize = _camera.orthographicSize - zoomStep;
            }
            else {
                newSize = _camera.orthographicSize + zoomStep;
            }
            
            _camera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
    }

    private void PanCamera() {
        //  Right click
        if (Input.GetMouseButtonDown(1)) {
            dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
        }

        //  Drag mouse while holding right click
        if (Input.GetMouseButton(1)) {
            Vector3 diff = dragOrigin - _camera.ScreenToWorldPoint(Input.mousePosition);

            _camera.transform.position += diff;
        }
    }
}
