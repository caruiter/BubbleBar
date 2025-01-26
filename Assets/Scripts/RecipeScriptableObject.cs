using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu]
public class RecipeScriptableObject : ScriptableObject
{
    [SerializeField] public string drinkName;

    [SerializeField] public List<Image> icons;
    [SerializeField] public List<string> ingredients;
}
