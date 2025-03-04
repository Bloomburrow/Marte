using Attributes;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using System.Collections;
using System.Linq;
using TMPro;

public class BattlefieldManager : MonoBehaviourSingleton
{
    #region Variables & Properties

    [Foldout("Variables & Properties/Player")]
    [ReadOnly][SerializeField] CharacterData playerCharacterData;
    [Foldout("Variables & Properties/Player")]
    [ReadOnly][SerializeField] List<GameObject> playerDeck;
    [Foldout("Variables & Properties/Player")]
    [SerializeField] Vector2 playerHandHalfSize;
    public float _playerHandMaxY => playerHandTransform.position.y + (playerHandHalfSize.y * 2f * playerHandTransform.localScale.y);
    List<GameObject> playerGraveyard;
    [Foldout("Variables & Properties/Opponent")]
    [ReadOnly][SerializeField] CharacterData opponentCharacterData;
    public  CharacterData _opponentCharacterData => opponentCharacterData;
    [Foldout("Variables & Properties/Opponent")]
    [ReadOnly][SerializeField] List<GameObject> opponentDeck;
    List<GameObject> opponentGraveyard;

    WaitForSeconds waitToSpawnANewCardForSeconds = new WaitForSeconds(0.1f);

    bool inTurn;
    public bool _inTurn
    {
        get => inTurn;
        set
        {
            inTurn = value;

            cardsPlayedInTurn = 0;

            damageCardsPlayedInTurn = 0;

            lastCardPlayedInTurn = null;
        }
    }

    int cardsPlayedInTurn;
    public int _cardsPlayedInTurn
    {
        get => cardsPlayedInTurn;
        set => cardsPlayedInTurn = value;
    }
    int damageCardsPlayedInTurn;
    public int _damageCardsPlayedInTurn
    {
        get => damageCardsPlayedInTurn;
        set => damageCardsPlayedInTurn = value;
    }
    Card lastCardPlayedInTurn;
    public Card _lastCardPlayedInTurn
    {
        get => lastCardPlayedInTurn;
        set => lastCardPlayedInTurn = value;
    }

    #endregion

    #region Events

    public const byte setCharacterOnOpponent = 1;
    public const byte confirmThatTheCharacterWasSettedOnOpponent = 2;
    public const byte setTheFirstTurnOnAll = 3;
    public const byte playCardOnOpponent = 4;

    Dictionary<byte, Action<object[]>> events;

    #endregion

