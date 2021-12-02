using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionIconHandler : MonoBehaviour {
    public Camera camera;
    public GameObject actionIcon;

    public List<GameObject> actionIcons = new List<GameObject>();

    private Transform _canvas;

    [SerializeField]
    private float xOffset = 50f;
    [SerializeField]
    private float scaleBase = 5f;

    private void Awake() {
        _canvas = FindObjectOfType<Canvas>().transform;
    }

    public IEnumerator Spawn(Sprite sprite, GameObject position) {
        //  Don't render any ui elements if we are not seen by any camera
        if (!position.gameObject.GetComponent<SpriteRenderer>().isVisible) {
            yield break;
        }
        
        GameObject instance = Instantiate(actionIcon, _canvas);
        ActionIcon icon = instance.GetComponent<ActionIcon>();
        icon.sprite = sprite;
        icon.position = position.transform;
        
        actionIcons.Add(instance);

        // for (float t = 0f; t < 5f; t += Time.deltaTime) {
        //     Color c = actionIcon;
        //     c.a = c.a - 0.1f;
        //     actionIcon.color = c;
        //
        //     yield return new WaitForSeconds(0.1f);
        // }

        yield return new WaitForSeconds(5f);

        actionIcons.Remove(instance);
    }

    private void Update() {
        foreach (GameObject instance in actionIcons) {
            ActionIcon icon = instance.GetComponent<ActionIcon>();
            Vector3 pos = icon.position.transform.position;
            Vector3 worldToScreen = camera.WorldToScreenPoint(pos);
            
            float orthographicSize = camera.orthographicSize;
            float scale = scaleBase / orthographicSize;
            instance.transform.localScale = new Vector3(scale, scale, scale) ;
            
            instance.transform.position = worldToScreen + new Vector3(0f, xOffset * scale, 0f);
        }
    }
}
