#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
#endif

namespace Attributes
{
    public partial class CardPicker
    {
        public enum Card
        {
            Ambush = 2,
            BetrayTheOwner = 26,
            BlueVial = 22,
            BulkAttack = 4,
            BurnTheSoul = 29,
            CallReinforcements = 32,
            CleanseTheSoul = 40,
            CorrosiveTouch = 7,
            Corrupt = 39,
            CursedCoins = 23,
            DamageMultiply = 33,
            DemonGrip = 28,
            EmeraldShield = 19,
            EvilWound = 25,
            Execute = 36,
            FireBall = 11,
            GuessTheFuture = 12,
            Health = 15,
            ImpresiveRage = 38,
            LaceratingLightning = 13,
            LeaveTheBody = 27,
            LivingFog = 20,
            MagicDage = 24,
            ManaAbsorption = 18,
            ManaBursts = 17,
            ManaRecovery = 14,
            None = ~0,
            OpenATreasureChest = 35,
            OpenMind = 16,
            OpenTheRedEye = 37,
            OpportunisticTheft = 34,
            Prison = 30,
            Punch = 0,
            QuickAssault = 1,
            Revenge = 3,
            RunicSword = 21,
            SpikeHammerAttack = 5,
            StoneSkin = 9,
            TheDamageMakesMeStrong = 31,
            TrickAttack = 8,
            Twice = 10,
            VampireBite = 6
        }

        #if UNITY_EDITOR

        [InitializeOnLoadMethod]
        static void InitializeOnLoad()
        {
			EditorApplication.delayCall += CardInitializeOnLoad;
		}

        static void CardInitializeOnLoad()
        {
			EditorApplication.delayCall -= CardInitializeOnLoad;

			List<string> currentCards = new List<string>
			{
				"            None = ~0,"
			};

			string[] prefabsPaths = Array.ConvertAll(AssetDatabase.FindAssets("t:prefab"), (prefabGUID) =>
			{
				return AssetDatabase.GUIDToAssetPath(prefabGUID);
			});

			foreach (string prefabPath in prefabsPaths)
			{
				global::Card card = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath).GetComponent<global::Card>();

				if (card != null)
					currentCards.Add($"            {card.GetType().Name} = {card._identificationNumber},");
			}

			currentCards.Sort();

			string lastCurrentCard = currentCards[currentCards.Count - 1];

			currentCards[currentCards.Count - 1] = lastCurrentCard.Remove(lastCurrentCard.Length - 1);

			string[] scriptsPaths = Array.ConvertAll(AssetDatabase.FindAssets($"t:Script {nameof(Card)}"), (scriptGUID) =>
			{
				return AssetDatabase.GUIDToAssetPath(scriptGUID);
			});

			string scriptPath = null;

			for (int i = 0, scriptsPathsLength = scriptsPaths.Length; i < scriptsPathsLength; i++)
			{
				if (new DirectoryInfo(Path.GetDirectoryName(scriptsPaths[i])).Name == "Enums")
				{
					scriptPath = scriptsPaths[i];

					break;
				}
			}

			List<string> scriptLines = new List<string>(File.ReadAllLines(scriptPath));

			for (int i = 0; i < scriptLines.Count; i++)
			{
				string scriptLine = scriptLines[i];

				if (scriptLine.Contains("enum"))
				{
					i += 2;

					for (int j = i; j < scriptLines.Count; j++)
					{
						if (scriptLines[j].Contains("}"))
						{
							List<string> oldCards = scriptLines.GetRange(i, j - i);

							if (Enumerable.SequenceEqual(currentCards, oldCards))
								return;

							scriptLines.RemoveRange(i, j - i);

							break;
						}
					}

					scriptLines.InsertRange(i, currentCards);

					File.WriteAllLines(scriptPath, scriptLines);

					AssetDatabase.Refresh();

					break;
				}
			}
		}

		#endif
	}
}
