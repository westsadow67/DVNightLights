using System.Reflection;
using UnityModManagerNet;
using Harmony12;
using UnityEngine;
using UnityEngine.Rendering;

namespace DVNightLights
{
    public class Main
    {
        public static UnityModManager.ModEntry myModEntry;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {

            myModEntry = modEntry;

            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            // Something
            return true; // If false the mod will show an error.
        }

        public static string GetPath()
        {
            string ModEntryPath = myModEntry.Path;
            return ModEntryPath;
        }
    }

    [HarmonyPatch(typeof(PlayerInstantiator), "Awake")]
    class StartUp
    {
        static void Postfix()
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.AddComponent<PlayerLight>();
        }
    }

    class PlayerLight : MonoBehaviour
    {
        static AssetBundle SpotLightBundle;
        static GameObject PlayerSpotLightPrefab;
        static GameObject PlayerSpotLight;
        Light PlayerSpot;
        AudioSource HalfLifeAudio;
        Transform PlayerCamTrans;

        //bool IsSpotOn = false;
        string modPath;

        void Start()
        {
            modPath = Main.GetPath();
            LoadPrefabs();

            PlayerCamTrans = PlayerManager.PlayerCamera.transform;
            //PlayerSpotLight.transform.SetParent(PlayerCamTrans, true);
        }

        void Update()
        {
            PlayerSpotLight.transform.rotation = PlayerCamTrans.rotation;
            PlayerSpotLight.transform.position = PlayerCamTrans.position;

            if (ReadPlayerInput())
            {
                ToggleSpotLight();
            }
        }

        void LoadPrefabs()
        {
            SpotLightBundle = AssetBundle.LoadFromFile(modPath + "Resources/playerspot");
            PlayerSpotLightPrefab = SpotLightBundle.LoadAsset<GameObject>("Assets/DVNightLights/PlayerSpot.prefab"); 
            Debug.Log("PlayerSpotLightPrefab: " + PlayerSpotLightPrefab);

            PlayerSpotLight = GameObject.Instantiate(PlayerSpotLightPrefab);
            PlayerSpot = PlayerSpotLight.GetComponent<Light>();
            HalfLifeAudio = PlayerSpotLight.GetComponent<AudioSource>();
        }

        bool ReadPlayerInput()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown("t"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void ToggleSpotLight()
        {
            PlayerSpot.enabled = !PlayerSpot.enabled;
            HalfLifeAudio.Play();
        }
    }
}
