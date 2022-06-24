using System;

namespace AwgenCore
{
  /// <summary>
  /// A resource identifier that represents a name and namespace pair for a registered resource pointer.
  /// </summary>
  public class ResourceLocation
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
    /// <param name="name">The name of the resource. Cannot be empty or null.</param>
    /// <param name="classname">The namespace of the resource. Cannot be empty or null.</param>
    /// <param name="type">The type of the resource. Cannot be null.</param>
    public ResourceLocation(string name, string classname, Type type)
    {
      if (string.IsNullOrEmpty(name))
      {
        throw new System.ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
      }

      if (string.IsNullOrEmpty(classname))
      {
        throw new System.ArgumentException($"'{nameof(classname)}' cannot be null or empty.", nameof(classname));
      }

      if (type == null)
      {
        throw new ArgumentNullException(nameof(type));
      }

      this.name = name;
      this.classname = classname;
      this.type = type;
    }


    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
      if ((obj == null) || !this.GetType().Equals(obj.GetType())) return false;

      ResourceLocation res = (ResourceLocation)obj;
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
      return String.Format("<{0} {1}:{2}>", this.type, this.classname, this.name);
    }
  }
}
