using Godot;

namespace ISA;

/// <summary>
/// A class that can be used to check if a global type exists.
/// </summary>
internal static class GlobalTypeChecker
{
    /* Private constants. */
    private static string Code =
        "extends Node;"
        + "\n"
        + "\nstatic func check(type_name : String) -> bool:"
        + "\n\treturn type_exists(type_name);";

    /* Private properties. */
    private static GDScript GDScript { get; set; } = null;
    private static Node Instance { get; set; }

    /* Public methods. */
    /// <summary>
    /// Check if a global class exists.
    /// </summary>
    public static bool Check(string typeName)
    {
        if (Instance == null)
            GenerateInstance();

        return (bool)Instance.Call("check", typeName);
    }

    /* Private methods. */
    private static void GenerateInstance()
    {
        // Create script.
        GDScript = new()
        {
            SourceCode = Code,
        };
        GDScript.Reload();

        // Create node.
        Instance = (Node)GDScript.New();
    }
}