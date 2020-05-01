"use strict";

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var MapLoader = /*#__PURE__*/function () {
  function MapLoader() {
    _classCallCheck(this, MapLoader);

    this.tileArray = [];
  }

  _createClass(MapLoader, [{
    key: "Load",
    value: function Load(path) {
      this.LoadTiles(); // Fill the map with respective tiles

      var texture = Texture.FromFile(path);
      var colorArray = texture.GetPixels();
      Map.SetTileSize(8, 8);

      for (var x = 0; x < texture.size.x; ++x) {
        for (var y = 0; y < texture.size.y; ++y) {
          var color = colorArray[x + texture.size.x * y].ToHtmlRGB();

          if (this.tileArray[color] !== undefined) {
            Map.SetTile(x, y, this.tileArray[color]);
          }
        }
      }
    }
  }, {
    key: "LoadTiles",
    value: function LoadTiles() {
      // First get the list of tiles
      var pathArray = Engine.FromJson("Maps/MapKeys.json");

      for (var i = 0; i < pathArray.length; ++i) {
        this.AddTile(pathArray[i]);
      }
    }
  }, {
    key: "AddTile",
    value: function AddTile(path) {
      var tile = Tile.FromFile(path);
      this.tileArray[tile.color] = tile;
    }
  }]);

  return MapLoader;
}();