using UnityEngine;

public interface IEvolvable
{
    public void Evolve<T1>(CharacterData characterData, string newName, Sprite newIllustration, string newDescription);
}
