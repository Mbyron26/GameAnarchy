using ColossalFramework.Importers;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class UIUtils {

        public static UITextureAtlas CreateTextureAtlas(string atlasName, string path, Dictionary<string, RectOffset> spriteParams, int maxSpriteSize = 1024) {
            var keys = spriteParams.Keys.ToArray();
            var value = spriteParams.Values.ToArray();
            Texture2D texture2D = new(maxSpriteSize, maxSpriteSize, TextureFormat.ARGB32, false);
            Texture2D[] textures = new Texture2D[spriteParams.Count];
            for (int i = 0; i < spriteParams.Count; i++) {
                textures[i] = LoadTextureFromAssembly(path + keys[i] + ".png");
            }
            Rect[] regions = texture2D.PackTextures(textures, 2, maxSpriteSize);
            UITextureAtlas uITextureAtlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            Material material = UnityEngine.Object.Instantiate(UIView.GetAView().defaultAtlas.material);
            material.mainTexture = texture2D;
            uITextureAtlas.material = material;
            uITextureAtlas.name = atlasName;
            for (int j = 0; j < spriteParams.Count; j++) {
                UITextureAtlas.SpriteInfo item = new() {
                    name = keys[j],
                    texture = textures[j],
                    region = regions[j],
                    border = value[j]
                };
                uITextureAtlas.AddSprite(item);
            }
            return uITextureAtlas;
        }

        public static Texture2D LoadTextureFromAssembly(string fileName) {
            try {
                Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName);
                byte[] array = new byte[s.Length];
                s.Read(array, 0, array.Length);
                return new Image(array).CreateTexture();
            }
            catch (Exception e) {
                ExternalLogger.Log($"Couldn't load texture from assembly, file name:{fileName}, detial:{e.Message}");
                return null;
            }
        }

        public static UITextureAtlas GetAtlas(string name) {
            var atlas = GetAtlas();
            if (atlas is not null) {
                foreach (var item in atlas) {
                    if (item.name == name) {
                        ExternalLogger.Log($"Obtained UITextureAtlas [{name}].");
                        return item;
                    }
                }
            }
            ExternalLogger.Log($"Couldn't find UITextureAtlas [{name}], use default atlas.");
            return UIView.GetAView().defaultAtlas;
        }

        public static IEnumerable<UITextureAtlas> GetAtlas() {
            UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            for (int i = 0; i < atlases.Length; i++) {
                yield return atlases[i];
            }
        }
    }
}
