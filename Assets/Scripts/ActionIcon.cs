using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActionIcon : MonoBehaviour {
    public Image image;
    public Sprite sprite;
    
    void Start() {
        if (image != null) {
            image.sprite = sprite;
        }

        StartCoroutine(Destroy());
    }

    IEnumerator Destroy() {
        yield return new WaitForSeconds(5f);

        Destroy(this.gameObject);
    }
}
