#if UNITY_EDITOR
using UnityEditor;
#endif
    
using UnityEngine;
using UnityEngine.Rendering;
    
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
class TransparencySortGraphicsHelper
{
    static TransparencySortGraphicsHelper()
    {
        OnLoad();
    }
    
    [RuntimeInitializeOnLoadMethod]
    static void OnLoad()
    {
        GraphicsSettings.transparencySortMode = TransparencySortMode.Default;
        GraphicsSettings.transparencySortAxis = new Vector3(1f, 1f, 1f);
    }
}
    
