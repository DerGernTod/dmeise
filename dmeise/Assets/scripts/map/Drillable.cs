using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drillable : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRendererUndrilled;
    private Texture2D maskTexture;
    private Action updateAction = () => { };
    private Controller controller;
    private Vector3 controllerPrevPos;
    private Color transparentColor = new Color(0, 0, 0, 0);
    private float overrideSize;
    void Start()
    {
        controller = FindObjectOfType<Controller>();
        maskTexture = Instantiate(spriteRendererUndrilled.sprite.texture);
        Sprite sprite = spriteRendererUndrilled.sprite;
        Sprite spriteClone = Sprite.Create(maskTexture, sprite.rect, Vector2.one * .5f, sprite.pixelsPerUnit);
        spriteClone.name = sprite.name + " (Clone)";
        spriteRendererUndrilled.sprite = spriteClone;
        overrideSize = maskTexture.width * .1f;
        // spriteRendererUndrilled.material.mainTexture = maskTexture;
    }

    void Update()
    {
        updateAction();
    }

    private void updateTexture() {
        Vector3 controllerPos = controller.transform.position;
        Vector3 curPos = transform.position;
        Vector2 controllerDelta = controllerPos - controllerPrevPos;
        if (controllerDelta.sqrMagnitude > float.Epsilon) {
            Vector2 positionDelta = (controllerPos - curPos + Vector3.one * .5f) * maskTexture.width;
            for (int x = (int)positionDelta.x; x < Mathf.Min(positionDelta.x + overrideSize, maskTexture.width); x++) {
                for (int y = (int)positionDelta.y; y < Mathf.Min(positionDelta.y + overrideSize, maskTexture.height); y++) {
                    maskTexture.SetPixel(x, y, transparentColor);
                }
            }
            maskTexture.Apply();
        }
    }

    private void OnMouseEnter() {
        controllerPrevPos = controller.transform.position;
        updateAction = updateTexture;
    }

    private void OnMouseExit() {
        updateAction = () => { };
    }
}
