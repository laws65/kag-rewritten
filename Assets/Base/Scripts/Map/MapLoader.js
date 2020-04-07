var MapLoader = {
    tileArray: [],

    Load(path) {
        this.LoadTiles()

        // Fill the map with respective tiles
        var sprite = Utils.GetSprite(path)
        var colorArray = sprite.GetPixels()

        Map.SetTileSize(8, 8);
        for (var x = 0; x < sprite.Width; ++x)
        {
            for (var y = 0; y < sprite.Height; ++y)
            {
                var color = colorArray[x + sprite.Width * y].ToHtmlRGB()
                
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
        var config = Utils.FromJson(path)

        var tile = new Tile()
        tile.SetSize(config.Sprite.Width, config.Sprite.Height)
        tile.SetSprite(Utils.GetSprite(config.Sprite.File))
        tile.SetFrame(config.Sprite.Frame)

        if (config.Sprite.Vertices) {
            let vertices = []
            for (var i = 0; i < config.Sprite.Vertices.length; ++i) {
                vertices.push(new Vector(config.Sprite.Vertices[i].x, config.Sprite.Vertices[i].y, 0))
            }
            tile.SetGeometry(vertices)
        }

        this.tileArray[config.Color] = tile
    }
};
