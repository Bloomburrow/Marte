using Attributes;
using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    #region Variables & Properties

    [Foldout("Variables & Properties/Paths")]
    [ReadOnly][SerializeField] string gameDataPath;
    [Foldout("Variables & Properties/Paths")]
    [ReadOnly][SerializeField] string playerDataPath;
    [Foldout("Variables & Properties/Paths")]
	[ReadOnly][SerializeField] string charactersMetaDataPath;
	[Foldout("Variables & Properties/Paths")]
	[ReadOnly][SerializeField] string cardsDataPath;
	[Foldout("Variables & Properties/Characters")]
    [SerializeField] List<CharacterData> charactersData;
    public List<CharacterData> _charactersData
    {
        get => charactersData;
    }
    public bool _hasCharacter
    {
        get
        {
            foreach (CharacterData characterData in charactersData)
            {
                if(characterData._isUnlocked)
                    return true;
            }

            return false;
        }
    }
    [Foldout("Variables & Properties/Characters")]
    [SerializeField] int selectedCharacterIndex;
    public int _selectedCharacterIndex
    {
        get => selectedCharacterIndex;
        set => selectedCharacterIndex = value;
    }
    [Foldout("Variables & Properties/Cards")]
    [SerializeField] List<Card> cardsPrefabs;
    public List<Card> _cardsPrefabs
    {
        get => cardsPrefabs;
    }

    [Foldout("Variables & Properties/Cards")]
    [SerializeField] List<CardsData> cardsData;
    public List<CardsData> _cardsData
    {
        get => cardsData;
    }

    #endregion

    #if UNITY_EDITOR

    void OnValidate()
    {
        if (charactersData == null)
            return;

        CharacterData[] gameCharactersData = Array.ConvertAll(AssetDatabase.FindAssets("t:CharacterData"), (characterDataGUID) =>
        {
            string characterDataPath = AssetDatabase.GUIDToAssetPath(characterDataGUID);

            return AssetDatabase.LoadAssetAtPath<CharacterData>(characterDataPath);
        });

        int gameCharactersDataLength = gameCharactersData.Length;

        CharacterData[] orderedGameCharactersData = new CharacterData[gameCharactersData.Length];

        for (int i = 0; i < gameCharactersDataLength; i++)
            orderedGameCharactersData[gameCharactersData[i]._identificationNumber] = gameCharactersData[i];

        gameCharactersData = orderedGameCharactersData;

        if (charactersData.Count < gameCharactersDataLength)
        {
            for (int i = charactersData.Count; i < gameCharactersDataLength; i++)
                charactersData.Add(gameCharactersData[i]);
        }
        else if (charactersData.Count > gameCharactersDataLength)
        {
            for (int i = 0, additionalCharactersData = charactersData.Count - gameCharactersDataLength; i < additionalCharactersData; i++)
                charactersData.RemoveAt(charactersData.Count - 1);
        }

        if (cardsPrefabs == null)
            return;

        List<Card> gameCards = new List<Card>();

        string[] assetPaths = AssetDatabase.GetAllAssetPaths();

        foreach (string assetPath in assetPaths)
        {
            if (assetPath.Contains(".prefab"))
            {
                GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (asset != null)
                {
                    Card card = asset.GetComponent<Card>();

                    if (card != null)
                        gameCards.Add(card);
                }
            }
        }

        int gameCardsCount = gameCards.Count;

        Card[] orderedGameCards = new Card[gameCards.Count];

        for (int i = 0; i < gameCardsCount; i++)
            orderedGameCards[gameCards[i]._identificationNumber] = gameCards[i];

        gameCards = new List<Card>(orderedGameCards);

        if (cardsPrefabs.Count < gameCardsCount)
        {
            for (int i = cardsPrefabs.Count; i < gameCardsCount; i++)
                cardsPrefabs.Add(gameCards[i]);
        }
        else if (cardsPrefabs.Count > gameCardsCount)
        {
            int additionalCardsData = cardsPrefabs.Count - gameCardsCount;

            for (int i = 0; i < additionalCardsData; i++)
                cardsPrefabs.RemoveAt(cardsPrefabs.Count - 1);
        }

		if (cardsData.Count < gameCardsCount)
		{
			for (int i = cardsData.Count; i < gameCardsCount; i++)
				cardsData.Add(new CardsData());
		}
		else if (cardsData.Count > gameCardsCount)
		{
			int additionalCardsData = cardsData.Count - gameCardsCount;

			for (int i = 0; i < additionalCardsData; i++)
				cardsData.RemoveAt(cardsData.Count - 1);
		}
	}

    #endif

    void Awake()
    {
        gameDataPath = Path.Combine(Application.persistentDataPath, "Game Data");

        if (!Directory.Exists(gameDataPath))
            Directory.CreateDirectory(gameDataPath);

        playerDataPath = Path.Combine(gameDataPath, "Player Data.json");

        charactersMetaDataPath = Path.Combine(gameDataPath, "Characters Meta Data");

		if (!Directory.Exists(charactersMetaDataPath))
			Directory.CreateDirectory(charactersMetaDataPath);

		cardsDataPath = Path.Combine(gameDataPath, "Cards Data");

		if (!Directory.Exists(cardsDataPath))
			Directory.CreateDirectory(cardsDataPath);

		LoadGameData();
    }

    void LoadGameData()
    {
        if (File.Exists(playerDataPath))
        {
            string playerDataJson = File.ReadAllText(playerDataPath);

            Dictionary<string, string> playeraData = Json.ToDictionary(playerDataJson);

            selectedCharacterIndex = int.Parse(playeraData["selectedCharacterIndex"]);
        }

        if (Directory.Exists(charactersMetaDataPath))
        {
            string[] charactersMetaDataPaths = Directory.GetFiles(charactersMetaDataPath);

            for (int i = 0, charactersMetaDataPathsLength = charactersMetaDataPaths.Length; i < charactersMetaDataPathsLength; i++)
            {
				string characterMetaDataJson = File.ReadAllText(charactersMetaDataPaths[i]);

				Dictionary<string, string> characterMetaData = Json.ToDictionary(characterMetaDataJson);

				CharacterData characterData = charactersData[int.Parse(characterMetaData["characterIdentificationNumber"])];

                characterData._isUnlocked = true;

				characterData._level = int.Parse(characterMetaData["level"]);

				characterData._fragments = int.Parse(characterMetaData["fragments"]);

				List<string> deckData = Json.ToList(characterMetaData["deckData"]);

                for (int j = 0, deckDataCount = deckData.Count; j < deckDataCount; j++)
                {
					Dictionary<string, string> cardIndexesData = Json.ToDictionary(deckData[j]);

					characterData._deckData._cardsIndexes[j] = new Vector2Int(int.Parse(cardIndexesData["cardIdentificationNumber"]), int.Parse(cardIndexesData["cardIndex"]));
                }
			}
        }

        if (Directory.Exists(cardsDataPath))
        {
            string[] cardsDataPaths = Directory.GetFiles(cardsDataPath);

			for (int i = 0, cardsDataPathsLength = cardsDataPaths.Length; i < cardsDataPathsLength; i++)
			{
                string cardDataPath = cardsDataPaths[i];

				string cardDataJson = File.ReadAllText(cardsDataPaths[i]);

				Dictionary<string, string> cardData = Json.ToDictionary(cardDataJson);

                cardsData[int.Parse(cardData["cardIdentificationNumber"])].Add(new CardData(cardData["isHolographic"] == "true", int.Parse(cardData["level"]), int.Parse(cardData["maxLevel"]), int.Parse(cardData["fragments"])));
			}
		}
	}

    public void SavePlayerData()
    {
        string playerDataJson = "{";

        playerDataJson += $"\"selectedCharacterIndex\":{selectedCharacterIndex}";

        playerDataJson += "}";

        File.WriteAllText(playerDataPath, playerDataJson);
    }

    public void SaveCharactersMetaData()
    {
        for (int i = 0, charactersDataCount = charactersData.Count; i < charactersDataCount; i++) 
        {
			CharacterData characterData = charactersData[i];

            if (characterData._isUnlocked)
            {
                string characterMetaDatapath = Path.Combine(charactersMetaDataPath, $"{characterData._name}.json");

                string characterMetaDataJson = "{";

                characterMetaDataJson += $"\"characterIdentificationNumber\":{characterData._identificationNumber},";

                characterMetaDataJson += $"\"level\":{characterData._level},";

                characterMetaDataJson += $"\"fragments\":{characterData._fragments},";

                characterMetaDataJson += $"\"deckData\":";

                DeckData deckData = characterData._deckData;

                characterMetaDataJson += "[";

                for (int j = 0, deckCardsIndexesCount = deckData._cardsIndexes.Count; j < deckCardsIndexesCount; j++)
                {
                    Vector2Int cardIndexes = deckData._cardsIndexes[j];

					characterMetaDataJson += "{";

					characterMetaDataJson += $"\"cardIdentificationNumber\":{cardIndexes.x},";

					characterMetaDataJson += $"\"cardIndex\":{cardIndexes.y}";

					characterMetaDataJson += "}" + ((j < deckCardsIndexesCount - 1) ? "," : "");
				}

                characterMetaDataJson += "]";

                characterMetaDataJson += "}";

                File.WriteAllText(characterMetaDatapath, characterMetaDataJson);
            }
		}
    }

    public void SaveCardsData()
    {
		for (int i = 0, j = 0, cardsDataCount = cardsData.Count; i < cardsDataCount; i++)
		{
            foreach (CardData cardData in cardsData[i])
            {
				string cardDataPath = Path.Combine(cardsDataPath, $"{cardsPrefabs[i]._name.Replace("\r\n", "").Replace("\n", "")} {j}.json");

				string cardDataJson = "{";

				cardDataJson += $"\"cardIdentificationNumber\":{i},";

				cardDataJson += $"\"isHolographic\":{cardData._isHolographic.ToString().ToLower()},";

				cardDataJson += $"\"level\":{cardData._level},";

                cardDataJson += $"\"maxLevel\":{cardData._maxLevel},";

                cardDataJson += $"\"fragments\":{cardData._fragments}";

				cardDataJson += "}";

				File.WriteAllText(cardDataPath, cardDataJson);

                j++;
			}

            j = 0;
		}
	}

    #if UNITY_EDITOR

    [MenuItem("PikPok/Utilities/Delete All Saved Data")]
    static void DeleteAllSavedData()
    {
        CharacterData[] gameCharactersData = Array.ConvertAll(AssetDatabase.FindAssets("t:CharacterData"), (characterDataGUID) =>
        {
            string characterDataPath = AssetDatabase.GUIDToAssetPath(characterDataGUID);

            return AssetDatabase.LoadAssetAtPath<CharacterData>(characterDataPath);
        });

        foreach (CharacterData gameCharacterData in gameCharactersData)
            gameCharacterData._isUnlocked = false;
       
        string gameDataPath = Path.Combine(Application.persistentDataPath, "Game Data");

        if (Directory.Exists(gameDataPath))
        {
            Directory.Delete(gameDataPath, true);

            Debug.Log("GameDataManager/DeleteAllSavedData/All saved data deleted");
        }
        else
            Debug.Log("GameDataManager/DeleteAllSavedData/No saved data found");
    }

    #endif
}
