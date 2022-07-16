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
    [Header("Logic Server")]

    [SerializeField, Range(1, 100)]
    [Tooltip("The number of game ticks that occur each second within the logic thread.")]
    private int gameTickRate = 20;


    /// <summary>
    /// A global singleton reference to the AwgenCore framework instance.
    /// </summary>
    public static AwgenCore Instance;


    /// <summary>
    /// Gets the logic server that is being managed by AwgenCore.
    /// </summary>
    public LogicServer LogicServer { get; private set; }


    /// <summary>
    /// Called when the game object is first initialized to setup AwgenCore
    /// specific handlers, registry, and servers/processes as needed.
    /// </summary>
    protected void Awake()
    {
      if (Instance != null)
      {
        Destroy(this.gameObject);
        return;
      }

      Instance = this;
      DontDestroyOnLoad(this.gameObject);

      var workers = Math.Max(SystemInfo.processorCount - 1, 1);
      LogicServer = new LogicServer(this.gameTickRate, workers);

      Voxel.BlockRegistry.Initialize();
    }


    /// <summary>
    /// Called every frame to execute all pending rendering tasks on the logic
    /// server.
    /// </summary>
    protected void Update()
    {
      LogicServer.SyncRendering();
    }
  }
}
