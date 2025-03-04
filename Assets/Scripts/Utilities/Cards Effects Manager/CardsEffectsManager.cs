using Attributes;
using System;
using System.Collections;
using UnityEngine;
using Utilities;

public class CardsEffectsManager : MonoBehaviourSingleton
{
    #region Variables & Properties

    [Foldout("Variables & Properties (MergerCamera)")]
    [SerializeField] RenderTexture renderTexture;

    [Foldout("Variables & Properties (MergerCamera)/Effects/Dissolve")]
    [SerializeField] float dissolveTime;

    #endregion

    #region Components

    [Foldout("Components (MergerCamera)/External")]
    [SerializeField] new Transform transform;
    [Foldout("Components (MergerCamera)/External")]
    [SerializeField] SpriteRenderer spriteRenderer;

    #endregion

    public void DissolveCard(Transform card, Action onDissolved)
    {
        StartCoroutine(DissolveCardCoroutine(card, onDissolved));
    }

    IEnumerator DissolveCardCoroutine(Transform card, Action onDissolved)
    {
        transform.position = new Vector3(card.position.x, card.position.y, transform.position.z);

        yield return null;

        Texture2D texture2D = new Texture2D(720, 1080, TextureFormat.RGBA64, false);

        RenderTexture.active = renderTexture;

        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

        texture2D.Apply();

        Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));

        sprite.name = card.gameObject.name; 

        spriteRenderer.sprite = sprite;

        card.gameObject.SetActive(false);

        spriteRenderer.gameObject.SetActive(true);

        float time = Time.time;

        float initialAlphaClipThreshold = 0;

        float finalAlphaClipThreshold = 1;

        while (Time.time <= time + dissolveTime)
        {
            spriteRenderer.material.SetFloat("_AlphaClipThreshold", initialAlphaClipThreshold + ((finalAlphaClipThreshold - initialAlphaClipThreshold) * ((Time.time - time) / dissolveTime)));

            yield return null;
        }

        spriteRenderer.material.SetFloat("_AlphaClipThreshold", finalAlphaClipThreshold);

        spriteRenderer.gameObject.SetActive(false);

        onDissolved?.Invoke();
    }
}
