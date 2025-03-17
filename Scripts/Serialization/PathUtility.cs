using Godot;
using System;

namespace Rusty.ISA
{
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
            // Res or user.
            if (path.StartsWith("res://") || path.StartsWith("user://"))
                return ProjectSettings.GlobalizePath(path);

            // Other paths.
            else if (OS.HasFeature("editor"))
                return ProjectSettings.GlobalizePath("res://") + ".godot/" + path;
            else
                return AppDomain.CurrentDomain.BaseDirectory + path;
        }
    }
}