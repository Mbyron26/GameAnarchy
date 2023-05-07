namespace MbyronModsCommon.UI;
using ColossalFramework;
using ColossalFramework.UI;
using System;
using UnityEngine;

public class UISlicedSpriteRender : UISpriteRender {
    public new static void RenderSprite(UIRenderData renderData, RenderOptions options) {
        options.baseIndex = renderData.vertices.Count;
        RebuildTriangles(renderData, options);
        RebuildVertices(renderData, options);
        RebuildUV(renderData, options);
        RebuildColors(renderData, options);
        if (options.fillAmount < 1f) {
            DoFill(renderData, options);
        }
    }
    private static void RebuildTriangles(UIRenderData renderData, RenderOptions options) {
        int baseIndex = options.baseIndex;
        PoolList<int> triangles = renderData.triangles;
        for (int i = 0; i < kSlicedTriangleIndices.Length; i++) {
            triangles.Add(baseIndex + kSlicedTriangleIndices[i]);
        }
    }
    private static void RebuildVertices(UIRenderData renderData, RenderOptions options) {
        float x = 0f;
        float y = 0f;
        float num = Mathf.Ceil(options.size.x);
        float num2 = Mathf.Ceil(-options.size.y);
        UITextureAtlas.SpriteInfo spriteInfo = options.spriteInfo;
        float num3 = (float)spriteInfo.border.left;
        float num4 = (float)spriteInfo.border.top;
        float num5 = (float)spriteInfo.border.right;
        float num6 = (float)spriteInfo.border.bottom;
        if (options.flip.IsFlagSet(UISpriteFlip.FlipHorizontal)) {
            float num7 = num5;
            num5 = num3;
            num3 = num7;
        }
        if (options.flip.IsFlagSet(UISpriteFlip.FlipVertical)) {
            float num8 = num6;
            num6 = num4;
            num4 = num8;
        }
        m_Vertices[0] = new Vector3(x, y, 0f) + options.offset;
        m_Vertices[1] = m_Vertices[0] + new Vector3(num3, 0f, 0f);
        m_Vertices[2] = m_Vertices[0] + new Vector3(num3, -num4, 0f);
        m_Vertices[3] = m_Vertices[0] + new Vector3(0f, -num4, 0f);
        m_Vertices[4] = new Vector3(num - num5, y, 0f) + options.offset;
        m_Vertices[5] = m_Vertices[4] + new Vector3(num5, 0f, 0f);
        m_Vertices[6] = m_Vertices[4] + new Vector3(num5, -num4, 0f);
        m_Vertices[7] = m_Vertices[4] + new Vector3(0f, -num4, 0f);
        m_Vertices[8] = new Vector3(x, num2 + num6, 0f) + options.offset;
        m_Vertices[9] = m_Vertices[8] + new Vector3(num3, 0f, 0f);
        m_Vertices[10] = m_Vertices[8] + new Vector3(num3, -num6, 0f);
        m_Vertices[11] = m_Vertices[8] + new Vector3(0f, -num6, 0f);
        m_Vertices[12] = new Vector3(num - num5, num2 + num6, 0f) + options.offset;
        m_Vertices[13] = m_Vertices[12] + new Vector3(num5, 0f, 0f);
        m_Vertices[14] = m_Vertices[12] + new Vector3(num5, -num6, 0f);
        m_Vertices[15] = m_Vertices[12] + new Vector3(0f, -num6, 0f);
        for (int i = 0; i < m_Vertices.Length; i++) {
            renderData.vertices.Add((m_Vertices[i] * options.pixelsToUnits).Quantize(options.pixelsToUnits));
        }
    }
    private static void RebuildUV(UIRenderData renderData, RenderOptions options) {
        UITextureAtlas atlas = options.atlas;
        Vector2 vector = new((float)atlas.texture.width, (float)atlas.texture.height);
        UITextureAtlas.SpriteInfo spriteInfo = options.spriteInfo;
        float num = (float)spriteInfo.border.top / vector.y;
        float num2 = (float)spriteInfo.border.bottom / vector.y;
        float num3 = (float)spriteInfo.border.left / vector.x;
        float num4 = (float)spriteInfo.border.right / vector.x;
        Rect region = spriteInfo.region;
        m_UVs[0] = new Vector2(region.x, region.yMax);
        m_UVs[1] = new Vector2(region.x + num3, region.yMax);
        m_UVs[2] = new Vector2(region.x + num3, region.yMax - num);
        m_UVs[3] = new Vector2(region.x, region.yMax - num);
        m_UVs[4] = new Vector2(region.xMax - num4, region.yMax);
        m_UVs[5] = new Vector2(region.xMax, region.yMax);
        m_UVs[6] = new Vector2(region.xMax, region.yMax - num);
        m_UVs[7] = new Vector2(region.xMax - num4, region.yMax - num);
        m_UVs[8] = new Vector2(region.x, region.y + num2);
        m_UVs[9] = new Vector2(region.x + num3, region.y + num2);
        m_UVs[10] = new Vector2(region.x + num3, region.y);
        m_UVs[11] = new Vector2(region.x, region.y);
        m_UVs[12] = new Vector2(region.xMax - num4, region.y + num2);
        m_UVs[13] = new Vector2(region.xMax, region.y + num2);
        m_UVs[14] = new Vector2(region.xMax, region.y);
        m_UVs[15] = new Vector2(region.xMax - num4, region.y);
        if (options.flip != UISpriteFlip.None) {
            for (int i = 0; i < m_UVs.Length; i += 4) {
                Vector2 vector2;
                if (options.flip.IsFlagSet(UISpriteFlip.FlipHorizontal)) {
                    vector2 = m_UVs[i];
                    m_UVs[i] = m_UVs[i + 1];
                    m_UVs[i + 1] = vector2;
                    vector2 = m_UVs[i + 2];
                    m_UVs[i + 2] = m_UVs[i + 3];
                    m_UVs[i + 3] = vector2;
                }
                if (options.flip.IsFlagSet(UISpriteFlip.FlipVertical)) {
                    vector2 = m_UVs[i];
                    m_UVs[i] = m_UVs[i + 3];
                    m_UVs[i + 3] = vector2;
                    vector2 = m_UVs[i + 1];
                    m_UVs[i + 1] = m_UVs[i + 2];
                    m_UVs[i + 2] = vector2;
                }
            }
            if (options.flip.IsFlagSet(UISpriteFlip.FlipHorizontal)) {
                Vector2[] array = new Vector2[m_UVs.Length];
                Array.Copy(m_UVs, array, m_UVs.Length);
                Array.Copy(m_UVs, 0, m_UVs, 4, 4);
                Array.Copy(array, 4, m_UVs, 0, 4);
                Array.Copy(m_UVs, 8, m_UVs, 12, 4);
                Array.Copy(array, 12, m_UVs, 8, 4);
            }
            if (options.flip.IsFlagSet(UISpriteFlip.FlipVertical)) {
                Vector2[] array2 = new Vector2[m_UVs.Length];
                Array.Copy(m_UVs, array2, m_UVs.Length);
                Array.Copy(m_UVs, 0, m_UVs, 8, 4);
                Array.Copy(array2, 8, m_UVs, 0, 4);
                Array.Copy(m_UVs, 4, m_UVs, 12, 4);
                Array.Copy(array2, 12, m_UVs, 4, 4);
            }
        }
        for (int j = 0; j < m_UVs.Length; j++) {
            renderData.uvs.Add(m_UVs[j]);
        }
    }
    private static void RebuildColors(UIRenderData renderData, RenderOptions options) {
        Color32 item = ((Color)options.color).linear;
        for (int i = 0; i < 16; i++) {
            renderData.colors.Add(item);
        }
    }
    private static int[][] GetFillIndices(UIFillDirection fillDirection, int baseIndex) {
        int[][] array = (fillDirection == UIFillDirection.Horizontal) ? kHorzFill : kVertFill;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                kFillIndices[i][j] = baseIndex + array[i][j];
            }
        }
        return kFillIndices;
    }
    private static void DoFill(UIRenderData renderData, RenderOptions options) {
        int baseIndex = options.baseIndex;
        PoolList<Vector3> vertices = renderData.vertices;
        PoolList<Vector2> uvs = renderData.uvs;
        int[][] fillIndices = GetFillIndices(options.fillDirection, baseIndex);
        bool invertFill = options.invertFill;
        if (options.invertFill) {
            for (int i = 0; i < fillIndices.Length; i++) {
                Array.Reverse(fillIndices[i]);
            }
        }
        int index = (options.fillDirection == UIFillDirection.Horizontal) ? 0 : 1;
        float num = vertices[fillIndices[0][invertFill ? 3 : 0]][index];
        float num2 = vertices[fillIndices[0][invertFill ? 0 : 3]][index];
        float num3 = Mathf.Abs(num2 - num);
        float num4 = (!invertFill) ? (num + options.fillAmount * num3) : (num2 - options.fillAmount * num3);
        for (int j = 0; j < fillIndices.Length; j++) {
            if (!invertFill) {
                for (int k = 3; k > 0; k--) {
                    float num5 = vertices[fillIndices[j][k]][index];
                    if (num5 >= num4) {
                        Vector3 value = vertices[fillIndices[j][k]];
                        value[index] = num4;
                        vertices[fillIndices[j][k]] = value;
                        float num6 = vertices[fillIndices[j][k - 1]][index];
                        if (num6 <= num4) {
                            float num7 = num5 - num6;
                            float t = (num4 - num6) / num7;
                            float b = uvs[fillIndices[j][k]][index];
                            float a = uvs[fillIndices[j][k - 1]][index];
                            Vector2 value2 = uvs[fillIndices[j][k]];
                            value2[index] = Mathf.Lerp(a, b, t);
                            uvs[fillIndices[j][k]] = value2;
                        }
                    }
                }
            } else {
                for (int l = 1; l < 4; l++) {
                    float num8 = vertices[fillIndices[j][l]][index];
                    if (num8 <= num4) {
                        Vector3 value3 = vertices[fillIndices[j][l]];
                        value3[index] = num4;
                        vertices[fillIndices[j][l]] = value3;
                        float num9 = vertices[fillIndices[j][l - 1]][index];
                        if (num9 >= num4) {
                            float num10 = num8 - num9;
                            float t2 = (num4 - num9) / num10;
                            float b2 = uvs[fillIndices[j][l]][index];
                            float a2 = uvs[fillIndices[j][l - 1]][index];
                            Vector2 value4 = uvs[fillIndices[j][l]];
                            value4[index] = Mathf.Lerp(a2, b2, t2);
                            uvs[fillIndices[j][l]] = value4;
                        }
                    }
                }
            }
        }
    }

    static UISlicedSpriteRender() {
        int[][] array = new int[4][];
        int[][] array2 = array;
        int num = 0;
        int[] array3 = new int[4];
        array2[num] = array3;
        int[][] array4 = array;
        int num2 = 1;
        int[] array5 = new int[4];
        array4[num2] = array5;
        int[][] array6 = array;
        int num3 = 2;
        int[] array7 = new int[4];
        array6[num3] = array7;
        int[][] array8 = array;
        int num4 = 3;
        int[] array9 = new int[4];
        array8[num4] = array9;
        kFillIndices = array;
        m_Vertices = new Vector3[16];
        m_UVs = new Vector2[16];
    }

    private static readonly int[] kSlicedTriangleIndices = new int[] {
                0,
                1,
                2,
                2,
                3,
            0,
            4,
            5,
            6,
            6,
            7,
            4,
            8,
            9,
            10,
            10,
            11,
            8,
            12,
            13,
            14,
            14,
            15,
            12,
            1,
            4,
            7,
            7,
            2,
            1,
            9,
            12,
            15,
            15,
            10,
            9,
            3,
            2,
            9,
            9,
            8,
            3,
            7,
            6,
            13,
            13,
            12,
            7,
            2,
            7,
            12,
            12,
            9,
            2
            };
    private static readonly int[][] kHorzFill = new int[][] {
            new int[] { 0, 1, 4, 5 },
            new int[] { 3, 2, 7, 6 },
            new int[] { 8, 9, 12,  13 },
            new int[] { 11, 10, 15, 14 }
            };
    private static readonly int[][] kVertFill = new int[][] {
            new int[] { 11, 8, 3, 0 },
            new int[] { 10, 9, 2, 1 },
            new int[] { 15,12, 7, 4 },
            new int[] { 14, 13, 6, 5 }
            };
    private static readonly int[][] kFillIndices;
    private static Vector3[] m_Vertices;
    private static Vector2[] m_UVs;
}
public class UISpriteRender {
    public static readonly int[] kTriangleIndices = new int[] { 0, 1, 3, 3, 1, 2 };
    public static void RenderSprite(UIRenderData data, RenderOptions options) {
        options.baseIndex = data.vertices.Count;
        RebuildTriangles(data, options);
        RebuildVertices(data, options);
        RebuildUV(data, options);
        RebuildColors(data, options);
        if (options.fillAmount < 1f) {
            DoFill(data, options);
        }
    }
    private static void RebuildTriangles(UIRenderData renderData, RenderOptions options) {
        int baseIndex = options.baseIndex;
        PoolList<int> triangles = renderData.triangles;
        triangles.EnsureCapacity(triangles.Count + kTriangleIndices.Length);
        for (int i = 0; i < kTriangleIndices.Length; i++) {
            triangles.Add(baseIndex + kTriangleIndices[i]);
        }
    }
    private static void RebuildVertices(UIRenderData renderData, RenderOptions options) {
        PoolList<Vector3> vertices = renderData.vertices;
        int baseIndex = options.baseIndex;
        float x = 0f;
        float y = 0f;
        float x2 = Mathf.Ceil(options.size.x);
        float y2 = Mathf.Ceil(-options.size.y);
        vertices.Add(new Vector3(x, y, 0f) * options.pixelsToUnits);
        vertices.Add(new Vector3(x2, y, 0f) * options.pixelsToUnits);
        vertices.Add(new Vector3(x2, y2, 0f) * options.pixelsToUnits);
        vertices.Add(new Vector3(x, y2, 0f) * options.pixelsToUnits);
        Vector3 b = options.offset.RoundToInt() * options.pixelsToUnits;
        for (int i = 0; i < 4; i++) {
            vertices[baseIndex + i] = (vertices[baseIndex + i] + b).Quantize(options.pixelsToUnits);
        }
    }
    private static void RebuildColors(UIRenderData renderData, RenderOptions options) {
        Color32 item = ((Color)options.color).linear;

        PoolList<Color32> colors = renderData.colors;
        for (int i = 0; i < 4; i++) {
            colors.Add(item);
        }
    }
    private static void RebuildUV(UIRenderData renderData, RenderOptions options) {
        Rect region = options.spriteInfo.region;
        PoolList<Vector2> uvs = renderData.uvs;
        uvs.Add(new Vector2(region.x, region.yMax));
        uvs.Add(new Vector2(region.xMax, region.yMax));
        uvs.Add(new Vector2(region.xMax, region.y));
        uvs.Add(new Vector2(region.x, region.y));
        Vector2 value = Vector2.zero;
        if (options.flip.IsFlagSet(UISpriteFlip.FlipHorizontal)) {
            value = uvs[1];
            uvs[1] = uvs[0];
            uvs[0] = value;
            value = uvs[3];
            uvs[3] = uvs[2];
            uvs[2] = value;
        }
        if (options.flip.IsFlagSet(UISpriteFlip.FlipVertical)) {
            value = uvs[0];
            uvs[0] = uvs[3];
            uvs[3] = value;
            value = uvs[1];
            uvs[1] = uvs[2];
            uvs[2] = value;
        }
    }
    private static void DoFill(UIRenderData renderData, RenderOptions options) {
        int baseIndex = options.baseIndex;
        PoolList<Vector3> vertices = renderData.vertices;
        PoolList<Vector2> uvs = renderData.uvs;
        int index = baseIndex + 3;
        int index2 = baseIndex + 2;
        int index3 = baseIndex;
        int index4 = baseIndex + 1;
        if (options.invertFill) {
            if (options.fillDirection == UIFillDirection.Horizontal) {
                index = baseIndex + 1;
                index2 = baseIndex;
                index3 = baseIndex + 2;
                index4 = baseIndex + 3;
            } else {
                index = baseIndex;
                index2 = baseIndex + 1;
                index3 = baseIndex + 3;
                index4 = baseIndex + 2;
            }
        }
        if (options.fillDirection == UIFillDirection.Horizontal) {
            vertices[index2] = Vector3.Lerp(vertices[index2], vertices[index], 1f - options.fillAmount);
            vertices[index4] = Vector3.Lerp(vertices[index4], vertices[index3], 1f - options.fillAmount);
            uvs[index2] = Vector2.Lerp(uvs[index2], uvs[index], 1f - options.fillAmount);
            uvs[index4] = Vector2.Lerp(uvs[index4], uvs[index3], 1f - options.fillAmount);
            return;
        }
        vertices[index3] = Vector3.Lerp(vertices[index3], vertices[index], 1f - options.fillAmount);
        vertices[index4] = Vector3.Lerp(vertices[index4], vertices[index2], 1f - options.fillAmount);
        uvs[index3] = Vector2.Lerp(uvs[index3], uvs[index], 1f - options.fillAmount);
        uvs[index4] = Vector2.Lerp(uvs[index4], uvs[index2], 1f - options.fillAmount);
    }


}

