using Godot;

namespace Rusty.ISA
{
    /// <summary>
    /// A wrapper around a generate execution handler GDScipt node.
    /// </summary>
    internal sealed class ExecutionHandler
    {
        /* Private properties. */
        private Node Node { get; set; }

        /* Constructors. */
        internal ExecutionHandler(GDScript script)
        {
            Node = (Node)script.New();
        }

        /* Public methods. */
        /// <summary>
        /// Call the handler's initialize method.
        /// </summary>
        public void Initialize(Process player)
        {
            Node.Call("_initialize", player);
        }

        /// <summary>
        /// Call the handler's execute method.
        /// </summary>
        public void Execute(string[] arguments, double deltaTime)
        {
            Node.Call("_execute", arguments, deltaTime);
        }
    }
}