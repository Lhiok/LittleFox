using UnityEngine;

public static class RaycastUtil {


    // 重载Raycast方法
    public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, int layerMask, float distance = 0.1f)
    {
        RaycastHit2D result = Physics2D.Raycast(origin, direction, distance, layerMask);
        Debug.DrawRay(origin, direction * distance, result? Color.red: Color.green);
        return result;
    }
}