using System;
using System.Collections.Generic;

namespace AwgenCore
{
  public class Registry<T> where T : IRegistrable<T>
  {
    private readonly Dictionary<ResourceLocation<T>, T> entries = new Dictionary<ResourceLocation<T>, T>();


    /// <summary>
    /// Gets a registable instance from this registry with the given resource
    /// location.
    /// </summary>
    /// <param name="resourceLocation">The resource location.</param>
    /// <returns>The registrable instance, or null if it does not exist.</returns>
    public T this[ResourceLocation<T> resourceLocation]
    {
      get
      {
        if (!this.entries.ContainsKey(resourceLocation)) return null;
        return this.entries[resourceLocation];
      }
    }


    /// <summary>
    /// Gets a registable instance from this registry with the given resource
    /// location name.
    /// </summary>
    /// <param name="resourceLocation">The resource location string to parse.</param>
    /// <returns>The registrable instance, or null if it does not exist.</returns>
    public T this[string location]
    {
      get
      {
        var resourceLocation = new ResourceLocation<T>(location);
        return this[resourceLocation];
      }
    }


    /// <summary>
    /// Registers a new entry into this registey.
    /// </summary>
    /// <param name="entry">The entry to add.</param>
    /// <exception cref="ArgumentNullException">If the entry is null.</exception>
    public void Register(T entry)
    {
      if (entry is null) throw new ArgumentNullException(nameof(entry));
      this.entries.Add(entry.Resource, entry);
    }
  }
}
