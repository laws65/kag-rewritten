var MapLoader = {
    Load: function(path) {
        // First get the list of tiles
        var tileArray = [];
        var keyArray = Utils.FromJson("Scripts/Map/MapKeys.json");

        for (var i = 0; i < keyArray.length; ++i) {
            var key = Utils.FromJson(keyArray[i]);
            keyArray[i] = key;

            var tile = new Tile();
            tile.SetSize(key.Sprite.Width, key.Sprite.Height);
            tile.SetSprite(Utils.GetSprite(key.Sprite.File));
            tile.SetFrame(key.Sprite.Frame)

            tileArray[key.Color] = tile;
        }

        // Fill the map with respective tiles
        var mapSprite = Utils.GetSprite(path);
        var colorArray = mapSprite.GetPixels();

        Map.SetTileSize(8, 8);
        for (var x = 0; x < mapSprite.Width; ++x)
        {
            for (var y = 0; y < mapSprite.Height; ++y)
            {
                var color = colorArray[x + mapSprite.Width * y].ToHtmlRGB();
                
                if (tileArray[color] !== undefined) {
                    Map.SetTile(x, y, tileArray[color]);
                }
            }
        }
    }
};
