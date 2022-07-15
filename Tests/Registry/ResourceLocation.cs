using System;
using NUnit.Framework;
using AwgenCore;

public class ResourceLocationTests
{
  public class TestStoneType : IRegistrable<TestStoneType>
  {
    public TestStoneType(ResourceLocation<TestStoneType> resource) : base(resource)
    {
    }
  }


  [Test]
  public void CreateResource()
  {
    var resource = new ResourceLocation<TestStoneType>("crafter", "dungeon/", "stone");

    Assert.AreEqual("crafter", resource.Classname);
    Assert.AreEqual("dungeon/", resource.Path);
    Assert.AreEqual("stone", resource.Name);
    Assert.AreEqual(typeof(TestStoneType), resource.Type);
  }


  [Test]
  public void CreateResource_LongPath()
  {
    var resource = new ResourceLocation<TestStoneType>("crafter", "dungeon/extra/deep/path/", "stone");

    Assert.AreEqual("crafter", resource.Classname);
    Assert.AreEqual("dungeon/extra/deep/path/", resource.Path);
    Assert.AreEqual("stone", resource.Name);
    Assert.AreEqual(typeof(TestStoneType), resource.Type);
  }


  [Test]
  public void IncorectArgs_ExpectException()
  {
    Assert.Throws<ArgumentException>(() => new ResourceLocation<TestStoneType>("", "", "stone"));
    Assert.Throws<ArgumentException>(() => new ResourceLocation<TestStoneType>("dungeon", "apple", "stone"));
    Assert.Throws<ArgumentException>(() => new ResourceLocation<TestStoneType>("red", "blue/", ""));
    Assert.Throws<ArgumentException>(() => new ResourceLocation<TestStoneType>("awgen", null, "stone"));
  }
}
