using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IngredientScriptableObject : ScriptableObject
{
    [SerializeField] string ingredientName;

    public string GetIngredient(){
        return ingredientName;
    }
}
