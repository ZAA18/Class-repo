using Unity.VisualScripting;
using UnityEngine;

public static class AniimatorExtensions  
{
    public static bool HasParameterOfType(this Animator self, string paraName, AnimatorControllerParameterType type)
    {
        foreach (AnimatorControllerParameter param in self.parameters)
        {
            if (param.type == type && param.name == paraName)
                return true;
        }
        return false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
}
