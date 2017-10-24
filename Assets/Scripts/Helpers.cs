using UnityEngine;
using UnityEngine.Events;

public static class Helpers {

    //Masks
    public static readonly int DefaultLayerMask = 1 << LayerMask.NameToLayer("Default");
	public static readonly int ObstacleLayerMask = 1 << LayerMask.NameToLayer("Obstacle");

    //Raycast check functions
    public static bool Check(Vector3 dir, Vector3 position, int mask, out RaycastHit hit) {
        Ray ray = new Ray(position, dir);
        if (Physics.Raycast(ray, out hit, 1f, mask))
            return true;
        return false;
    }

    public static bool Check(Vector3 dir, Vector3 position, int mask) {
        RaycastHit hit;
        return Check(dir, position, mask, out hit);
    }

    public static bool Check(Vector3 dir, Vector3 position, int mask, string tag) {
        RaycastHit hit;
        if (Check(dir, position, mask, out hit)) {
            if (tag == "" || hit.collider.tag == tag) {
                return true;
            }
        }
        return false;
    }

	//TODO: Layer check methods for most used layers
//    public static bool CheckWall(Vector3 dir, Vector3 position) {
//        return Check(dir, position, WallLayerMask);
//    }

    /// <summary>
    /// Tests if an component has been destroyed. Works for Interfaces.
    /// </summary>
    public static bool IsNullOrDestroyed(this object value) {
        return ReferenceEquals(value, null) || value.Equals(null);
    }
}
