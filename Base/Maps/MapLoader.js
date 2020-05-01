class MapLoader {
    constructor() {
        this.tileArray = []
    }

    Load(path) {
        this.LoadTiles()

        // Fill the map with respective tiles
        let texture = Texture.FromFile(path)
        let colorArray = texture.GetPixels()

        Map.SetTileSize(8, 8)
        for (let x = 0; x < texture.size.x; ++x)
        {
            for (let y = 0; y < texture.size.y; ++y)
            {
                let color = colorArray[x + texture.size.x * y].ToHtmlRGB()
                
                if (this.tileArray[color] !== undefined) {
                    Map.SetTile(x, y, this.tileArray[color])
                }
            }
        }
    }

    LoadTiles() {
        // First get the list of tiles
        let pathArray = Engine.FromJson("Maps/MapKeys.json")

        for (let i = 0; i < pathArray.length; ++i) {
            this.AddTile(pathArray[i])
        }
    }

    AddTile(path) {
        let tile = Tile.FromFile(path)

        this.tileArray[tile.color] = tile
    }
}