public struct RenderOptions {
    private UITextureAtlas m_Atlas;
    private UITextureAtlas.SpriteInfo m_SpriteInfo;
    private Color32 m_Color;
    private float m_PixelsToUnits;
    private Vector2 m_Size;
    private UISpriteFlip m_Flip;
    private bool m_InvertFill;
    private UIFillDirection m_FillDirection;
    private float m_FillAmount;
    private Vector3 m_Offset;
    private int m_BaseIndex;

    public UITextureAtlas atlas {
        get {
            return m_Atlas;
        }
        set {
            m_Atlas = value;
        }
    }

    public UITextureAtlas.SpriteInfo spriteInfo {
        get {
            return m_SpriteInfo;
        }
        set {
            m_SpriteInfo = value;
        }
    }

    public Color32 color {
        get {
            return m_Color;
        }
        set {
            m_Color = value;
        }
    }

    public float pixelsToUnits {
        get {
            return m_PixelsToUnits;
        }
        set {
            m_PixelsToUnits = value;
        }
    }

    public Vector2 size {
        get {
            return m_Size;
        }
        set {
            m_Size = value;
        }
    }

    public UISpriteFlip flip {
        get {
            return m_Flip;
        }
        set {
            m_Flip = value;
        }
    }

    public bool invertFill {
        get {
            return m_InvertFill;
        }
        set {
            m_InvertFill = value;
        }
    }

    public UIFillDirection fillDirection {
        get {
            return m_FillDirection;
        }
        set {
            m_FillDirection = value;
        }
    }

    public float fillAmount {
        get {
            return m_FillAmount;
        }
        set {
            m_FillAmount = value;
        }
    }

    public Vector3 offset {
        get {
            return m_Offset;
        }
        set {
            m_Offset = value;
        }
    }

    public int baseIndex {
        get {
            return m_BaseIndex;
        }
        set {
            m_BaseIndex = value;
        }
    }
}



