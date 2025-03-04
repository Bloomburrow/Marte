using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public class CardCreatorEditorWindow : EditorWindow
    {
        #region Variables & Properties

        static Vector2 cardSize = new Vector2(334f, 500f);

        Card.Type cardType;
        Sprite cardFrameOutlineSprite;
        Sprite cardInnerFrameSprite;
        Sprite cardIllustrationMaskSprite;
        Vector3 cardIllustrationOffset;
        Sprite cardIllustrationSprite;   
        Sprite cardNameTextBoxSprite;
        string cardName = "";
        TMP_FontAsset cardNameFont;
        Card.Rarity cardRarity;
        Sprite cardRaritySprite;
        Sprite cardDescriptionTextBoxSprite;
        string cardDescription = "";
        TMP_FontAsset cardDescriptionFont;
        Sprite cardTypeIconSprite;
        bool cardHasSpeed = false;
        int cardSpeed = 1;

        Sprite zoomIcon;

        #endregion

        #region Events

        Action onDestroy;

        #endregion

        #region Components

        Camera sceneCamera;

        SpriteRenderer cardFrameOutlineSpriteRenderer;
        SpriteRenderer cardInnerFrameSpriteRenderer;
        SpriteMask cardIllustrationMaskSpriteRenderer;
        Transform cardIllustrationTransform;
        SpriteRenderer cardIllustrationSpriteRenderer;
        SpriteRenderer cardNameBoxSpriteRenderer;
        MeshRenderer cardNameTextMeshRenderer;
        TMP_Text cardNameText;
        SpriteRenderer cardRaritySpriteRenderer;
        SpriteRenderer cardDescriptionTextBoxSpriteRenderer;
        MeshRenderer cardDescriptionTextMeshRenderer;
        TMP_Text cardDescriptionText;
        SpriteRenderer cardTypeIconSpriteRenderer;
        MeshRenderer cardTypeTextMeshRenderer;
        TMP_Text cardTypeText;
        MeshRenderer cardSpeedTextMeshRenderer;
        TMP_Text cardSpeedText;

        #endregion

        [MenuItem("Window/Card Creator")]

        public static void ShowWindow()
        {
            EditorWindow cardCreatorEditorWindow = EditorWindow.GetWindow(typeof(CardCreatorEditorWindow));

            cardCreatorEditorWindow.titleContent = new GUIContent("Card Creator");

            cardCreatorEditorWindow.minSize = new Vector2((cardSize.x * 2f) + 100f, cardSize.y + 50f);

            cardCreatorEditorWindow.maxSize = cardCreatorEditorWindow.minSize;
        }

        private void OnEnable()
        {
            sceneCamera = Camera.main;

            if (sceneCamera == null)
                return;

            float orthographicSize = sceneCamera.orthographicSize;

            Transform cardTransform = new GameObject("Card").GetComponent<Transform>();

            cardTransform.gameObject.hideFlags = HideFlags.NotEditable;

            cardFrameOutlineSpriteRenderer = new GameObject("Frame Outline", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();

            cardFrameOutlineSpriteRenderer.gameObject.hideFlags = HideFlags.NotEditable;

            cardFrameOutlineSpriteRenderer.gameObject.transform.SetParent(cardTransform);

            cardInnerFrameSpriteRenderer = new GameObject("Inner Frame", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();

            cardInnerFrameSpriteRenderer.gameObject.hideFlags = HideFlags.NotEditable;

            cardInnerFrameSpriteRenderer.gameObject.transform.SetParent(cardTransform);

            cardIllustrationMaskSpriteRenderer = new GameObject("Illustration Mask", typeof(SpriteMask)).GetComponent<SpriteMask>();

            cardIllustrationMaskSpriteRenderer.gameObject.hideFlags = HideFlags.NotEditable;

            cardIllustrationMaskSpriteRenderer.gameObject.transform.SetParent(cardTransform);

            GameObject cardIllustration = new GameObject("Illustration", typeof(SpriteRenderer));

            cardIllustration.gameObject.hideFlags = HideFlags.NotEditable;

            cardIllustration.transform.SetParent(cardIllustrationMaskSpriteRenderer.gameObject.transform);

            cardIllustrationTransform = cardIllustration.GetComponent<Transform>();

            cardIllustrationSpriteRenderer = cardIllustration.GetComponent<SpriteRenderer>();

            SpriteRenderer cardNameTextBoxSpriteRenderer = new GameObject("Name Text Box", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();

            cardNameTextBoxSpriteRenderer.gameObject.hideFlags = HideFlags.NotEditable;

            cardNameTextBoxSpriteRenderer.gameObject.transform.SetParent(cardTransform);

            GameObject cardNameText = new GameObject("Text", typeof(MeshRenderer), typeof(TextMeshProUGUI));

            cardNameText.gameObject.hideFlags = HideFlags.NotEditable;

            cardNameText.transform.SetParent(cardNameTextBoxSpriteRenderer.gameObject.transform);

            cardNameTextMeshRenderer = cardNameText.GetComponent<MeshRenderer>();

            this.cardNameText = cardNameText.GetComponent<TMP_Text>();

            cardRaritySpriteRenderer = new GameObject("Rarity", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();

            cardRaritySpriteRenderer.gameObject.hideFlags = HideFlags.NotEditable;

            cardRaritySpriteRenderer.gameObject.transform.SetParent(cardTransform);

            SpriteRenderer cardDescriptionTextBoxSpriteRenderer = new GameObject("Description Text Box", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();

            cardDescriptionTextBoxSpriteRenderer.gameObject.hideFlags = HideFlags.NotEditable;

            cardDescriptionTextBoxSpriteRenderer.gameObject.transform.SetParent(cardTransform);

            GameObject cardDescriptionText = new GameObject("Text", typeof(MeshRenderer), typeof(TextMeshProUGUI));

            cardDescriptionText.gameObject.hideFlags = HideFlags.NotEditable;

            cardDescriptionText.transform.SetParent(cardDescriptionTextBoxSpriteRenderer.gameObject.transform);

            cardDescriptionTextMeshRenderer = cardDescriptionText.GetComponent<MeshRenderer>();

            this.cardDescriptionText = cardDescriptionText.GetComponent<TMP_Text>();

            cardTypeIconSpriteRenderer = new GameObject("Type Icon", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();

            cardTypeIconSpriteRenderer.gameObject.hideFlags = HideFlags.NotEditable;

            cardTypeIconSpriteRenderer.gameObject.transform.SetParent(cardTransform);

            GameObject cardTypeText = new GameObject("Type Text", typeof(MeshRenderer), typeof(TextMeshProUGUI));

            cardTypeText.gameObject.hideFlags = HideFlags.NotEditable;

            cardTypeText.transform.SetParent(cardTransform);

            cardTypeTextMeshRenderer = cardTypeText.GetComponent<MeshRenderer>();

            this.cardTypeText = cardTypeText.GetComponent<TMP_Text>();

            GameObject cardSpeedText = new GameObject("Speed Text", typeof(MeshRenderer), typeof(TextMeshProUGUI));

            cardSpeedText.gameObject.hideFlags = HideFlags.NotEditable;

            cardSpeedText.transform.SetParent(cardTransform);

            cardSpeedTextMeshRenderer = cardTypeText.GetComponent<MeshRenderer>();

            this.cardSpeedText = cardSpeedText.GetComponent<TMP_Text>();

            string[] zoomIconsPaths = Array.ConvertAll(AssetDatabase.FindAssets("Zoom Icon"), (zoomIconGUID) => 
            {
                return AssetDatabase.GUIDToAssetPath(zoomIconGUID);
            });

            for (int i = 0, zoomIconsPathsLength = zoomIconsPaths.Length; i < zoomIconsPathsLength; i++)
            {
                string zoomIconPath = zoomIconsPaths[i];

                if (zoomIconPath.Contains("Utilities") && zoomIconPath.Contains("Card Creator"))
                {
                    zoomIcon = AssetDatabase.LoadAssetAtPath<Sprite>(zoomIconPath);

                    break;
                }
            }

            onDestroy = () =>
            {
                sceneCamera.orthographicSize = orthographicSize;

                DestroyImmediate(cardTransform.gameObject);
            };
        }

        void OnGUI()
        {
            Rect position = new Rect(0f, 0f, this.position.width, this.position.height);

            Rect leftPosition = new Rect(position.x, position.y, position.width / 2f, position.height / 2f);

            EditorGUI.LabelField(new Rect(leftPosition.x + 25f, leftPosition.y + 10f, leftPosition.width - 50f, 18f), "Variables", EditorStyles.boldLabel);

            cardType = (Card.Type)EditorGUI.EnumPopup(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 18f, leftPosition.width - 50f, 18f), "Type", cardType);

            cardFrameOutlineSprite = (Sprite)EditorGUI.ObjectField(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 41f, leftPosition.width - 50f, 18f), "Frame Outline Sprite", cardFrameOutlineSprite, typeof(Sprite), false);

            cardInnerFrameSprite = (Sprite)EditorGUI.ObjectField(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 62f, leftPosition.width - 50f, 18f), "Inner Frame Sprite", cardInnerFrameSprite, typeof(Sprite), false);

            cardIllustrationMaskSprite = (Sprite)EditorGUI.ObjectField(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 85f, leftPosition.width - 50f, 18f), "Illustration Mask Sprite", cardIllustrationMaskSprite, typeof(Sprite), false);

            cardIllustrationOffset = EditorGUI.Vector3Field(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 108f, leftPosition.width - 50f, 18f), "Illustration Offset", cardIllustrationOffset);

            cardIllustrationSprite = (Sprite)EditorGUI.ObjectField(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 154f, leftPosition.width - 50f, 18f), "Illustration Sprite", cardIllustrationSprite, typeof(Sprite), false);

            cardName = EditorGUI.TextField(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 177f, leftPosition.width - 50f, 18f), "Name", cardName);

            cardNameFont = (TMP_FontAsset)EditorGUI.ObjectField(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 200f, leftPosition.width - 50f, 18f), "Name Font", cardNameFont, typeof(TMP_FontAsset), false);

            cardRarity = (Card.Rarity)EditorGUI.EnumPopup(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 223f, leftPosition.width - 50f, 18f), "Rarity", cardRarity);

            cardRaritySprite = (Sprite)EditorGUI.ObjectField(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 246f, leftPosition.width - 50f, 18f), "Rarity Sprite", cardRaritySprite, typeof(Sprite), false);

            cardDescriptionTextBoxSprite = (Sprite)EditorGUI.ObjectField(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 269f, leftPosition.width - 50f, 18f), "Description Text Box Sprite", cardDescriptionTextBoxSprite, typeof(Sprite), false);

            EditorGUI.LabelField(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 292f, leftPosition.width - 50f, 18f), "Description");

            cardDescription = EditorGUI.TextArea(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 315f, leftPosition.width - 50f, 90f), cardDescription);

            cardDescriptionFont = (TMP_FontAsset)EditorGUI.ObjectField(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 410f, leftPosition.width - 50f, 18f), "Description Font", cardDescriptionFont, typeof(TMP_FontAsset), false);

            cardTypeIconSprite = (Sprite)EditorGUI.ObjectField(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 433f, leftPosition.width - 50f, 18f), "Type Icon Sprite", cardTypeIconSprite, typeof(Sprite), false);

            cardHasSpeed = EditorGUI.Toggle(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 456f, 18f, 18f), "Speed", cardHasSpeed);

            if(cardHasSpeed)
                cardSpeed = EditorGUI.IntSlider(new Rect(leftPosition.x + 25f, leftPosition.y + 15f + 479f, leftPosition.width - 50f, 18f), cardSpeed, 1, 5);

            GUI.Box(new Rect(position.x + (position.width / 2f), position.y, 1f, position.height), "");

            Rect rigthPosition = new Rect(position.x + (position.width / 2f), position.y, position.width / 2f, position.height);

            if (sceneCamera == null)
            {
                GUIStyle labelStyle = GUI.skin.GetStyle("Label");

                labelStyle.alignment = TextAnchor.UpperCenter;

                EditorGUI.LabelField(new Rect(rigthPosition.x + 25f, rigthPosition.y + (rigthPosition.height / 2f), rigthPosition.width - 50f, 18f), "No Camera Found In The Scene", labelStyle);
            }
            else
            {
                Handles.SetCamera(sceneCamera);

                Handles.DrawCamera(new Rect(rigthPosition.x + 25f, rigthPosition.y + 25f, rigthPosition.width - 50f, rigthPosition.height - 50f), sceneCamera);

                EditorGUI.LabelField(new Rect(rigthPosition.x + 50f, rigthPosition.y + rigthPosition.height - 58f, 18f, 18f), new GUIContent(zoomIcon.texture));

                sceneCamera.orthographicSize = EditorGUI.Slider(new Rect(rigthPosition.x + 68f, rigthPosition.y + rigthPosition.height - 58f, rigthPosition.width - 118f, 18f), sceneCamera.orthographicSize, 1f, 10f);
            }
        }

        void OnDestroy()
        {
            onDestroy?.Invoke();
        }
    }
}
