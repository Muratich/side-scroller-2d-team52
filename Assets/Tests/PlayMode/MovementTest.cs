using NUnit.Framework;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
   private GameObject go;
    private Movement movement;

    [SetUp]
    public void SetUp() {
        go = new GameObject("Mover");
        movement = go.AddComponent<Movement>();
        movement.rb = go.AddComponent<Rigidbody2D>();
        var input = go.AddComponent<StubInputHandler>();
        movement.input = input;
        movement.control = true;
    }

    [Test]
    public void Control_False_PreventsMovement() {
        movement.control = false;
        movement.rb.velocity = new Vector2(5, 0);
        movement.FixedUpdate();
        Assert.AreEqual(Vector2.zero, movement.rb.velocity);
    }
}
