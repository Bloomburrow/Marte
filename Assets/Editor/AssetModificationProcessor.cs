using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class AssetModificationProcessor : UnityEditor.AssetModificationProcessor
{
    static void OnWillCreateAsset(string assetPath)
    {
        if (assetPath.Contains(".meta"))
            return;

        EditorCoroutine.StartCoroutine(OnWillCreateAssetCoroutine(assetPath));
    }

    private static IEnumerator OnWillCreateAssetCoroutine(string assetPath)
    {
        Type type = AssetDatabase.GetMainAssetTypeAtPath(assetPath);

        while (type == null)
        {
            type = AssetDatabase.GetMainAssetTypeAtPath(assetPath);

            yield return null;
        }

        if (type == typeof(CharacterData))
        {
            CharacterData characterData = AssetDatabase.LoadAssetAtPath<CharacterData>(assetPath);

            characterData._identificationNumber = AssetDatabase.FindAssets("t:CharacterData").Length - 1;

            AssetDatabase.Refresh();

            yield return null;
        }

        if (type == typeof(GameObject))
        {
            GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            Card card = gameObject.GetComponent<Card>();

            if (card != null)
            {
                card._identificationNumber = GetCardsPrefabsCount() - 1;

                PrefabUtility.SavePrefabAsset(gameObject);

                AssetDatabase.Refresh();

                yield return null;
            }
        }
    }

    public static int GetCardsPrefabsCount()
    {
        int cardsPrefabsCount = 0;

        string[] assetPaths = AssetDatabase.GetAllAssetPaths();

        foreach (string assetPath in assetPaths)
        {
            if (assetPath.Contains(".prefab") && AssetDatabase.LoadAssetAtPath<GameObject>(assetPath).GetComponent<Card>() != null)
                cardsPrefabsCount++;

        }

        return cardsPrefabsCount;
    }
}
