namespace Rusty.ISA;

/// <summary>
/// A descriptor for a instruction rule.
/// </summary>
[ResourceDescriptor(typeof(InstructionRule), "instr")]
public sealed class InstructionRuleDescriptor : CompileRuleDescriptor
{
    /* Public properties. */
    [XmlProperty("opcode")] public string Opcode { get; set; } = "";

    /* Public methods. */
    public override InstructionRule GenerateObject()
    {
        return new InstructionRule(ID, DisplayName, Description, Opcode, Preview);
    }
}