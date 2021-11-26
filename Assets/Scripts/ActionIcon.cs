using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIcon : MonoBehaviour {
    public Sprite sprite;
    
    void Start() {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy() {
        yield return new WaitForSeconds(5f);

        Destroy(this.gameObject);
    }
}
