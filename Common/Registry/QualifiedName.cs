using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AwgenCore
{
  /// <summary>
  /// A read-only snapshot of a resource location pointer and a set of
  /// overriding propertt values for that resource.
  /// </summary>
  /// <typeparam name="T">The registerable instance type.</typeparam>
  public class QualifiedName<T> where T : IRegistrable<T>
  {
    private readonly string nameString;


    /// <summary>
    /// Gets the registerable instance being stored in this state.
    /// </summary>
    public T RegisterableInstance { get; private set; }


    /// <summary>
    /// Gets the resource location of this qualified name.
    /// </summary>
    public ResourceLocation<T> Resource { get; private set; }

    /// <summary>
    /// Gets a read-only dictionary of all properties defined by this qualified
    /// name.
    /// </summary>
    public ReadOnlyDictionary<string, string> Properties { get; private set; }


    /// <summary>
    /// Creates a new QualifiedName instance based off of a given input string.
    /// </summary>
    /// <param name="registry">The registry that the registerable instance is stored in.</param>
    /// <param name="qualifiedName">The qualified name text to parse.</param>
    /// <exception cref="ArgumentException">If the qualifiedName cannot be parsed.</exception>
    /// <exception cref="ArgumentException">If the qualifiedName contains invalid properties.</exception>
    /// <exception cref="ArgumentException">If the registry does not contain an entry for the indicated resource.</exception>
    public QualifiedName(Registry<T> registry, string qualifiedName)
    {
      var regex = new Regex(@"^([a-z0-9_]+):((?:[a-z0-9_]+\/)*)([a-z0-9_]+)(\[((?:\s*([a-z0-9_]+)\s*=\s*(?:([^\s,\[\]\""]+?)|\""(?<7>[^\""]*?)\""),?\s*)*)\])?$");
      var match = regex.Match(qualifiedName);
      if (!match.Success) throw new ArgumentException("Invalid qualified name string! Cannot parse.", nameof(qualifiedName));

      var classname = match.Groups[1].Value;
      var path = match.Groups[2]?.Value ?? "";
      var name = match.Groups[3].Value;
      var properties = new Dictionary<string, string>();

      Resource = new ResourceLocation<T>(classname, path, name);
      RegisterableInstance = registry[Resource];
      if (RegisterableInstance == null) throw new ArgumentException("Unable to retrieve instance from registry!", nameof(qualifiedName));

      if (match.Groups.Count >= 7)
      {
        for (var i = 0; i < match.Groups[6].Captures.Count; i++)
        {
          var key = match.Groups[6].Captures[i].Value;
          var value = match.Groups[7].Captures[i].Value;

          if (!RegisterableInstance.DefaultProperties.ContainsKey(key))
            throw new ArgumentException($"Property '{key}' is not valid for the given registry instance!", nameof(qualifiedName));

          if (!RegisterableInstance.DefaultProperties[key].Equals(value))
            properties.Add(key, value);
        }
      }

      Properties = new ReadOnlyDictionary<string, string>(properties);
      this.nameString = GenerateNameString();
    }


    /// <summary>
    /// Creates a new qualified name from a given resource location and a list
    /// of properties.
    /// </summary>
    /// <param name="registry">The registry that the registerable instance is stored in.</param>
    /// <param name="resource">The resource location for this qualified name.</param>
    /// <param name="properties">A dictionary of property-value pairs.</param>
    /// <exception cref="ArgumentException">If the properties dictionary contains invalid properties.</exception>
    /// <exception cref="ArgumentException">If the registry does not contain an entry for the indicated resource.</exception>
    public QualifiedName(Registry<T> registry, ResourceLocation<T> resource, Dictionary<string, string> properties)
    {
      var propClone = new Dictionary<string, string>();
      Resource = resource;
      RegisterableInstance = registry[Resource];
      if (RegisterableInstance == null) throw new ArgumentException("Unable to retrieve instance from registry!", nameof(resource));

      foreach (var pair in properties)
      {
        if (!RegisterableInstance.DefaultProperties.ContainsKey(pair.Key))
          throw new ArgumentException($"Property '{pair.Key}' is not valid for the given registry instance!", nameof(properties));

        if (!RegisterableInstance.DefaultProperties[pair.Key].Equals(pair.Value))
          propClone.Add(pair.Key, pair.Value);
      }

      Properties = new ReadOnlyDictionary<string, string>(propClone);
      this.nameString = GenerateNameString();
    }


    /// <summary>
    /// Generates a formatted qualified name string.
    /// </summary>
    /// <returns>A formatted name string for this qualified name.</returns>
    private string GenerateNameString()
    {
      var valueRegex = new Regex(@"^[a-zA-Z0-9_.]+$");

      var properties = "";
      foreach (var pair in Properties)
      {
        if (properties.Length > 0) properties += ", ";
        properties += $"{pair.Key}=";

        if (valueRegex.IsMatch(pair.Value)) properties += pair.Value;
        else properties += $"\"{pair.Value}\"";
      }

      if (properties.Length > 0) properties = $"[{properties}]";
      return $"{Resource.Classname}:{Resource.Path}{Resource.Name}{properties}";
    }


    /// <inheritdoc/>
    public override string ToString()
    {
      return this.nameString;
    }


    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
      if (obj == null) return false;
      if (!obj.GetType().Equals(GetType())) return false;

      var other = obj as QualifiedName<T>;
      return this.nameString.Equals(other.nameString);
    }


    /// <inheritdoc/>
    public override int GetHashCode()
    {
      return this.nameString.GetHashCode();
    }
  }
}
