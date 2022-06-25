using System;
using System.Collections.Generic;

namespace AwgenCore
{
  /// <summary>
  /// A singleton collection of all registries that currently exist within the
  /// application instance.
  /// </summary>
  public static class GlobalRegistry
  {
    private static Dictionary<Type, object> registries = new Dictionary<Type, object>();


    /// <summary>
    /// Gets the registry for the provided registrable type, or creates it if it
    /// does not yet exist.
    /// </summary>
    /// <typeparam name="T">The data type of registrable of the Registry.</typeparam>
    /// <returns>The registry for the given data type.</returns>
    public static Registry<T> GetOrCreateRegistry<T>() where T : IRegistrable<T>
    {
      Type t = typeof(T);
      if (registries.ContainsKey(t)) return (Registry<T>)registries[t];

      Registry<T> r = new Registry<T>();
      registries[t] = r;
      return r;
    }
  }
}
