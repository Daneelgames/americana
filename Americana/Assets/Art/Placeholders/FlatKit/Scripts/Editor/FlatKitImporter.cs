
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace FlatKit
{
    public class FlatKitImporter : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var isTargetAsset = importedAssets.Any(path => path.StartsWith("FlatKit/"));
            if (isTargetAsset)
            {
                InitializeOnLoad();
            }
        }

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            if (UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset == null)
            {
                Debug.Log("Importing Flat Kit. Render pipeline: Built-in.");   
            }
            else
            {
                Debug.Log("Importing Flat Kit. Render pipeline: URP.");
            }
        }
    }
}