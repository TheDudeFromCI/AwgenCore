using System;
using NUnit.Framework;
using AwgenCore;

public class QualifiedNameTests
{
  public class TestFoodType : IRegistrable<TestFoodType>
  {
    public TestFoodType(string classname, string path, string name) : base(new ResourceLocation<TestFoodType>(classname, path, name))
    {
      SetDefaultProperty("cooked", "false");
      SetDefaultProperty("is_meat", "false");
      SetDefaultProperty("name", "");
    }
  }


  public Registry<TestFoodType> GetRegistry()
  {
    var registry = new Registry<TestFoodType>();
    registry.Register(new TestFoodType("awgen", "", "apple"));
    registry.Register(new TestFoodType("awgen", "", "banana"));
    registry.Register(new TestFoodType("awgen", "meat/", "steak"));
    return registry;
  }


  [Test]
  public void SimpleName()
  {
    var registry = GetRegistry();
    var qualified = new QualifiedName<TestFoodType>(registry, "awgen:apple");

    Assert.NotNull(qualified.RegisterableInstance);
    Assert.AreEqual("awgen", qualified.Resource.Classname);
    Assert.AreEqual("", qualified.Resource.Path);
    Assert.AreEqual("apple", qualified.Resource.Name);
    Assert.AreEqual(0, qualified.Properties.Count);
  }


  [Test]
  public void SimpleName_With2Properties()
  {
    var registry = GetRegistry();
    var qualified = new QualifiedName<TestFoodType>(registry, "awgen:meat/steak[cooked=true, is_meat=true,]");

    Assert.NotNull(qualified.RegisterableInstance);
    Assert.AreEqual("awgen", qualified.Resource.Classname);
    Assert.AreEqual("meat/", qualified.Resource.Path);
    Assert.AreEqual("steak", qualified.Resource.Name);
    Assert.AreEqual(2, qualified.Properties.Count);
    Assert.AreEqual("true", qualified.Properties["cooked"]);
    Assert.AreEqual("true", qualified.Properties["is_meat"]);
  }


  [Test]
  public void SimpleName_PropertyInQuote()
  {
    var registry = GetRegistry();
    var qualified = new QualifiedName<TestFoodType>(registry, "awgen:banana[name=\"Banana, Rare\"]");

    Assert.NotNull(qualified.RegisterableInstance);
    Assert.AreEqual("awgen", qualified.Resource.Classname);
    Assert.AreEqual("", qualified.Resource.Path);
    Assert.AreEqual("banana", qualified.Resource.Name);
    Assert.AreEqual(1, qualified.Properties.Count);
    Assert.AreEqual("Banana, Rare", qualified.Properties["name"]);
  }


  [Test]
  public void NoClassname_ExpectError()
  {
    var registry = GetRegistry();

    Assert.Throws<ArgumentException>(() => new QualifiedName<TestFoodType>(registry, "stone"));
    Assert.Throws<ArgumentException>(() => new QualifiedName<TestFoodType>(registry, "awgen:/stone"));
    Assert.Throws<ArgumentException>(() => new QualifiedName<TestFoodType>(registry, "dungeon:rock[meep=]"));
    Assert.Throws<ArgumentException>(() => new QualifiedName<TestFoodType>(registry, "food:red/apple[edible=\"true]"));
    Assert.Throws<ArgumentException>(() => new QualifiedName<TestFoodType>(registry, "food:apple[fruit=true,,]"));
    Assert.Throws<ArgumentException>(() => new QualifiedName<TestFoodType>(registry, "awgen:void"));
  }
}
