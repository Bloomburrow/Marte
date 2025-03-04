using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorCoroutine
{
    #region Variables & Properties

    static EditorCoroutine instance;
    public static EditorCoroutine _instance
    {
        get
        {
            if (instance == null)
                instance = new EditorCoroutine();

            return instance;
        }
    }

    private List<IEnumerator> coroutines;
	private IEnumerator currentCoroutine;
	private float currentCoroutineDelay = 0;

    #endregion
	
	EditorCoroutine()
	{
		coroutines = new List<IEnumerator>();

		EditorApplication.update += Update;
	}
	
	public void Update()
	{
		if (currentCoroutine == null && coroutines.Count > 0)
		{
			currentCoroutine = coroutines[0];

			coroutines.Remove(currentCoroutine);
		}
		
		if (currentCoroutine != null && Time.realtimeSinceStartup > currentCoroutineDelay)
		{
			if (!currentCoroutine.MoveNext())
				currentCoroutine = null;
			else if (currentCoroutine.Current is WaitForSeconds)
			{
				WaitForSeconds waitForSeconds = (WaitForSeconds)currentCoroutine.Current;

				currentCoroutineDelay = Time.realtimeSinceStartup + waitForSeconds._seconds;
			}
		}
	}

    public static void StartCoroutine(IEnumerator coroutine)
    {
        _instance.coroutines.Add(coroutine);
    }
}
