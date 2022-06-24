namespace AwgenCore
{
  public interface IRegistrable<T> where T : IRegistrable<T>
  {
    /// <summary>
    /// Gets the unquie resource location for this registrable object type.
    /// </summary>
    /// <returns>The resource location pointer.</returns>
    ResourceLocation<T> GetResourceLocation();
  }
}
