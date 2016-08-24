//----------------------------------------------
//                 SpriteMask
//          Copyright © 2015 TrueSoft
//             support@truesoft.pl
//----------------------------------------------

using UnityEngine;

public class SpriteMaskExample05 : MonoBehaviour
{
    public Sprite spriteAnimal1;
    public Sprite spriteAnimal2;
    public Sprite spriteCloud;
    public Sprite spriteTree;
    public GameObject target;

    void Start ()
    {
        if (target == null) {
            target = gameObject;
        }

        SpriteMask mask = target.AddComponent <SpriteMask> ();
        mask.type = SpriteMask.Type.Rectangle;
        mask.size = new Vector2 (350f, 350f);

        attachSprite (spriteCloud, new Vector2 (110f, 160f)).sortingOrder = 1;
        attachSprite (spriteTree, new Vector2 (-120f, 75f)).sortingOrder = 2;
        attachSprite (spriteAnimal1, new Vector2 (100f, -40f)).sortingOrder = 3;
        attachSprite (spriteAnimal2, new Vector2 (-160f, -100f)).sortingOrder = 4;

        // Call this to get sprites masked
        mask.updateSprites ();
    }

    private SpriteRenderer attachSprite (Sprite sprite, Vector2 position)
    {
        GameObject go = new GameObject (sprite.name);
        go.transform.parent = target.transform;
        go.transform.localPosition = position;
        SpriteRenderer sr = go.AddComponent <SpriteRenderer> ();
        sr.sprite = sprite;
        return sr;
    }
}
