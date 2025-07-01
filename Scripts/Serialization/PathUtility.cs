using Godot;
using System;
using System.IO;

namespace Rusty.ISA;

/// <summary>
/// An utility class that can convert resource, user and relative paths to global paths.
/// </summary>
public static class PathUtility
{
    /* Public methods. */
    /// <summary>
    /// Get a global file path from a resource, user or relative one.
    /// </summary>
    public static string GetPath(string path)
    {
        // Rooted paths.
        if (Path.IsPathRooted(path))
            return path;

        // Res or user.
        else if (path.StartsWith("res://") || path.StartsWith("user://"))
            return ProjectSettings.GlobalizePath(path);

        // Other relative paths.
        // In the editor, this returns a .godot sub-path.
        // Otherwise, it returns a base directory sub-path.
        else if (OS.HasFeature("editor"))
            return ProjectSettings.GlobalizePath("res://") + ".godot/" + path;
        else
            return AppDomain.CurrentDomain.BaseDirectory + path;
    }

    /// <summary>
    /// Check if a path starts with a '\' or a drive (such as "C:\").
    /// </summary>
    public static bool IsGlobalPath(string path)
    {
        return Path.IsPathRooted(path);
    }
}