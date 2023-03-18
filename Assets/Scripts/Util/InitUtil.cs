using UnityEngine;

namespace Util
{
    public static class InitUtil
    {
        public static GameObject InitGo(GameObject tar, Vector3 pos, float os = 0)
        {
            // var offset = Random.Range(-os, os);
            // var angle = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + offset);
            // return tar.Instantiate(tar, pos, angle);
            return tar;
        }
    }
}