    #region Components

    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] GameDataManager gameDataManager;
    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] BattlefieldUIManager battlefieldUIManager;

    [Foldout("Components/Player")]
    [SerializeField] Transform playerDeckTransform;
    [Foldout("Components/Player")]
    [SerializeField] Transform playerHandTransform;
    [Foldout("Components/Player")]
    [SerializeField] TMP_Text playerParticlesText;
    [Foldout("Components/Player")]
    [SerializeField] Animation playerParticlesTextAnimation;
    [Foldout("Components/Player")]
    [SerializeField] ParticleSystem playerHealParticleSystem;
    [Foldout("Components/Player")]
    [SerializeField] ParticleSystem playerHitParticleSystem;
    [Foldout("Components/Opponent")]
    [SerializeField] Transform opponentDeckTransform;
    [Foldout("Components/Opponent")]
    [SerializeField] TMP_Text opponentParticlesText;
    [Foldout("Components/Opponent")]
    [SerializeField] Animation opponentParticlesTextAnimation;
    [Foldout("Components/Opponent")]
    [SerializeField] ParticleSystem opponentHealParticleSystem;
    [Foldout("Components/Opponent")]
    [SerializeField] ParticleSystem opponentHitParticleSystem;

    #endregion

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    protected override void Awake()
    {
        base.Awake();

        playerGraveyard = new List<GameObject>();

        opponentGraveyard = new List<GameObject>();
    }

    void Start()
    {
        events = new Dictionary<byte, Action<object[]>>
        {
            { setCharacterOnOpponent, SetOpponentCharacter },
            { confirmThatTheCharacterWasSettedOnOpponent, ConfirmThatTheCharacterWasSetted },
            { setTheFirstTurnOnAll, SetTheFirstTurn },
            { playCardOnOpponent, PlayOpponentCard }
        };

        gameDataManager = this.GetSingleton<GameDataManager>();

        battlefieldUIManager = this.GetSingleton<BattlefieldUIManager>();

        CharacterData playerCharacterData = gameDataManager._charactersData[gameDataManager._selectedCharacterIndex];

        this.playerCharacterData = Instantiate(playerCharacterData);

        this.playerCharacterData._statsData["Lives"].onValueChange += OnPlayerLivesChange;

        DeckData playerCharacterDeckData = this.playerCharacterData._deckData;

        for (int i = 0, deckSize = playerCharacterDeckData._size; i < deckSize; i++)
        {
            Vector2Int cardIndexes = playerCharacterDeckData._cardsIndexes[i];

            CardData cardData = gameDataManager._cardsData[cardIndexes.x][cardIndexes.y];

            Card cardCopy = Instantiate(gameDataManager._cardsPrefabs[cardIndexes.x]);

            cardCopy.name = cardCopy._name;

            cardCopy._transform.SetParent(playerDeckTransform);

            cardCopy._transform.localPosition = Vector3.zero;

            cardCopy._transform.localScale = Vector3.one;

            cardCopy._gameplayIdentificationNumber = i;

            cardCopy._data._isHolographic = cardData._isHolographic;

            cardCopy._data._level = cardData._level;

            cardCopy.Initialize(() =>
            {
                return this.playerCharacterData;
            }, () =>
            {
                return opponentCharacterData;
            });

            cardCopy.gameObject.SetActive(false);

            playerDeck.Add(cardCopy.gameObject);
        }

        this.playerCharacterData._statsData.Insert(2, new StatData(false, "Max Lives", this.playerCharacterData._statsData["Lives"]._value, false, this.playerCharacterData._statsData["Lives"]._valueIncreasePerLevel));

        this.playerCharacterData._statsData.Insert(6, new StatData(false, "Max Actions Per Turn", this.playerCharacterData._statsData["Actions Per Turn"]._value, false, this.playerCharacterData._statsData["Actions Per Turn"]._valueIncreasePerLevel));

        this.playerCharacterData._statsData.Add(new StatData(false, "Max Mana", this.playerCharacterData._statsData["Mana"]._value, false, this.playerCharacterData._statsData["Actions Per Turn"]._valueIncreasePerLevel));

        battlefieldUIManager.SetPlayerCharacter(this.playerCharacterData);

        UpdateUIStat(this.playerCharacterData, "Armor");

        if (this.playerCharacterData._ability._moment == Moment.AtTheBeginningOfTheGame)
            this.playerCharacterData._ability.Execute(this.playerCharacterData);

        SetCharacterOnOpponent(this.playerCharacterData);

        StartCoroutine(DrawInitialHandCoroutine());
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void OnEvent(EventData evenData)
    {
        byte eventCode = evenData.Code;

        if (!events.ContainsKey(eventCode))
            return;

        object[] customData = null;

        if (evenData.CustomData != null)
            customData = (object[])evenData.CustomData;

        events[eventCode](customData);
    }

    void SetCharacterOnOpponent(CharacterData playerCharacterData)
    {
        string dataToSendJson = "{";

        dataToSendJson += $"\"characterIndex\":\"{gameDataManager._selectedCharacterIndex}\",";

        dataToSendJson += $"\"characterLevel\":{playerCharacterData._level},";

        dataToSendJson += $"\"characterDeck\":";

        dataToSendJson += $"[";

        for (int i = 0, deckSize = playerCharacterData._deckData._size; i < deckSize; i++)
        {
            Vector2Int cardIndexes = playerCharacterData._deckData._cardsIndexes[i];

            CardData cardData = gameDataManager._cardsData[cardIndexes.x][cardIndexes.y];

            string cardDataJson = "{";

            cardDataJson += $"\"cardIdentificationNumber\":{cardIndexes.x},";

            cardDataJson += $"\"cardIsHolographic\":{cardData._isHolographic},";

            cardDataJson += $"\"cardLevel\":{cardData._level}";

            cardDataJson += "}" + ((i < deckSize - 1) ? "," : "");

            dataToSendJson += cardDataJson;
        }

        dataToSendJson += $"]";

        dataToSendJson += "}";

        object[] dataToSend = new object[] { dataToSendJson };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; 

        PhotonNetwork.RaiseEvent(setCharacterOnOpponent, dataToSend, raiseEventOptions, SendOptions.SendReliable);
    }

    void SetOpponentCharacter(object[] customData)
    {
        #if UNITY_EDITOR
        Debug.Log($"BatllefieldManager/SetOpponentCharacter/Custom data: {(string)customData[0]}");
        #endif

        Dictionary<string,string> customDataDictionary = Json.ToDictionary((string)customData[0]);

        CharacterData opponentCharacterData = gameDataManager._charactersData[Int32.Parse(customDataDictionary["characterIndex"])];

        this.opponentCharacterData = Instantiate(opponentCharacterData);

        this.opponentCharacterData._level = int.Parse(customDataDictionary["characterLevel"]);

        this.opponentCharacterData._statsData["Lives"].onValueChange += OnOpponentLivesChange;

        List<string> characterDeck = Json.ToList(customDataDictionary["characterDeck"]);

        this.opponentCharacterData._deckData._cardsIndexes.Clear();

        for (int i = 0, characterDeckCount = characterDeck.Count; i < characterDeckCount; i++)
        {
            Dictionary<string, string> cardData = Json.ToDictionary(characterDeck[i]);

            Card cardCopy = Instantiate(gameDataManager._cardsPrefabs[Int32.Parse(cardData["cardIdentificationNumber"])]);

            cardCopy.name = cardCopy._name;

            cardCopy._transform.SetParent(opponentDeckTransform);

            cardCopy._transform.localPosition = Vector3.zero;

            cardCopy._transform.localScale = Vector3.one;

            cardCopy._gameplayIdentificationNumber = i;

            cardCopy._data._isHolographic = bool.Parse(cardData["cardIsHolographic"]);

            cardCopy._data._level = Int32.Parse(cardData["cardLevel"]);

            cardCopy.Initialize(() =>
            {
                return this.opponentCharacterData;
            }, () =>
            {
                return playerCharacterData;
            });

            cardCopy.gameObject.SetActive(false);

            opponentDeck.Add(cardCopy.gameObject);
        }

        this.opponentCharacterData._statsData.Insert(2, new StatData(false, "Max Lives", this.opponentCharacterData._statsData["Lives"]._value, false, this.opponentCharacterData._statsData["Lives"]._valueIncreasePerLevel));

        this.opponentCharacterData._statsData.Insert(6, new StatData(false, "Max Actions Per Turn", this.opponentCharacterData._statsData["Actions Per Turn"]._value, false, this.opponentCharacterData._statsData["Actions Per Turn"]._valueIncreasePerLevel));

        this.opponentCharacterData._statsData.Add(new StatData(false, "Max Mana", this.opponentCharacterData._statsData["Mana"]._value, false, this.opponentCharacterData._statsData["Actions Per Turn"]._valueIncreasePerLevel));

        battlefieldUIManager.SetOpponentCharacter(this.opponentCharacterData);

        UpdateUIStat(this.opponentCharacterData, "Armor");

        if (this.opponentCharacterData._ability._moment == Moment.AtTheBeginningOfTheGame)
            this.opponentCharacterData._ability.Execute(this.opponentCharacterData);

        ConfirmThatTheCharacterWasSettedOnOpponent();
    }

    IEnumerator DrawInitialHandCoroutine()
    {
        List<GameObject> playerDeck = new List<GameObject>();

        while (this.playerDeck.Count > 0)
        {
            GameObject playerCard = this.playerDeck[UnityEngine.Random.Range(0, this.playerDeck.Count)];

            this.playerDeck.Remove(playerCard);

            playerDeck.Add(playerCard); 
        }

        this.playerDeck = playerDeck;

        for (int i = 0, playerDeckCount = this.playerDeck.Count; i < playerDeckCount; i++)
            this.playerDeck[i].transform.SetSiblingIndex(i);

        for (int i = 0, initalHand = (int)playerCharacterData._statsData["Hand"]._value; i < initalHand; i++)
        {
            DrawCard();

            yield return waitToSpawnANewCardForSeconds;
        }
    }

    void ConfirmThatTheCharacterWasSettedOnOpponent()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

        PhotonNetwork.RaiseEvent(confirmThatTheCharacterWasSettedOnOpponent, null, raiseEventOptions, SendOptions.SendReliable);
    }    

    void ConfirmThatTheCharacterWasSetted(object[] customData)
    {
        SetTheFirstTurnOnAll();
    }

    void SetTheFirstTurnOnAll()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        Player randomPlayer = PhotonNetwork.CurrentRoom.Players.ElementAt(UnityEngine.Random.Range(0, PhotonNetwork.CurrentRoom.Players.Count)).Value;

        object[] dataToSend = new object[] { randomPlayer.NickName };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All};

        PhotonNetwork.RaiseEvent(setTheFirstTurnOnAll, dataToSend, raiseEventOptions, SendOptions.SendReliable);
    }

    void SetTheFirstTurn(object[] customData)
    {
        #if UNITY_EDITOR
        Debug.Log($"BatllefieldManager/SetTheFirstTurn/Custom data: {(string)customData[0]}");
        #endif

        inTurn = (string)customData[0] == PhotonNetwork.NickName;

        if (inTurn)
        {
            battlefieldUIManager.SetPlayerTurn(true);

            AddStat(opponentCharacterData, false, "Armor", 3f);
        }
        else
        {
            battlefieldUIManager.SetOpponentTurn(true);

            AddStat(playerCharacterData, false, "Armor", 3f);
        }
    }

    public void PlayCardOnOpponent(int gameplayIdentificationNumber)
    {
        object[] dataToSend = new object[] { gameplayIdentificationNumber };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

        PhotonNetwork.RaiseEvent(playCardOnOpponent, dataToSend, raiseEventOptions, SendOptions.SendReliable);
    }

    public void PlayOpponentCard(object[] customData)
    {
        #if UNITY_EDITOR
        Debug.Log($"BatllefieldManager/PlayOpponentCard/Custom data: {(int)customData[0]}");
        #endif

        for (int i = 0, opponentDeckCount = opponentDeck.Count; i < opponentDeckCount; i++)
        {
            Card opponentCard = opponentDeck[i].GetComponent<Card>();

            if (opponentCard._gameplayIdentificationNumber == (int)customData[0])
            {
                opponentCharacterData._statsData["Actions Per Turn"]._value -= 1;

                if (opponentCard._type == Card.Type.Spell)
                    opponentCharacterData._statsData["Mana"]._value -= opponentCard._statsData["Mana Cost"]._value;

                StartCoroutine(PlayOpponentCardCoroutine(opponentCard));

                battlefieldUIManager.UpdateOpponentStat("Actions Per Turn");

                if (opponentCard._type == Card.Type.Spell)
                    battlefieldUIManager.UpdateOpponentStat("Mana");

                return;
            }
        }
    }

    IEnumerator PlayOpponentCardCoroutine(Card opponentCard)
    {
        opponentCard._transform.SetParent(null);

        opponentCard._transform.position = Vector3.zero;

        opponentCard._transform.localScale = new Vector3(0.4f, 0.4f, 1f);

        opponentCard._sortingOrder = 100;

        opponentCard.gameObject.SetActive(true);

        yield return null;

        opponentCard.Play();
    }

    public void AddStat(CharacterData characterData, bool isMoment, string name, float value, bool increasePerLevel = false, float valueIncreasePerLevel = 0f)
    {
        if (characterData._statsData.Contains(name))
            characterData._statsData[name]._value += value;
        else
            characterData._statsData.Add(new StatData(isMoment, name, value, increasePerLevel, valueIncreasePerLevel));

        UpdateUIStat(characterData, name);
    }

    public void UpdateUIStat(CharacterData characterData, string name)
    {
        if (characterData == playerCharacterData)
            battlefieldUIManager.UpdatePlayerStat(name);
        else
            battlefieldUIManager.UpdateOpponentStat(name);
    }

    public void AddState(CharacterData characterData, string name, int turns)
    {
        if (characterData._statesData.Contains(name))
            characterData._statesData[name]._leftTurns += turns;
        else
            characterData._statesData.Add(new StateData(name, turns));

        UpdateUIState(characterData, name);
    }

    public void UpdateUIState(CharacterData characterData, string name)
    {
        if (characterData == playerCharacterData)
            battlefieldUIManager.UpdatePlayerState(name);
        else
            battlefieldUIManager.UpdateOpponentState(name);
    }

    void OnPlayerLivesChange(float deltaLives)
    {
        if (deltaLives > 0)
            playerParticlesText.text = $"+{deltaLives}";
        else
            playerParticlesText.text = $"{deltaLives}";

        playerParticlesTextAnimation.Play();

        if (deltaLives > 0)
            playerHealParticleSystem.Play();
        else
            playerHitParticleSystem.Play();
    }

    void OnOpponentLivesChange(float deltaLives)
    {
        if (deltaLives > 0)
            opponentParticlesText.text = $"+{deltaLives}";
        else
            opponentParticlesText.text = $"{deltaLives}";

        opponentParticlesTextAnimation.Play();

        if (deltaLives > 0)
            opponentHealParticleSystem.Play();
        else
            opponentHitParticleSystem.Play();
    }

    void DrawCard()
    {
        Card card = playerDeck[playerDeck.Count - 1].GetComponent<Card>();

        playerDeck.Remove(card.gameObject);

        battlefieldUIManager.UpdatePlayerDeck(-1);

        AddCardToHand(card);
    }

    public void AddCardToHand(Card card)
    {
        card._transform.SetParent(playerHandTransform);

        card.gameObject.SetActive(true);

        OrganizeHand();
    }

    public void RemoveCardFromHand(Card card)
    {
        card._transform.SetParent(null);

        OrganizeHand();
    }

    void OrganizeHand()
    {
        for (int i = 0, playerHandTransformChildCount = playerHandTransform.childCount; i < playerHandTransformChildCount; i++)
        {
            Card card = playerHandTransform.GetChild(i).GetComponent<Card>();

            card._sortingOrder = i;

            Vector2 cardSpriteSize = Card.spriteSize;

            Vector2 playerHandHalfSize = new Vector2(Math.Clamp((cardSpriteSize.x / 4f) + ((cardSpriteSize.x / 4f) * (playerHandTransformChildCount - 1)), cardSpriteSize.x / 4f, this.playerHandHalfSize.x), this.playerHandHalfSize.y);

            float x = (-playerHandHalfSize.x / 2f) * (playerHandTransformChildCount - 1);

            bool cardsExcess = x < -this.playerHandHalfSize.x;

            if (cardsExcess)
                x = -playerHandHalfSize.x;

            float distanceBetweenCards = 0f;

            if(cardsExcess)
                distanceBetweenCards = (playerHandHalfSize.x * 2f) / (playerHandTransformChildCount - 1);
            else
            {
                if (playerHandTransformChildCount > 1)
                    distanceBetweenCards = playerHandHalfSize.x;
            }

            x += (distanceBetweenCards * i);

            float angle = (180f / (playerHandHalfSize.x * 2f)) * (x + playerHandHalfSize.x);

            float y = MathF.Sin(angle * (Mathf.PI / 180f));

            card.MoveToLocalPosition(new Vector3(x, y, 0f));

            angle = ((-1f / 3f) * angle) + 30f;

            card.RotateToLocalEulerAngles(new Vector3(0f, 0f, angle));

            if (card._transform.localScale != Vector3.one)
                card.LocalScale(Vector3.one);
        }
    }

    public void AddCardToGraveyard(CharacterData characterData, GameObject gameObject)
    {
        gameObject.SetActive(false);

        if (characterData == playerCharacterData)
            playerGraveyard.Add(gameObject);
        else
            opponentGraveyard.Add(gameObject);
    }

    public void ImproveCards<T1>(CharacterData characterData, string newDescription)
    {
        List<GameObject> deck = (characterData == playerCharacterData) ? playerDeck : opponentDeck;

        for (int i = 0, deckCount = deck.Count; i < deckCount; i++)
        {
            Card card = deck[i].GetComponent<Card>();

            if (card.GetType() == typeof(T1))
            {
                IImprovable improvable = (IImprovable)card;

                improvable.Improve(characterData, newDescription);
            }
        }
    }

    public void EvolveCards<T1,T2>(CharacterData characterData, string newName, Sprite newIllustration, string newDescription)
    {
        List<GameObject> deck = (characterData == playerCharacterData) ? playerDeck : opponentDeck;

        for (int i = 0, deckCount = deck.Count; i < deckCount; i++)
        {
            Card card = deck[i].GetComponent<Card>();

            if (card.GetType() == typeof(T1))
            {
                IEvolvable evolvable = (IEvolvable)card;

                evolvable.Evolve<T2>(characterData, newName, newIllustration, newDescription);
            }
        }
    }
   
    #if UNITY_EDITOR

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(playerHandTransform.position, new Vector3((playerHandHalfSize.x * playerHandTransform.localScale.x) * 2f, (playerHandHalfSize.y * playerHandTransform.localScale.y) * 2f, 0f));
    }

    #endif
}
