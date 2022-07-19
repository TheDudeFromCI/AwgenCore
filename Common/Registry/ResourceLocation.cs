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
    /// <exception cref="ArgumentNullException">If the path is null.</exception>
    /// <exception cref="ArgumentException">If the classname, path, or name are in an invalid format.</exception>
    public ResourceLocation(string classname, string path, string name)
    {
      if (string.IsNullOrEmpty(classname))
        throw new ArgumentException($"'{nameof(classname)}' cannot be null or empty.", nameof(classname));

      if (path == null)
        throw new ArgumentNullException($"'{nameof(path)}' cannot be null.", nameof(path));

      if (string.IsNullOrEmpty(name))
        throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));

      var nameRegex = new Regex(@"^[a-z0-9_]+$");
      var pathRegex = new Regex(@"^([a-z0-9_]+/)*$");

      if (!nameRegex.IsMatch(classname)) throw new ArgumentException("Invalid classname format: " + classname, nameof(classname));
      if (!pathRegex.IsMatch(path)) throw new ArgumentException("Invalid path format: " + path, nameof(path));
      if (!nameRegex.IsMatch(name)) throw new ArgumentException("Invalid name format: " + name, nameof(name));

      Name = name;
      Path = path;
      Classname = classname;
      Type = typeof(T);
    }


    /// <summary>
    /// Creates a new ResourceLocation instance from a parsed full name string.
    /// </summary>
    /// <param name="location">The name of the resource to be parsed.</param>
    /// <exception cref="ArgumentException">If the location is null, empty, or cannot be parsed.</exception>
    public ResourceLocation(string location)
    {
      if (string.IsNullOrEmpty(location))
        throw new ArgumentException($"'{nameof(location)}' cannot be null or empty.", nameof(location));

      var regex = new Regex(@"^([a-z0-9_]+):([a-z0-9_]+/)*([a-z0-9_]+)$");
      var match = regex.Match(location);
      if (!match.Success) throw new ArgumentException("Invalid resource location string! Cannot parse.", nameof(location));

      Name = match.Groups[3].Value;
      Path = match.Groups[2].Value ?? "";
      Classname = match.Groups[1].Value;
      Type = typeof(T);
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
