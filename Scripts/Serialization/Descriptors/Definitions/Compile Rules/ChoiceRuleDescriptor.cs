using System.Collections.Generic;

namespace Rusty.ISA;

/// <summary>
/// A descriptor for a choice rule.
/// </summary>
[ResourceDescriptor(typeof(ChoiceRule), "choice")]
public sealed class ChoiceRuleDescriptor : CompileRuleDescriptor
{
    /* Public properties. */
    [XmlProperty("types")] public List<CompileRuleDescriptor> Types { get; } = new();
    [XmlProperty("selected")] public int DefaultSelected { get; set; }

    /* Public methods. */
    public override ChoiceRule GenerateObject()
    {
        List<CompileRule> types = new();
        foreach (CompileRuleDescriptor choice in Types)
        {
            types.Add(choice.GenerateObject());
        }
        return new ChoiceRule(ID, DisplayName, Description, types.ToArray(), DefaultSelected, Preview);
    }
}