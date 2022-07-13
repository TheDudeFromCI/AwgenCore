using System;
using UnityEngine;

namespace AwgenCore
{
  /// <summary>
  /// The main connector for loading and managing the Awgen Core framework. This
  /// behaviour should be place on a top-level, empty game object with no other
  /// components or children. This object should then be placed on every scene.
  /// Awgen Core will manage redundency and initialization from there.
  /// </summary>
  public class AwgenCore : MonoBehaviour
  {
    private static bool initialized;


    [Header("Logic Server")]

    [SerializeField, Range(1, 100)]
    [Tooltip("The number of game ticks that occur each second within the logic thread.")]
    private int gameTickRate = 20;


    private LogicServer logicServer;


    /// <summary>
    /// Called when the game object is first initialized to setup AwgenCore
    /// specific handlers, registry, and servers/processes as needed.
    /// </summary>
    protected void Awake()
    {
      if (initialized) return;
      DontDestroyOnLoad(gameObject);
      initialized = true;

      var workers = Math.Max(SystemInfo.processorCount - 1, 1);
      this.logicServer = new LogicServer(this.gameTickRate, workers);

      Voxel.BlockRegistry.Initialize();
    }
  }
}
