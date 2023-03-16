using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using LitJson;

namespace Util
{
    public static class SerializeFileUtil
    {
        public static T ParseJsonFileTo<T>(string path, string errMsg = "解析失败")
        {
            T result;
            if (File.Exists(path))
            {
                var sr = new StreamReader(path);
                var jsonStr = sr.ReadToEnd();
                sr.Close();
                result = JsonMapper.ToObject<T>(jsonStr);
            }
            else
            {
                Debug.LogError($"{errMsg}, 找不到 PATH = \"{path}\"");
                return default;
            }

            if (result == null)
            {
                Debug.LogError($"{errMsg}");
                return default;
            }

            return result;
        }

        public static void ToJsonGenFile<T>(string path, T obj)
        {
            var saveJsonStr = JsonMapper.ToJson(obj);
            var sw = new StreamWriter(path);
            sw.Write(saveJsonStr);
            sw.Close();
        }
    }
}