var MapLoader = {
    tileArray: [],

    Load(path) {
        this.LoadTiles()

        // Fill the map with respective tiles
        var texture = new Texture(path)
        var colorArray = texture.GetPixels()

        Map.SetTileSize(8, 8);
        for (var x = 0; x < texture.width; ++x)
        {
            for (var y = 0; y < texture.height; ++y)
            {
                var color = colorArray[x + texture.width * y].ToHtmlRGB()
                
                if (this.tileArray[color] !== undefined) {
                    Map.SetTile(x, y, this.tileArray[color])
                }
            }
        }
    },

    LoadTiles() {
        // First get the list of tiles
        var pathArray = Utils.FromJson("Scripts/Map/MapKeys.json")

        for (var i = 0; i < pathArray.length; ++i) {
            this.AddTile(pathArray[i])
        }
    },

    AddTile(path) {
        var tile = new Tile(path);

        this.tileArray[tile.color] = tile;
    }
};
