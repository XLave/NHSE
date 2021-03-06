﻿using System;
using System.Drawing;
using NHSE.Core;
using NHSE.Sprites;

namespace NHSE.WinForms
{
    public sealed class MapViewer : MapView, IDisposable
    {
        // Cached acre view objects to remove allocation/GC
        private readonly int[] PixelsItemAcre1;
        private readonly int[] PixelsItemAcreX;
        private readonly Bitmap ScaleAcre;
        private readonly int[] PixelsItemMap;
        private readonly Bitmap MapReticle;

        private readonly int[] PixelsBackgroundAcre1;
        private readonly int[] PixelsBackgroundAcreX;
        private readonly Bitmap BackgroundAcre;
        private readonly int[] PixelsBackgroundMap1;
        private readonly int[] PixelsBackgroundMapX;
        private readonly Bitmap BackgroundMap;

        public MapViewer(MapManager m) : base(m)
        {
            var l1 = m.Items.Layer1;
            PixelsItemAcre1 = new int[l1.GridWidth * l1.GridHeight];
            PixelsItemAcreX = new int[PixelsItemAcre1.Length * AcreScale * AcreScale];
            ScaleAcre = new Bitmap(l1.GridWidth * AcreScale, l1.GridHeight * AcreScale);

            PixelsItemMap = new int[l1.MapWidth * l1.MapHeight * MapScale * MapScale];
            MapReticle = new Bitmap(l1.MapWidth * MapScale, l1.MapHeight * MapScale);

            PixelsBackgroundAcre1 = new int[16 * 16];
            PixelsBackgroundAcreX = new int[PixelsItemAcreX.Length];
            BackgroundAcre = new Bitmap(ScaleAcre.Width, ScaleAcre.Height);

            PixelsBackgroundMap1 = new int[PixelsItemMap.Length / 4];
            PixelsBackgroundMapX = new int[PixelsItemMap.Length];
            BackgroundMap = new Bitmap(MapReticle.Width, MapReticle.Height);
        }

        public void Dispose()
        {
            ScaleAcre.Dispose();
            MapReticle.Dispose();
            BackgroundAcre.Dispose();
            BackgroundMap.Dispose();
        }

        public Bitmap GetLayerAcre(int t) => GetLayerAcre(X, Y, t);
        public Bitmap GetMapWithReticle(int t) => GetMapWithReticle(X, Y, t, Map.CurrentLayer);

        public Bitmap GetBackgroundTerrain(int index = -1)
        {
            return TerrainSprite.GetMapWithBuildings(Map, null, PixelsBackgroundMap1, PixelsBackgroundMapX, BackgroundMap, 2, index);
        }

        private Bitmap GetLayerAcre(int topX, int topY, int t)
        {
            var layer = Map.CurrentLayer;
            return FieldItemSpriteDrawer.GetBitmapItemLayerAcre(layer, topX, topY, AcreScale, PixelsItemAcre1, PixelsItemAcreX, ScaleAcre, t);
        }

        public Bitmap GetBackgroundAcre(Font f, byte tbuild, byte tterrain, int index = -1)
        {
            return TerrainSprite.GetAcre(this, f, PixelsBackgroundAcre1, PixelsBackgroundAcreX, BackgroundAcre, index, tbuild, tterrain);
        }

        private Bitmap GetMapWithReticle(int topX, int topY, int t, FieldItemLayer layer)
        {
            return FieldItemSpriteDrawer.GetBitmapItemLayer(layer, topX, topY, PixelsItemMap, MapReticle, t);
        }
    }
}