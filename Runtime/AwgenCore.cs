using UnityEngine;

namespace AwgenCore
{
  public class AwgenCore : MonoBehaviour
  {
    private static bool initialized;

    void Awake()
    {
      if (initialized) return;
      initialized = true;

      Voxel.BlockRegistry.Initialize();
    }
  }
}
