using System;
using System.Text.RegularExpressions;

namespace AwgenCore
{
  /// <summary>
  /// A resource identifier that represents a name and namespace pair for a
  /// registered resource pointer.
  /// </summary>
  public class ResourceLocation<T> where T : IRegistrable<T>
  {
    /// <summary>
    /// The name of the resource.
    /// </summary>
    public readonly string Name;


    /// <summary>
    /// The namespace of the resource.
    /// </summary>
    public readonly string Classname;


    /// <summary>
    /// The path of the resource within the classname.
    /// </summary>
    public readonly string Path;


    /// <summary>
    /// The resource type.
    /// </summary>
    public readonly Type Type;


    /// <summary>
    /// Creates a new ResourceLocation instance.
    /// </summary>
    /// <param name="name">The name of the resource.</param>
    /// <param name="classname">The namespace of the resource.</param>
    /// <exception cref="ArgumentException">If the name or classname are empty or null.</exception>
    public ResourceLocation(string classname, string path, string name)
    {
      if (string.IsNullOrEmpty(classname))
        throw new ArgumentException($"'{nameof(classname)}' cannot be null or empty.", nameof(classname));

      if (path == null)
        throw new ArgumentException($"'{nameof(path)}' cannot be null.", nameof(path));

      if (string.IsNullOrEmpty(name))
        throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));

      var nameRegex = new Regex(@"^[a-z0-9_]+$");
      var pathRegex = new Regex(@"^([a-z0-9_]+/)*$");

      if (!nameRegex.IsMatch(classname)) throw new ArgumentException("Invalid classname format: " + classname, nameof(classname));
      if (!pathRegex.IsMatch(path)) throw new ArgumentException("Invalid path format: " + path, nameof(path));
      if (!nameRegex.IsMatch(name)) throw new ArgumentException("Invalid name format: " + name, nameof(name));

      this.Name = name;
      this.Path = path;
      this.Classname = classname;
      this.Type = typeof(T);
    }


    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
      if ((obj == null) || !this.GetType().Equals(obj.GetType())) return false;

      ResourceLocation<T> res = (ResourceLocation<T>)obj;
      return res.Name.Equals(this.Name)
          && res.Path.Equals(this.Path)
          && res.Classname.Equals(this.Classname)
          && res.Type.Equals(this.Type);
    }


    /// <inheritdoc/>
    public override int GetHashCode()
    {
      int hash = 17;
      hash = 31 * hash + this.Name.GetHashCode();
      hash = 31 * hash + this.Path.GetHashCode();
      hash = 31 * hash + this.Classname.GetHashCode();
      hash = 31 * hash + this.Type.GetHashCode();
      return hash;
    }


    /// <inheritdoc/>
    public override string ToString()
    {
      return String.Format("<{0} {1}:{3}{2}>", this.Type.Name, this.Classname, this.Name, this.Path);
    }
  }
}
