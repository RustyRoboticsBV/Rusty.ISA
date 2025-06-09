using Godot;
using Godot.Collections;

namespace Rusty.ISA;

/// <summary>
/// A register that can be accessed by instruction execution handlers, and can be used to store data.
/// All execution handlers can access all registers.
/// </summary>
[GlobalClass]
public sealed partial class Register : Resource
{
    /* Public properties. */
    /// <summary>
    /// The values in the register. Don't use this directly unless you absolutely have to.
    /// </summary>
    [Export] public Array<Variant> Values { get; private set; } = new();

    /// <summary>
    /// The number of values in the register.
    /// </summary>
    public int Count => Values.Count;

    /* Public methods. */
    /// <summary>
    /// Add a new value to the register.
    /// </summary>
    public void Push(Variant value)
    {
        Values.Add(value);
    }

    /// <summary>
    /// Remove the newest value in the register and return it.
    /// </summary>
    public Variant Pop()
    {
        Variant newest = Top();
        Values.RemoveAt(Values.Count - 1);
        return newest;
    }

    /// <summary>
    /// Remove the oldest value in the register and return it.
    /// </summary>
    public Variant Dequeue()
    {
        Variant oldest = Back();
        Values.RemoveAt(0);
        return oldest;
    }

    /// <summary>
    /// Return the newest value on the register, without removing it.
    /// </summary>
    public Variant Top()
    {
        return Values[^1];
    }

    /// <summary>
    /// Return the oldest value on the register, without removing it.
    /// </summary>
    public Variant Back()
    {
        return Values[0];
    }
}