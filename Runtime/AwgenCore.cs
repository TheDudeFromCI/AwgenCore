using System;
using System.Collections.Generic;
using UnityEngine;
using AwgenCore.Voxel;

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


    [Header("TEMP")]
    [SerializeField]
    private Mesh cubeMesh;

    private Dictionary<Type, object> registries = new Dictionary<Type, object>();


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

      InitializeBlockRegistry();
    }

    // TODO Remove this method once a more official implementation is working.
    private void InitializeBlockRegistry()
    {
      var registry = GetOrCreateRegistry<BlockType>();
      registry.Register(new StoneBlock(MeshData.CreateFromUnityMesh(this.cubeMesh)));
      registry.Register(new VoidBlock());
    }


    /// <summary>
    /// Called every frame to execute all pending rendering tasks on the logic
    /// server.
    /// </summary>
    protected void Update()
    {
      LogicServer.SyncRendering();
    }


    /// <summary>
    /// When the game ends, stop the logic server.
    /// </summary>
    protected void OnDestroy()
    {
      LogicServer?.Stop();
    }


    /// <summary>
    /// Gets the registry for the provided registrable type, or creates it if it
    /// does not yet exist.
    /// </summary>
    /// <typeparam name="T">The data type of registrable of the Registry.</typeparam>
    /// <returns>The registry for the given data type.</returns>
    public Registry<T> GetOrCreateRegistry<T>() where T : IRegistrable<T>
    {
      Type t = typeof(T);
      if (this.registries.ContainsKey(t)) return (Registry<T>)this.registries[t];

      Registry<T> r = new Registry<T>();
      registries[t] = r;
      return r;
    }
  }
}
