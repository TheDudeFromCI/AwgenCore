using System;

namespace AwgenCore
{
  /// <summary>
  /// A resource identifier that represents a name and namespace pair for a registered resource pointer.
  /// </summary>
  public class ResourceLocation<T> where T : IRegistrable<T>
  {
    /// <summary>
    /// The name of the resource.
    /// </summary>
    public readonly string name;


    /// <summary>
    /// The namespace of the resource.
    /// </summary>
    public readonly string classname;


    /// <summary>
    /// The resource type.
    /// </summary>
    public readonly Type type;


    /// <summary>
    /// Creates a new ResourceLocation instance.
    /// </summary>
    /// <param name="name">The name of the resource.</param>
    /// <param name="classname">The namespace of the resource.</param>
    /// <exception cref="ArgumentException">If the name or classname are empty or null.</exception>
    public ResourceLocation(string name, string classname)
    {
      if (string.IsNullOrEmpty(name))
      {
        throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
      }

      if (string.IsNullOrEmpty(classname))
      {
        throw new ArgumentException($"'{nameof(classname)}' cannot be null or empty.", nameof(classname));
      }

      this.name = name;
      this.classname = classname;
      this.type = typeof(T);
    }


    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
      if ((obj == null) || !this.GetType().Equals(obj.GetType())) return false;

      ResourceLocation<T> res = (ResourceLocation<T>)obj;
      return res.name.Equals(this.name)
          && res.classname.Equals(this.classname)
          && res.type.Equals(this.type);
    }


    /// <inheritdoc/>
    public override int GetHashCode()
    {
      int hash = 17;
      hash = 31 * hash + this.name.GetHashCode();
      hash = 31 * hash + this.classname.GetHashCode();
      hash = 31 * hash + this.type.GetHashCode();
      return hash;
    }


    /// <inheritdoc/>
    public override string ToString()
    {
      return String.Format("<{0} {1}:{2}>", this.type.Name, this.classname, this.name);
    }
  }
}
