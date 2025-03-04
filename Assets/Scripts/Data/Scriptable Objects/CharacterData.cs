using Attributes;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Objects/Character Data", order = 1)]
public class CharacterData : ScriptableObject
{
    #region Variables & Properties

    [Foldout("Variables & Properties/Indentification Data")]
    [SerializeField] new string name;
    public string _name
    {
        get => name;
    }
    [Foldout("Variables & Properties/Indentification Data")]
    [ReadOnly][SerializeField] int identificationNumber;
    public int _identificationNumber
    {
        get => identificationNumber;
        #if UNITY_EDITOR
        set => identificationNumber = value;
        #endif
    }
    [Foldout("Variables & Properties/Indentification Data")]
    [SerializeField] Sprite sprite;
    public Sprite _sprite
    {
        get => sprite;
    }
    [Foldout("Variables & Properties/Storable Data")]
    [SerializeField] bool isUnlocked;
    public bool _isUnlocked
    {
        get => isUnlocked;
        set => isUnlocked = value;
    }
    [Foldout("Variables & Properties/Storable Data")]
    [LevelSlider][SerializeField] Vector2Int level;
    public int _level
    {
        get => level.x;
        set
        {
            if (value > level.x)
            {
                int lastLevel = level.x;

                level.x = value;

                int levelDifference = level.x - lastLevel;

                for (int i = 0, statsDataCount = statsData._count; i < statsDataCount; i++)
                {
                    StatData statData = statsData[i];

                    statData._value += statData._valueIncreasePerLevel * levelDifference;
                }
            }
        }
    }
    public int _maxLevel
    {
        get => level.y;
    }
    [Foldout("Variables & Properties/Storable Data")]
    [SerializeField] int fragments;
    public int _fragments
    {
        get => fragments;
        set
        {
            int nextLevel = _level + 1;

            int maxLevel = _maxLevel;

            int fragmentsPerLevel = 384;

            while (nextLevel < maxLevel)
            {
                fragmentsPerLevel /= 2;

                maxLevel--;
            }

            if (value > fragmentsPerLevel)
                value = fragmentsPerLevel;

            if (value > fragments)
            {
                fragments = value;

                if (fragments == fragmentsPerLevel)
                {
                    _level = nextLevel;

                    fragments = 0;
                }
            }
        }
    }
    [Foldout("Variables & Properties/Storable Data")]
    [SerializeField] DeckData deckData;
    public DeckData _deckData
    {
        get => deckData;
    }
    [Foldout("Variables & Properties/Game Data")]
    [CardPicker][SerializeField] List<int> initialDeckCardsIndexes;
    public List<int> _initialDeckCardsIndexes
    {
        get => initialDeckCardsIndexes;
    }
    [Foldout("Variables & Properties/Game Data")]
    [SerializeField] protected List<string> tagsData;
    public List<string> _tagsData
    {
        get => tagsData;
    }
    [Foldout("Variables & Properties/Game Data")]
    [SerializeField] StatsData statsData;
    public StatsData _statsData
    {
        get => statsData;
    }
    [Foldout("Variables & Properties/Game Data")]
    [SerializeField] StatesData statesData;
    public StatesData _statesData
    {
        get => statesData;
    }
    [Foldout("Variables & Properties/Game Data")]
    [SerializeField] UIStyleData UIStyleData;
    public UIStyleData _UIStyleData
    {
        get => UIStyleData;
    }

    #endregion

    #region Components

    [Foldout("Components/External")]
    [SerializeField] Ability ability;
    public Ability _ability
    {
        get => ability;
    }

    #endregion


    #if UNITY_EDITOR

    void OnValidate()
    {
        EditorApplication.playModeStateChanged -= PlayModeStateChanged;

        EditorApplication.playModeStateChanged += PlayModeStateChanged;

        List<Vector2Int> cardsIndexes = deckData._cardsIndexes;

		if (cardsIndexes.Count < deckData._size)
        {
            for (int i = cardsIndexes.Count, deckDataSize = deckData._size; i < deckDataSize; i++)
				cardsIndexes.Add(-Vector2Int.one);
        }
        else if (cardsIndexes.Count > deckData._size)
        {
            for (int i = 0, additionalCardsIndexes = cardsIndexes.Count - deckData._size; i < additionalCardsIndexes; i++)
                deckData._cardsIndexes.RemoveAt(deckData._cardsIndexes.Count - 1);
        }

		if (initialDeckCardsIndexes.Count < deckData._size)
		{
			for (int i = initialDeckCardsIndexes.Count, deckDataSize = deckData._size; i < deckDataSize; i++)
				initialDeckCardsIndexes.Add(-1);
		}
		else if (initialDeckCardsIndexes.Count > deckData._size)
		{
			for (int i = 0, additionalInitialDeckCardsIdentificationNumbers = initialDeckCardsIndexes.Count - deckData._size; i < additionalInitialDeckCardsIdentificationNumbers; i++)
				initialDeckCardsIndexes.RemoveAt(deckData._cardsIndexes.Count - 1);
		}
	}

    void PlayModeStateChanged(PlayModeStateChange playModeState)
    {
        if (playModeState == PlayModeStateChange.ExitingPlayMode && isUnlocked)
            isUnlocked = false;
    }

    #endif
}
