using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class PlayModeStateDetector
{
	static PlayModeStateDetector()
	{
		EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

		EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
	}

	private static void OnPlayModeStateChanged(PlayModeStateChange playModeState)
	{
		switch (playModeState)
		{
			case PlayModeStateChange.ExitingPlayMode:
			{
				CharacterData[] gameCharactersData = Array.ConvertAll(AssetDatabase.FindAssets("t:CharacterData"), (characterDataGUID) =>
				{
					string characterDataPath = AssetDatabase.GUIDToAssetPath(characterDataGUID);

					return AssetDatabase.LoadAssetAtPath<CharacterData>(characterDataPath);
				});

				for (int i = 0, gameCharactersDataLength = gameCharactersData.Length; i < gameCharactersDataLength; i++)
				{
					List<Vector2Int> cardsIndexes = gameCharactersData[i]._deckData._cardsIndexes;

					for (int j = 0, cardsIndexesCount = cardsIndexes.Count; j < cardsIndexesCount; j++)
					{
						if (cardsIndexes[j] != -Vector2Int.one)
							cardsIndexes[j] = -Vector2Int.one;
					}
				}

				break;
			}
		}
	}
}