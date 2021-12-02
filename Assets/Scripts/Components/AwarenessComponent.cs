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
        RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2) transform.position, new Vector2(range, range), 0f, Vector2.right);

        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.CompareTag(targetTag)) {
                Transform found = hit.transform;
                if (found != null) {
                    Debug.Log("Target found: " + found, transform.root);
                    return found;
                }
            }
        }

        return null;
    }
}
