using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class IngredientScriptableObject : ScriptableObject
{
    [SerializeField] string ingredientName;
    [SerializeField] public Sprite ingredientIcon;

    public string GetIngredient(){
        return ingredientName;
    }
}
