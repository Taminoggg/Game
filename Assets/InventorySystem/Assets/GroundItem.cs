using UnityEditor;
using UnityEngine;

namespace InventorySystem.Assets
{
    public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
    {
        public ItemObject item;

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
            GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
            EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
        }
    }
}
