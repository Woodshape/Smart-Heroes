using UnityEngine;

namespace GOAP {
    public sealed class GWorld {
        public static GWorld Instance { get; } = new GWorld();
        private static WorldStates world;

        static GWorld() {
            world = new WorldStates();

            Debug.Log("GOAP -> Created world: " + world);
        }

        // private GWorld() { }

        public WorldStates GetWorld() {
            return world;
        }
    }
}
