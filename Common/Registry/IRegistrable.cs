namespace AwgenCore
{
  public abstract class IRegistrable<T> where T : IRegistrable<T>
  {
    private readonly ResourceLocation<T> resourceLocation;


    /// <summary>
    /// Creates a new Registrable resource instance.
    /// </summary>
    /// <param name="name">The name of this resource.</param>
    /// <param name="classname">The namespace of this resource.</param>
    public IRegistrable(string name, string classname)
    {
      this.resourceLocation = new ResourceLocation<T>(name, classname);
    }


    /// <summary>
    /// Gets the unquie resource location for this registrable object type. This
    /// value should never change.
    /// </summary>
    /// <returns>The resource location pointer.</returns>
    public ResourceLocation<T> GetResourceLocation()
    {
      return this.resourceLocation;
    }


    /// <inheritdoc/>
    public override string ToString()
    {
      return this.resourceLocation.ToString();
    }
  }
}
