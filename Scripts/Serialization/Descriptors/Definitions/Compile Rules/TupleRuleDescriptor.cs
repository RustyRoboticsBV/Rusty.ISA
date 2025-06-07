using System.Collections.Generic;

namespace Rusty.ISA;

/// <summary>
/// A descriptor for a tuple rule.
/// </summary>
[ResourceDescriptor(typeof(TupleRule), "tuple")]
public sealed class TupleRuleDescriptor : CompileRuleDescriptor
{
    /* Public properties. */
    [XmlProperty("types")] public List<CompileRuleDescriptor> Types { get; } = new();

    /* Public methods. */
    public override TupleRule GenerateObject()
    {
        List<CompileRule> types = new();
        foreach (CompileRuleDescriptor type in Types)
        {
            types.Add(type.GenerateObject());
        }
        return new TupleRule(ID, DisplayName, Description, types.ToArray(), Preview);
    }
}