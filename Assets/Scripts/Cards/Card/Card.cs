using Attributes;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using System.IO;
#endif
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public abstract partial class Card : MonoBehaviour
{
    #region Variables & Properties

    public string _name
    {
        get => nameText.text;
    }
    [Foldout("Variables & Properties/Indentification Data")]
    [ReadOnly][SerializeField] protected int identificationNumber;
    public int _identificationNumber
    {
        get => identificationNumber;
        #if UNITY_EDITOR
        set => identificationNumber = value;
        #endif
    }
    [Foldout("Variables & Properties/Indentification Data")]
    [ReadOnly][SerializeField] protected int gameplayIdentificationNumber;
    public int _gameplayIdentificationNumber
    {
        get => gameplayIdentificationNumber;
        set => gameplayIdentificationNumber = value;
    }
    [Foldout("Variables & Properties/Indentification Data")]
    [SerializeField] protected Type type;
    public Type _type
    {
        get => type;
    }
    public static Vector2 spriteSize = new Vector2(7.2f, 10.8f);
    int sortingOrder = 0;
    public int _sortingOrder
    {
        get => sortingOrder;
        set
        {
            int lastSortingOrder = sortingOrder;

            sortingOrder = value;

            int sortingOrderDifference = sortingOrder - lastSortingOrder;

            int additionalSortingOrder = sortingOrderDifference * (maxSortingOrderOnCard + 1);

            spriteRenderer.sortingOrder += additionalSortingOrder;

            outlineSpriteRenderer.sortingOrder += additionalSortingOrder;

            iconSpriteRenderer.sortingOrder += additionalSortingOrder;

            nameTitleSpriteRenderer.sortingOrder += additionalSortingOrder;

            nameMeshRenderer.sortingOrder += additionalSortingOrder;

            illustrationSpriteRenderer.sortingOrder += additionalSortingOrder;

            descriptionSpriteRenderer.sortingOrder += additionalSortingOrder;

            descriptionMeshRenderer.sortingOrder += additionalSortingOrder;
        }
    }
    int maxSortingOrderOnCard = 2;
    [Foldout("Variables & Properties/Storable Data")]
    [SerializeField] protected CardData data;
    public CardData _data
    {
        get => data;
    }
    protected Func<CharacterData> playerCharacterData;
    protected Func<CharacterData> opponentCharacterData;
    [Foldout("Variables & Properties/Game Data")]
    [SerializeField] protected List<string> tagsData;
    [Foldout("Variables & Properties/Game Data")]
    [SerializeField] protected StatsData statsData;
    public StatsData _statsData
    {
        get => statsData;
    }
 
    Coroutine moveToLocalPositionCoroutine;
    float moveToLocalPositionTime = 0.1f;

    Coroutine rotateToLocalEulerAnglesCoroutine;
    float rotateToLocalEulerAnglesTime = 0.1f;

    Coroutine localScaleCoroutine;
    float localScaleCoroutineTime = 0.05f;

    #endregion

    #region Components

    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] protected BattlefieldManager battlefieldManager;
    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] protected BattlefieldUIManager battlefieldUIManager;
    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] protected CardsEffectsManager cardsEffectsManager;

    [Foldout("Components/Internal")]
    [ReadOnly][SerializeField] protected new Transform transform;
    public Transform _transform
    {
        get => transform;
    }
    [Foldout("Components/Internal")]
    [ReadOnly][SerializeField] protected SpriteRenderer spriteRenderer;

    [Foldout("Components/External")]
    [SerializeField] protected SpriteRenderer outlineSpriteRenderer;
    [Foldout("Components/External")]
    [SerializeField] protected SpriteRenderer iconSpriteRenderer;
    [Foldout("Components/External")]
    [SerializeField] protected SpriteRenderer nameTitleSpriteRenderer;
    [Foldout("Components/External")]
    [SerializeField] protected TMP_Text nameText;
    [Foldout("Components/External")]
    [SerializeField] protected MeshRenderer nameMeshRenderer;
    [Foldout("Components/External")]
    [SerializeField] protected SpriteRenderer illustrationSpriteRenderer;
    [Foldout("Components/External")]
    [SerializeField] protected SpriteRenderer descriptionSpriteRenderer;
    [Foldout("Components/External")]
    [SerializeField] protected TMP_Text descriptionText;
    [Foldout("Components/External")]
    [SerializeField] protected MeshRenderer descriptionMeshRenderer;

    #endregion

    #if UNITY_EDITOR

    void OnValidate()
    {
        if (transform == null)
            transform = GetComponent<Transform>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        EditorApplication.delayCall += CardOnValidate;
    }

    void CardOnValidate()
    {
        EditorApplication.delayCall -= CardOnValidate;

        if (this == null) return;

        switch (type)
        {
            case Type.Curse:
                {
                    Color actionColor = new Color(100f / 255f, 45f / 255f, 120f / 255f, 1f);

                    SetColor(actionColor);

                    SetIcon(33);

                    break;
                }
            case Type.Damage:
                {
                    Color damageColor = new Color(210f / 255f, 55f / 255f, 55f / 255f, 1f);

                    SetColor(damageColor);

                    SetIcon(34);

                    break;
                }
            case Type.Equip:
                {
                    Color equipColor = new Color(85f / 255f, 230f / 255f, 130f / 255f, 1f);

                    SetColor(equipColor);

                    SetIcon(76);

                    break;
                }
            case Type.Event:
                {
                    Color actionColor = new Color(200f / 255f, 185f / 255f, 110f / 255f, 1f);

                    SetColor(actionColor);

                    SetIcon(133);

                    break;
                }
            case Type.Spell:
                {
                    Color spellColor = new Color(0f / 255f, 190f / 255f, 250f / 255f, 1f);

                    SetColor(spellColor);

                    SetIcon(72);

                    break;
                }
            default:
                {
                    Color noneColor = Color.white;

                    SetColor(noneColor);

                    SetIcon(212);

                    break;
                }
        }
    }

    void SetColor(Color color)
    {
        if (spriteRenderer == null)
            return;

        if (spriteRenderer.color != color)
            spriteRenderer.color = color;

        if (nameTitleSpriteRenderer == null)
            return;

        Color nameTitleColor = new Color(color.r - 50f / 255f, color.g - 50f / 255f, color.b - 50f / 255f, 1f);

        if (nameTitleSpriteRenderer.color != nameTitleColor)
            nameTitleSpriteRenderer.color = nameTitleColor;
    }

    void SetIcon(int iconSpriteSheetIndex)
    {
        if (iconSpriteRenderer == null)
            return;

        string[] assetsGUID = AssetDatabase.FindAssets("RPG Icons");

        if (assetsGUID != null && assetsGUID.Length > 0)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetsGUID[0]);

            UnityEngine.Object[] spriteSheet = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            for (int i = 0, length = spriteSheet.Length; i < length; i++)
            {
                if (spriteSheet[i] is Sprite sprite && sprite.name == $"RPG Icons {iconSpriteSheetIndex}")
                {
                    if (iconSpriteRenderer.sprite != sprite)
                        iconSpriteRenderer.sprite = sprite;

                    break;
                }
            }
        }
    }

    [ContextMenu("Swap Script")]
    void SwapScript()
    {
        string scriptName = gameObject.name.Replace(" ", "");

        if (System.Type.GetType(scriptName) != null)
            return;

        string scriptPath = $"{Application.dataPath}/Scripts/Cards/{scriptName}.cs";

        string script = $"using System.IO;\r\n" +
                        $"using UnityEditor;\r\n" +
                        $"using UnityEngine;" +
                        $"\r\n\r\n" +
                        $"public class {scriptName} : Card\r\n" +
                        $"{{\r\n    " +
                        $"public override void Play()\r\n" +
                        $"{{\r\n    " +
                        $"base.Play();\r\n" +
                        $"}}\r\n\r\n    " +
                        $"[InitializeOnLoadMethod]\r\n    " +
                        $"static void SetScript()\r\n    " +
                        $"{{\r\n        " +
                        $"GameObject gameObject = GameObject.Find(\"{gameObject.name}\");\r\n\r\n        " +
                        $"gameObject.AddComponent<{scriptName}>();\r\n\r\n        " +
                        $"Card card = gameObject.GetComponent<Card>();\r\n\r\n        " +
                        $"if (card != null)\r\n            " +
                        $"DestroyImmediate(card);\r\n\r\n        " +
                        $"string script = $\"using UnityEngine;\" +\r\n                        " +
                        $"$\"\\r\\n\\r\\n\" +\r\n                        " +
                        $"$\"public class {scriptName} : Card\\r\\n\" +\r\n                        " +
                        $"$\"{{{{\\r\\n    \" +\r\n                        " +
                        $"$\"public override void Play()\\r\\n    \" +\r\n" +
                        $"$\"{{{{\\r\\n        \" +\r\n" +
                        $"$\"base.Play();\\r\\n    \" +\r\n" +
                        $"$\"}}}}\\r\\n\" +\r\n                        " +
                        $"$\"}}}}\";\r\n\r\n        " +
                        $"File.WriteAllText(\"{scriptPath}\", script);\r\n\r\n        " +
                        $"AssetDatabase.Refresh();\r\n    " +
                        $"}}\r\n" +
                        $"}}";

        File.WriteAllText(scriptPath, script);

        AssetDatabase.Refresh();
    }

    #endif

    void Awake()
    {
        data.onLevelUp += OnLevelUp;
    }

    public virtual void Initialize(Func<CharacterData> playerCharacterData, Func<CharacterData> opponentCharacterData)
    {
        battlefieldManager = this.GetSingleton<BattlefieldManager>();

        battlefieldUIManager = this.GetSingleton<BattlefieldUIManager>();

        cardsEffectsManager = this.GetSingleton<CardsEffectsManager>();

        this.playerCharacterData = playerCharacterData;

        this.opponentCharacterData = opponentCharacterData;

        string description = descriptionText.text;

        string fixedDescription = "";

        for (int i = 0, descriptionLength = description.Length; i < descriptionLength; i++)
        {
            if (description[i] == '{')
            {
                string statDescription = "";

                for (int j = i + 1; j < description.Length; j++)
                {
                    if (description[j] == '}')
                    {
                        string[] stat = statDescription.Split('-');

                        switch (stat[0])
                        {
                            case "S":
                                {
                                    statDescription = statsData[stat[1]]._value.ToString();

                                    break;
                                }
                            case "C":
                                {
                                    statDescription = this.playerCharacterData()._statsData[stat[1]]._value.ToString();

                                    break;
                                }
                        }

                        fixedDescription += statDescription;

                        i = j;

                        break;
                    }
                    else if (j == description.Length - 1)
                    {
                        #if UNITY_EDITOR
                        Debug.Log("Card/Initialize/Missing } on stat description");
                        #endif

                        i = j;

                        break;
                    }
                    else
                        statDescription += description[j];
                }
            }
            else
                fixedDescription += description[i];
        }

        descriptionText.text = fixedDescription;
    }

    public virtual void OnMouseDown()
    {
        if (!battlefieldManager._inTurn)
            return;

        battlefieldManager.RemoveCardFromHand(this);

        _sortingOrder = 100;

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.touches[0].position);

        transform.position = new Vector3(position.x, position.y, transform.position.z);

        transform.localEulerAngles = Vector3.zero;

        LocalScale(new Vector3(0.4f, 0.4f, 1f));
    }

    public virtual void OnMouseDrag()
    {
        if (!battlefieldManager._inTurn)
            return;

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.touches[0].position);

        transform.position = new Vector3(position.x, position.y, transform.position.z);

        outlineSpriteRenderer.gameObject.SetActive(transform.position.y > battlefieldManager._playerHandMaxY);
    }

    public virtual void OnMouseUp()
    {
        if (!battlefieldManager._inTurn)
            return;

        if (transform.position.y > battlefieldManager._playerHandMaxY)
        {
            float actionsPerTurn = playerCharacterData()._statsData["Actions Per Turn"]._value;

            float deltaActionsPerTurn = actionsPerTurn - 1;

            if (deltaActionsPerTurn < 0)
            {
                if(outlineSpriteRenderer.gameObject.activeSelf)
                    outlineSpriteRenderer.gameObject.SetActive(false);

                battlefieldManager.AddCardToHand(this);

                return;
            }
            else
                playerCharacterData()._statsData["Actions Per Turn"]._value -= 1;

            if (type == Type.Spell)
            {
                float mana = playerCharacterData()._statsData["Mana"]._value;

                float manaCost = statsData["Mana Cost"]._value;

                float deltaMana = mana - manaCost;

                if (deltaMana < 0)
                {
                    if (outlineSpriteRenderer.gameObject.activeSelf)
                        outlineSpriteRenderer.gameObject.SetActive(false);

                    battlefieldManager.AddCardToHand(this);

                    return;
                }
                else
                    playerCharacterData()._statsData["Mana"]._value -= manaCost;
            }

            outlineSpriteRenderer.gameObject.SetActive(false);

            Play();

            battlefieldUIManager.UpdatePlayerStat("Actions Per Turn");

            if (type == Type.Spell)
                battlefieldUIManager.UpdatePlayerStat("Mana");
        }
        else
            battlefieldManager.AddCardToHand(this);
    }

    void OnDisable()
    {
        data.onLevelUp -= OnLevelUp;
    }

    void OnLevelUp(int currentLevel, int lastLevel)
    {
        int levelDifference = currentLevel - lastLevel;

        for (int i = 0, statsDataCount = statsData._count; i < statsDataCount; i++)
        {
            StatData statData = statsData[i];

            statData._value += statData._valueIncreasePerLevel * levelDifference;
        }
    }

    public void MoveToLocalPosition(Vector3 finalLocalPosition)
    {
        if (moveToLocalPositionCoroutine != null)
            StopCoroutine(moveToLocalPositionCoroutine);

        moveToLocalPositionCoroutine = StartCoroutine(MoveToLocalPositionCoroutine(finalLocalPosition));
    }

    IEnumerator MoveToLocalPositionCoroutine(Vector3 finalLocalPosition)
    {
        Vector3 initialLocalPosition = transform.localPosition;

        float time = Time.time;

        while (Time.time < time + moveToLocalPositionTime)
        {
            transform.localPosition = initialLocalPosition + ((finalLocalPosition - initialLocalPosition) * ((Time.time - time) / moveToLocalPositionTime));

            yield return null;
        }

        transform.localPosition = finalLocalPosition;
    }

    public void RotateToLocalEulerAngles(Vector3 finalLocalEulerAngles)
    {
        if (rotateToLocalEulerAnglesCoroutine != null)
            StopCoroutine(rotateToLocalEulerAnglesCoroutine);

        rotateToLocalEulerAnglesCoroutine = StartCoroutine(RotateToLocalEulerAnglesCoroutine(finalLocalEulerAngles));
    }

    IEnumerator RotateToLocalEulerAnglesCoroutine(Vector3 finalLocalEulerAngles)
    {
        Vector3 initialLocalEulerAngles = transform.localEulerAngles;

        if (Mathf.Abs(finalLocalEulerAngles.z - initialLocalEulerAngles.z) > Mathf.Abs(finalLocalEulerAngles.z - (initialLocalEulerAngles.z - 360f)))
            initialLocalEulerAngles.z = initialLocalEulerAngles.z - 360f;

        float time = Time.time;

        while (Time.time < time + rotateToLocalEulerAnglesTime)
        {
            transform.localEulerAngles = initialLocalEulerAngles + ((finalLocalEulerAngles - initialLocalEulerAngles) * ((Time.time - time) / rotateToLocalEulerAnglesTime));

            yield return null;
        }

        transform.localEulerAngles = finalLocalEulerAngles;
    }

    public void LocalScale(Vector3 finalLocalScale)
    {
        if (localScaleCoroutine != null)
            StopCoroutine(localScaleCoroutine);

        localScaleCoroutine = StartCoroutine(LocalScaleCoroutine(finalLocalScale));
    }

    IEnumerator LocalScaleCoroutine(Vector3 finalLocalScale)
    {
        Vector3 initialLocalScale = transform.localScale;

        float time = Time.time;

        while (Time.time < time + localScaleCoroutineTime)
        {
            transform.localScale = initialLocalScale + ((finalLocalScale - initialLocalScale) * ((Time.time - time) / localScaleCoroutineTime));

            yield return null;
        }

        transform.localScale = finalLocalScale;
    }

    public virtual void Play()
    {
        if (battlefieldManager._inTurn)
            battlefieldManager.PlayCardOnOpponent(gameplayIdentificationNumber);

        cardsEffectsManager.DissolveCard(transform, OnDissolved);

        battlefieldUIManager.ShowRecord(illustrationSpriteRenderer.sprite);
    }

    protected virtual void OnDissolved()
    {
        battlefieldManager._cardsPlayedInTurn++;

        if (type == Type.Damage)
            battlefieldManager._damageCardsPlayedInTurn++;

        battlefieldManager._lastCardPlayedInTurn = this;

        battlefieldManager.AddCardToGraveyard(playerCharacterData(), gameObject);
    }
}

