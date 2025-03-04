using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardsData: IEnumerator, IEnumerable
{
	#region Variables & Properties

	[SerializeField] List<CardData> cardsData;
	int currentIndex = -1;
	public CardData this[int index] => cardsData[index];
	public int _count => cardsData.Count;

	#endregion

	public object Current
	{
		get { return cardsData[currentIndex]; }
	}

	public bool MoveNext()
	{
		currentIndex++;

		return currentIndex < cardsData.Count;
	}

	public void Reset()
	{
		currentIndex = -1;
	}

	public IEnumerator GetEnumerator()
	{
		return (IEnumerator)this;
	}

	public int Add(CardData cardData)
	{
		cardsData.Add(cardData);

		return _count - 1;
	}
}
