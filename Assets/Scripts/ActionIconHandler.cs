using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionIconHandler : MonoBehaviour
{
    public ActionIcon actionIcon;

    public IEnumerator Spawn(Sprite sprite) {
        ActionIcon instance = Instantiate(actionIcon, transform);
        instance.sprite = sprite;

        instance.GetComponent<Image>().sprite = sprite;
        
        // for (float t = 0f; t < 5f; t += Time.deltaTime) {
        //     Color c = actionIcon;
        //     c.a = c.a - 0.1f;
        //     actionIcon.color = c;
        //
        //     yield return new WaitForSeconds(0.1f);
        // }

        yield return new WaitForSeconds(1f);
    }
}
