using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Vector3 amount;

    Transform cam;

    Vector3 lastCamPosition;

    [SerializeField] bool infinityHorizontal;
    [SerializeField] bool infinityVertical;

    private float textureUnitySizeX;
    private float textureUnitySizeY;

    private void Awake()
    {
        cam = Camera.main.transform;
        lastCamPosition = cam.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;

        Texture2D texture = sprite.texture;

        textureUnitySizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitySizeY = texture.height / sprite.pixelsPerUnit;
    }


    private void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - lastCamPosition;

        transform.position += new Vector3(deltaMovement.x * amount.x, deltaMovement.y * amount.y);

        lastCamPosition = cam.position;

        if (infinityHorizontal)
        {
            if (Mathf.Abs(cam.position.x - transform.position.x) >= textureUnitySizeX)
            {
                float offsetPositionX = (cam.position.x - transform.position.x) % textureUnitySizeX;
                transform.position = new Vector3(cam.position.x + offsetPositionX, transform.position.y);
            }
        }

        if (infinityVertical)
        {
            if (Mathf.Abs(cam.transform.position.y - transform.position.y) >= textureUnitySizeY)
            {
                float offsetPositionY = (cam.position.y - transform.position.y) % textureUnitySizeY;
                transform.position = new Vector3(transform.position.x, cam.position.y + offsetPositionY);
            }
        }


    }



}
