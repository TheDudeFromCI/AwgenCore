using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AwgenCore
{
  public abstract class IRegistrable<T> where T : IRegistrable<T>
  {
    private readonly Dictionary<string, string> defaultProperties;


    /// <summary>
    /// Gets the resource location pointer of this registerable instance.
    /// </summary>
    public ResourceLocation<T> Resource { get; private set; }


    /// <summary>
    /// Gets a read-only dictionary of default properties defines by this
    /// registerable instance. Any properties that are not listed in this
    /// dictionary cannot be provided to this registerable instance.
    /// </summary>
    public ReadOnlyDictionary<string, string> DefaultProperties { get; private set; }


    /// <summary>
    /// Creates a new Registrable resource instance.
    /// </summary>
    /// <param name="resource">The resource location for this registerable instance.</param>
    public IRegistrable(ResourceLocation<T> resource)
    {
      this.defaultProperties = new Dictionary<string, string>();

      Resource = resource;
      DefaultProperties = new ReadOnlyDictionary<string, string>(this.defaultProperties);
    }


    /// <inheritdoc/>
    public override string ToString()
    {
      return Resource.ToString();
    }


    /// <summary>
    /// Creates a new property for this block type and sets it's default value.
    /// This method is only intended to be called from within the constructor
    /// of child classes.
    /// </summary>
    /// <param name="property">The property name.</param>
    /// <param name="value">The default property value.</param>
    protected void SetDefaultProperty(string property, string value)
    {
      this.defaultProperties[property] = value;
    }
  }
}
