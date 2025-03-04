using Attributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordPanel : Panel
{
    #region Components

    [Foldout("Components (RecordPanel)/External")]
    [SerializeField] List<Image> recordsImages;

    #endregion

    public void ShowRecord(Sprite recordSprite)
    {
        ShowRecord(0, recordSprite);
    }

    void ShowRecord(int index, Sprite recordSprite)
    {
        Image recordsImage = recordsImages[index];

        if (recordsImage.gameObject.activeSelf)
        {
            Sprite lastRecordSprite = recordsImage.sprite;

            index += 1;

            if(index < recordsImages.Count)
                ShowRecord(index, lastRecordSprite);

            recordsImage.sprite = recordSprite;
        }
        else
        {
            recordsImage.gameObject.SetActive(true);

            recordsImage.sprite = recordSprite;
        }
    }
}
