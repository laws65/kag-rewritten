"use strict";

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

Engine.Import("Maps/MapLoader.js");

var Rules = /*#__PURE__*/function () {
  function Rules() {
    _classCallCheck(this, Rules);

    this.name = "Generic game mode";
    this.description = "No description provided.";
  }

  _createClass(Rules, [{
    key: "Start",
    value: function Start() {}
  }, {
    key: "OnPlayerConnected",
    value: function OnPlayerConnected(player) {}
  }, {
    key: "OnPlayerDisconnected",
    value: function OnPlayerDisconnected(player) {}
  }]);

  return Rules;
}();

Engine.Export(Rules);