using UnityModManagerNet;
using UnityEngine;

namespace CUBE
{
    public class Main
    {
        public static UnityModManager.ModEntry myModEntry;
        public static string ModEntryPath;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            myModEntry = modEntry;
            ModEntryPath = myModEntry.Path;

            WorldStreamingInit.LoadingFinished += CubeInit;

            return true;
        }

        private static void CubeInit()
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(0, 130, 0);
            cube.transform.localScale = new Vector3(10, 10, 10);
        }
    }
}
