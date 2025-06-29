using NUnit.Framework;

public class SimpleTests {
    [Test]
    public void AlwaysPasses() {
        Assert.AreEqual(2 + 2, 4);
    }
}
