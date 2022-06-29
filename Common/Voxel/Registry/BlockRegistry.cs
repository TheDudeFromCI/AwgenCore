namespace AwgenCore.Voxel
{
  /// <summary>
  /// A small container for all default block types that are loaded in Awgen
  /// Voxel.
  /// </summary>
  public static class BlockRegistry
  {
    public readonly static VoidBlock VOID_BLOCK = new VoidBlock();

    public readonly static StoneBlock STONE_BLOCK = new StoneBlock();


    /// <summary>
    /// Create the registry and register all default block types on class access.
    /// </summary>
    static BlockRegistry()
    {
      var registry = GlobalRegistry.GetOrCreateRegistry<BlockType>();
      registry.Register(VOID_BLOCK);
      registry.Register(STONE_BLOCK);
    }


    /// <summary>
    /// Initializes the block type repository. Exists only to allow for the
    /// static constructor to be executed.
    /// </summary>
    public static void Initialize()
    {
    }
  }
}
