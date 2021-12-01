using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public enum AwarenessType {
    NONE,
    VISION,
    SENSE,
    AGGRESSION
}

public class AwarenessComponent : MonoBehaviour {
    public AwarenessType type;

    public Transform FindInRange(String targetTag, float range) {
        RaycastHit2D hit = Physics2D.BoxCast((Vector2) transform.position, new Vector2(range, range), 0f, Vector2.right);

        if (hit.collider.CompareTag(targetTag)) {
            Transform found = hit.transform;
            if (found != null) {
                Debug.Log("Target found: " + found, found.gameObject);
                return found;
            }
        }

        return null;
    }
}
