"use strict";

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var Main = /*#__PURE__*/function () {
  function Main() {
    _classCallCheck(this, Main);
  }

  _createClass(Main, [{
    key: "Start",
    value: function Start(rulesPath) {
      var rulesClass = Engine.Import(rulesPath);
      this.rules = Engine.FromClass(rulesClass);
      this.rules.Start();
    }
  }, {
    key: "OnPlayerConnected",
    value: function OnPlayerConnected(player) {
      this.rules.OnPlayerConnected(player);
    }
  }, {
    key: "OnPlayerDisconnected",
    value: function OnPlayerDisconnected(player) {
      this.rules.OnPlayerDisconnected(player);
    }
  }]);

  return Main;
}();

Engine.Export(Main);