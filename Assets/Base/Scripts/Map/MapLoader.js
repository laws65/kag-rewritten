var MapLoader = {
    Load: function(path) {
        var keyArray = Utils.FromJson("Scripts/Map/MapKeys.json");
        var keyUsed = [];

        var mapSprite = Utils.GetSprite(path);
        var colorArray = mapSprite.GetPixels();

        Map.SetCellSize(8, 8);
        for (var i = 0; i < colorArray.length; ++i) {
            var color = colorArray[i];

            for (var j = 0; j < keyArray.length; ++j) {
                if (color.ToHtmlRGB() == keyArray[j].Color) {
                    if (!keyUsed.includes(color.ToHtmlRGB())) {

                        var tile = new Tile();
                        tile.SetSprite(Utils.GetSprite(keyArray[j].Sprite));

                        keyUsed[color.ToHtmlRGB] = tile;
                    }
                    
                    Map.SetTile(i % mapSprite.Width, i / mapSprite.Width, tile);
                }
            }
        }
    }
};
