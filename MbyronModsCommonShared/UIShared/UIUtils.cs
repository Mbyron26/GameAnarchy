using ColossalFramework.UI;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MbyronModsCommon {
    public class UIUtils {
        public static UITextureAtlas CreateTextureAtlas(string atlasName, string path, string[] spriteNames, int maxSpriteSize) {
            Texture2D texture2D = new Texture2D(maxSpriteSize, maxSpriteSize, TextureFormat.ARGB32, false);
            Texture2D[] textures = new Texture2D[spriteNames.Length];
            for (int i = 0; i < spriteNames.Length; i++) {
                textures[i] = LoadTextureFromAssembly(path + spriteNames[i] + ".png");
            }
            Rect[] regions = texture2D.PackTextures(textures, 2, maxSpriteSize);
            UITextureAtlas uITextureAtlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            Material material = Object.Instantiate(UIView.GetAView().defaultAtlas.material);
            material.mainTexture = texture2D;
            uITextureAtlas.material = material;
            uITextureAtlas.name = atlasName;
            for (int j = 0; j < spriteNames.Length; j++) {
                UITextureAtlas.SpriteInfo item = new UITextureAtlas.SpriteInfo() {
                    name = spriteNames[j],
                    texture = textures[j],
                    region = regions[j]
                };
                uITextureAtlas.AddSprite(item);
            }
            return uITextureAtlas;
        }

        public static Texture2D LoadTextureFromAssembly(string filename) {
            Texture2D texture = null;
            try {
                Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename);
                byte[] array = new byte[s.Length];
                s.Read(array, 0, array.Length);
                texture = new Texture2D(2, 2);
                texture.LoadImage(array);
            }
            catch {
                Debug.Log($"Error loading: {filename}");
                throw;
            }
            return texture;
        }

        public static UITextureAtlas GetAtlas(string name) {
            UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            for (int i = 0; i < atlases.Length; i++) {
                if (atlases[i].name == name)
                    return atlases[i];
            }
            return UIView.GetAView().defaultAtlas;
        }
    }
}
