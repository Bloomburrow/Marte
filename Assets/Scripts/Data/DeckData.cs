using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeckData
{
    #region Variables & Properties

    [SerializeField] int size;
    public int _size
    {
        get => size;
    }
	[SerializeField] List<Vector2Int> cardsIndexes;
    public List<Vector2Int> _cardsIndexes
    {
        get => cardsIndexes;
    }

	#endregion
}
