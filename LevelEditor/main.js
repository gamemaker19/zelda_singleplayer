var __values = (this && this.__values) || function (o) {
    var m = typeof Symbol === "function" && o[Symbol.iterator], i = 0;
    if (m) return m.call(o);
    return {
        next: function () {
            if (o && i >= o.length) o = void 0;
            return { value: o && o[i++], done: !o };
        }
    };
};
System.register("canvasUI", [], function (exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var CanvasUI, MouseButton, KeyCode;
    return {
        setters: [],
        execute: function () {
            CanvasUI = (function () {
                function CanvasUI(canvasId) {
                    var _this = this;
                    this.ctrlHeld = false;
                    this.mouseX = 0;
                    this.mouseY = 0;
                    this.deltaX = 0;
                    this.deltaY = 0;
                    this.zoom = 1;
                    this.mousedown = false;
                    this.middlemousedown = false;
                    this.rightmousedown = false;
                    this.keysHeld = new Set();
                    this.canvasEl = $(canvasId)[0];
                    this.ctx = this.canvasEl.getContext("2d");
                    this.wrapper = $(this.canvasEl).parent()[0];
                    this.ctx.webkitImageSmoothingEnabled = false;
                    this.ctx.mozImageSmoothingEnabled = false;
                    this.ctx.imageSmoothingEnabled = false;
                    this.canvasEl.onkeydown = function (e) {
                        var keyCode = e.keyCode;
                        _this.onKeyDown(keyCode, !_this.keysHeld.has(keyCode));
                        _this.keysHeld.add(keyCode);
                        if (keyCode === KeyCode.SPACE) {
                            e.preventDefault();
                        }
                    };
                    this.canvasEl.onkeydown = function (e) {
                        var keyCode = e.keyCode;
                        var firstFrame = !_this.keysHeld.has(keyCode);
                        _this.keysHeld.add(keyCode);
                        _this.onKeyDown(e.keyCode, firstFrame);
                    };
                    this.canvasEl.onkeyup = function (e) {
                        var keyCode = e.keyCode;
                        _this.keysHeld.delete(keyCode);
                        _this.onKeyUp(e.keyCode);
                    };
                    this.canvasEl.onmousemove = function (event) {
                        var oldMouseX = _this.mouseX;
                        var oldMouseY = _this.mouseY;
                        var rawMouseX = event.pageX - _this.canvasEl.offsetLeft;
                        var rawMouseY = event.pageY - _this.canvasEl.offsetTop;
                        _this.mouseX = _this.getRealMouseX(rawMouseX);
                        _this.mouseY = _this.getRealMouseY(rawMouseY);
                        _this.deltaX = _this.mouseX - oldMouseX;
                        _this.deltaY = _this.mouseY - oldMouseY;
                        if (_this.mousedown) {
                            _this.dragEndX = _this.mouseX;
                            _this.dragEndY = _this.mouseY;
                        }
                        _this.onMouseMove(_this.deltaX, _this.deltaY);
                    };
                    this.canvasEl.onmousedown = function (e) {
                        if (e.which === 1) {
                            if (!_this.mousedown) {
                                _this.dragStartX = _this.mouseX;
                                _this.dragStartY = _this.mouseY;
                                _this.dragEndX = _this.mouseX;
                                _this.dragEndY = _this.mouseY;
                            }
                            _this.mousedown = true;
                            e.preventDefault();
                            _this.onMouseDown(MouseButton.LEFT);
                        }
                        else if (e.which === 2) {
                            _this.middlemousedown = true;
                            e.preventDefault();
                            _this.onMouseDown(MouseButton.MIDDLE);
                        }
                        else if (e.which === 3) {
                            _this.rightmousedown = true;
                            e.preventDefault();
                            _this.onMouseDown(MouseButton.RIGHT);
                        }
                    };
                    this.canvasEl.onmouseup = function (e) {
                        if (e.which === 1) {
                            _this.mousedown = false;
                            e.preventDefault();
                            _this.onMouseUp(MouseButton.LEFT);
                        }
                        else if (e.which === 2) {
                            _this.middlemousedown = false;
                            e.preventDefault();
                            _this.onMouseUp(MouseButton.MIDDLE);
                        }
                        else if (e.which === 3) {
                            _this.rightmousedown = true;
                            e.preventDefault();
                            _this.onMouseUp(MouseButton.RIGHT);
                        }
                    };
                    this.canvasEl.onmousewheel = function (e) {
                        var delta = (e.wheelDelta / 180);
                        _this.onMouseWheel(delta);
                    };
                    this.canvasEl.onmouseleave = function () {
                        _this.mousedown = false;
                        _this.onMouseLeave();
                    };
                }
                CanvasUI.prototype.isHeld = function (keyCode) {
                    return this.keysHeld.has(keyCode);
                };
                CanvasUI.prototype.redraw = function () { };
                CanvasUI.prototype.onMouseLeave = function () { };
                CanvasUI.prototype.onKeyUp = function (keyCode) {
                };
                CanvasUI.prototype.onMouseMove = function (deltaX, deltaY) {
                };
                CanvasUI.prototype.onMouseDown = function (mouseButton) {
                };
                CanvasUI.prototype.onMouseUp = function (mouseButton) {
                };
                CanvasUI.prototype.onKeyDown = function (keyCode, firstFrame) {
                };
                CanvasUI.prototype.onMouseWheel = function (delta) {
                };
                CanvasUI.prototype.getRealMouseX = function (rawMouseX) {
                    var zoomProportion = this.zoom - 1;
                    var w = this.canvasEl.width / this.zoom;
                    var w2 = (this.canvasEl.width - w) / 2;
                    return w2 + (rawMouseX * (1 / this.zoom));
                };
                CanvasUI.prototype.getRealMouseY = function (rawMouseY) {
                    var zoomProportion = this.zoom - 1;
                    var h = this.canvasEl.height / this.zoom;
                    var h2 = (this.canvasEl.height - h) / 2;
                    return h2 + (rawMouseY * (1 / this.zoom));
                };
                return CanvasUI;
            }());
            exports_1("CanvasUI", CanvasUI);
            (function (MouseButton) {
                MouseButton[MouseButton["LEFT"] = 1] = "LEFT";
                MouseButton[MouseButton["MIDDLE"] = 2] = "MIDDLE";
                MouseButton[MouseButton["RIGHT"] = 3] = "RIGHT";
            })(MouseButton || (MouseButton = {}));
            exports_1("MouseButton", MouseButton);
            (function (KeyCode) {
                KeyCode[KeyCode["CANCEL"] = 3] = "CANCEL";
                KeyCode[KeyCode["HELP"] = 6] = "HELP";
                KeyCode[KeyCode["BACK_SPACE"] = 8] = "BACK_SPACE";
                KeyCode[KeyCode["TAB"] = 9] = "TAB";
                KeyCode[KeyCode["CLEAR"] = 12] = "CLEAR";
                KeyCode[KeyCode["ENTER"] = 13] = "ENTER";
                KeyCode[KeyCode["ENTER_SPECIAL"] = 14] = "ENTER_SPECIAL";
                KeyCode[KeyCode["SHIFT"] = 16] = "SHIFT";
                KeyCode[KeyCode["CONTROL"] = 17] = "CONTROL";
                KeyCode[KeyCode["ALT"] = 18] = "ALT";
                KeyCode[KeyCode["PAUSE"] = 19] = "PAUSE";
                KeyCode[KeyCode["CAPS_LOCK"] = 20] = "CAPS_LOCK";
                KeyCode[KeyCode["KANA"] = 21] = "KANA";
                KeyCode[KeyCode["EISU"] = 22] = "EISU";
                KeyCode[KeyCode["JUNJA"] = 23] = "JUNJA";
                KeyCode[KeyCode["FINAL"] = 24] = "FINAL";
                KeyCode[KeyCode["HANJA"] = 25] = "HANJA";
                KeyCode[KeyCode["ESCAPE"] = 27] = "ESCAPE";
                KeyCode[KeyCode["CONVERT"] = 28] = "CONVERT";
                KeyCode[KeyCode["NONCONVERT"] = 29] = "NONCONVERT";
                KeyCode[KeyCode["ACCEPT"] = 30] = "ACCEPT";
                KeyCode[KeyCode["MODECHANGE"] = 31] = "MODECHANGE";
                KeyCode[KeyCode["SPACE"] = 32] = "SPACE";
                KeyCode[KeyCode["PAGE_UP"] = 33] = "PAGE_UP";
                KeyCode[KeyCode["PAGE_DOWN"] = 34] = "PAGE_DOWN";
                KeyCode[KeyCode["END"] = 35] = "END";
                KeyCode[KeyCode["HOME"] = 36] = "HOME";
                KeyCode[KeyCode["LEFT"] = 37] = "LEFT";
                KeyCode[KeyCode["UP"] = 38] = "UP";
                KeyCode[KeyCode["RIGHT"] = 39] = "RIGHT";
                KeyCode[KeyCode["DOWN"] = 40] = "DOWN";
                KeyCode[KeyCode["SELECT"] = 41] = "SELECT";
                KeyCode[KeyCode["PRINT"] = 42] = "PRINT";
                KeyCode[KeyCode["EXECUTE"] = 43] = "EXECUTE";
                KeyCode[KeyCode["PRINTSCREEN"] = 44] = "PRINTSCREEN";
                KeyCode[KeyCode["INSERT"] = 45] = "INSERT";
                KeyCode[KeyCode["DELETE"] = 46] = "DELETE";
                KeyCode[KeyCode["NUM_0"] = 48] = "NUM_0";
                KeyCode[KeyCode["NUM_1"] = 49] = "NUM_1";
                KeyCode[KeyCode["NUM_2"] = 50] = "NUM_2";
                KeyCode[KeyCode["NUM_3"] = 51] = "NUM_3";
                KeyCode[KeyCode["NUM_4"] = 52] = "NUM_4";
                KeyCode[KeyCode["NUM_5"] = 53] = "NUM_5";
                KeyCode[KeyCode["NUM_6"] = 54] = "NUM_6";
                KeyCode[KeyCode["NUM_7"] = 55] = "NUM_7";
                KeyCode[KeyCode["NUM_8"] = 56] = "NUM_8";
                KeyCode[KeyCode["NUM_9"] = 57] = "NUM_9";
                KeyCode[KeyCode["COLON"] = 58] = "COLON";
                KeyCode[KeyCode["SEMICOLON"] = 59] = "SEMICOLON";
                KeyCode[KeyCode["LESS_THAN"] = 60] = "LESS_THAN";
                KeyCode[KeyCode["EQUALS"] = 61] = "EQUALS";
                KeyCode[KeyCode["GREATER_THAN"] = 62] = "GREATER_THAN";
                KeyCode[KeyCode["QUESTION_MARK"] = 63] = "QUESTION_MARK";
                KeyCode[KeyCode["AT"] = 64] = "AT";
                KeyCode[KeyCode["A"] = 65] = "A";
                KeyCode[KeyCode["B"] = 66] = "B";
                KeyCode[KeyCode["C"] = 67] = "C";
                KeyCode[KeyCode["D"] = 68] = "D";
                KeyCode[KeyCode["E"] = 69] = "E";
                KeyCode[KeyCode["F"] = 70] = "F";
                KeyCode[KeyCode["G"] = 71] = "G";
                KeyCode[KeyCode["H"] = 72] = "H";
                KeyCode[KeyCode["I"] = 73] = "I";
                KeyCode[KeyCode["J"] = 74] = "J";
                KeyCode[KeyCode["K"] = 75] = "K";
                KeyCode[KeyCode["L"] = 76] = "L";
                KeyCode[KeyCode["M"] = 77] = "M";
                KeyCode[KeyCode["N"] = 78] = "N";
                KeyCode[KeyCode["O"] = 79] = "O";
                KeyCode[KeyCode["P"] = 80] = "P";
                KeyCode[KeyCode["Q"] = 81] = "Q";
                KeyCode[KeyCode["R"] = 82] = "R";
                KeyCode[KeyCode["S"] = 83] = "S";
                KeyCode[KeyCode["T"] = 84] = "T";
                KeyCode[KeyCode["U"] = 85] = "U";
                KeyCode[KeyCode["V"] = 86] = "V";
                KeyCode[KeyCode["W"] = 87] = "W";
                KeyCode[KeyCode["X"] = 88] = "X";
                KeyCode[KeyCode["Y"] = 89] = "Y";
                KeyCode[KeyCode["Z"] = 90] = "Z";
                KeyCode[KeyCode["SLEEP"] = 95] = "SLEEP";
                KeyCode[KeyCode["NUMPAD0"] = 96] = "NUMPAD0";
                KeyCode[KeyCode["NUMPAD1"] = 97] = "NUMPAD1";
                KeyCode[KeyCode["NUMPAD2"] = 98] = "NUMPAD2";
                KeyCode[KeyCode["NUMPAD3"] = 99] = "NUMPAD3";
                KeyCode[KeyCode["NUMPAD4"] = 100] = "NUMPAD4";
                KeyCode[KeyCode["NUMPAD5"] = 101] = "NUMPAD5";
                KeyCode[KeyCode["NUMPAD6"] = 102] = "NUMPAD6";
                KeyCode[KeyCode["NUMPAD7"] = 103] = "NUMPAD7";
                KeyCode[KeyCode["NUMPAD8"] = 104] = "NUMPAD8";
                KeyCode[KeyCode["NUMPAD9"] = 105] = "NUMPAD9";
                KeyCode[KeyCode["MULTIPLY"] = 106] = "MULTIPLY";
                KeyCode[KeyCode["ADD"] = 107] = "ADD";
                KeyCode[KeyCode["SEPARATOR"] = 108] = "SEPARATOR";
                KeyCode[KeyCode["SUBTRACT"] = 109] = "SUBTRACT";
                KeyCode[KeyCode["DECIMAL"] = 110] = "DECIMAL";
                KeyCode[KeyCode["DIVIDE"] = 111] = "DIVIDE";
                KeyCode[KeyCode["F1"] = 112] = "F1";
                KeyCode[KeyCode["F2"] = 113] = "F2";
                KeyCode[KeyCode["F3"] = 114] = "F3";
                KeyCode[KeyCode["F4"] = 115] = "F4";
                KeyCode[KeyCode["F5"] = 116] = "F5";
                KeyCode[KeyCode["F6"] = 117] = "F6";
                KeyCode[KeyCode["F7"] = 118] = "F7";
                KeyCode[KeyCode["F8"] = 119] = "F8";
                KeyCode[KeyCode["F9"] = 120] = "F9";
                KeyCode[KeyCode["F10"] = 121] = "F10";
                KeyCode[KeyCode["F11"] = 122] = "F11";
                KeyCode[KeyCode["F12"] = 123] = "F12";
                KeyCode[KeyCode["F13"] = 124] = "F13";
                KeyCode[KeyCode["F14"] = 125] = "F14";
                KeyCode[KeyCode["F15"] = 126] = "F15";
                KeyCode[KeyCode["F16"] = 127] = "F16";
                KeyCode[KeyCode["F17"] = 128] = "F17";
                KeyCode[KeyCode["F18"] = 129] = "F18";
                KeyCode[KeyCode["F19"] = 130] = "F19";
                KeyCode[KeyCode["F20"] = 131] = "F20";
                KeyCode[KeyCode["F21"] = 132] = "F21";
                KeyCode[KeyCode["F22"] = 133] = "F22";
                KeyCode[KeyCode["F23"] = 134] = "F23";
                KeyCode[KeyCode["F24"] = 135] = "F24";
                KeyCode[KeyCode["NUM_LOCK"] = 144] = "NUM_LOCK";
                KeyCode[KeyCode["SCROLL_LOCK"] = 145] = "SCROLL_LOCK";
                KeyCode[KeyCode["WIN_OEM_FJ_JISHO"] = 146] = "WIN_OEM_FJ_JISHO";
                KeyCode[KeyCode["WIN_OEM_FJ_MASSHOU"] = 147] = "WIN_OEM_FJ_MASSHOU";
                KeyCode[KeyCode["WIN_OEM_FJ_TOUROKU"] = 148] = "WIN_OEM_FJ_TOUROKU";
                KeyCode[KeyCode["WIN_OEM_FJ_LOYA"] = 149] = "WIN_OEM_FJ_LOYA";
                KeyCode[KeyCode["WIN_OEM_FJ_ROYA"] = 150] = "WIN_OEM_FJ_ROYA";
                KeyCode[KeyCode["CIRCUMFLEX"] = 160] = "CIRCUMFLEX";
                KeyCode[KeyCode["EXCLAMATION"] = 161] = "EXCLAMATION";
                KeyCode[KeyCode["DOUBLE_QUOTE"] = 162] = "DOUBLE_QUOTE";
                KeyCode[KeyCode["HASH"] = 163] = "HASH";
                KeyCode[KeyCode["DOLLAR"] = 164] = "DOLLAR";
                KeyCode[KeyCode["PERCENT"] = 165] = "PERCENT";
                KeyCode[KeyCode["AMPERSAND"] = 166] = "AMPERSAND";
                KeyCode[KeyCode["UNDERSCORE"] = 167] = "UNDERSCORE";
                KeyCode[KeyCode["OPEN_PAREN"] = 168] = "OPEN_PAREN";
                KeyCode[KeyCode["CLOSE_PAREN"] = 169] = "CLOSE_PAREN";
                KeyCode[KeyCode["ASTERISK"] = 170] = "ASTERISK";
                KeyCode[KeyCode["PLUS"] = 171] = "PLUS";
                KeyCode[KeyCode["PIPE"] = 172] = "PIPE";
                KeyCode[KeyCode["HYPHEN_MINUS"] = 173] = "HYPHEN_MINUS";
                KeyCode[KeyCode["OPEN_CURLY_BRACKET"] = 174] = "OPEN_CURLY_BRACKET";
                KeyCode[KeyCode["CLOSE_CURLY_BRACKET"] = 175] = "CLOSE_CURLY_BRACKET";
                KeyCode[KeyCode["TILDE"] = 176] = "TILDE";
                KeyCode[KeyCode["VOLUME_MUTE"] = 181] = "VOLUME_MUTE";
                KeyCode[KeyCode["VOLUME_DOWN"] = 182] = "VOLUME_DOWN";
                KeyCode[KeyCode["VOLUME_UP"] = 183] = "VOLUME_UP";
                KeyCode[KeyCode["COMMA"] = 188] = "COMMA";
                KeyCode[KeyCode["MINUS"] = 189] = "MINUS";
                KeyCode[KeyCode["PERIOD"] = 190] = "PERIOD";
                KeyCode[KeyCode["SLASH"] = 191] = "SLASH";
                KeyCode[KeyCode["BACK_QUOTE"] = 192] = "BACK_QUOTE";
            })(KeyCode || (KeyCode = {}));
            exports_1("KeyCode", KeyCode);
            ;
        }
    };
});
System.register("line", ["point"], function (exports_2, context_2) {
    "use strict";
    var __moduleName = context_2 && context_2.id;
    var point_1, Line;
    return {
        setters: [
            function (point_1_1) {
                point_1 = point_1_1;
            }
        ],
        execute: function () {
            Line = (function () {
                function Line(point1, point2) {
                    this.point1 = point1;
                    this.point2 = point2;
                }
                Line.prototype.onSegment = function (p, q, r) {
                    if (q.x <= Math.max(p.x, r.x) && q.x >= Math.min(p.x, r.x) &&
                        q.y <= Math.max(p.y, r.y) && q.y >= Math.min(p.y, r.y))
                        return true;
                    return false;
                };
                Line.prototype.orientation = function (p, q, r) {
                    var val = (q.y - p.y) * (r.x - q.x) -
                        (q.x - p.x) * (r.y - q.y);
                    if (val == 0)
                        return 0;
                    return (val > 0) ? 1 : 2;
                };
                Object.defineProperty(Line.prototype, "x1", {
                    get: function () { return this.point1.x; },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Line.prototype, "y1", {
                    get: function () { return this.point1.y; },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Line.prototype, "x2", {
                    get: function () { return this.point2.x; },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Line.prototype, "y2", {
                    get: function () { return this.point2.y; },
                    enumerable: true,
                    configurable: true
                });
                Line.prototype.checkLineIntersection = function (line1StartX, line1StartY, line1EndX, line1EndY, line2StartX, line2StartY, line2EndX, line2EndY) {
                    var denominator, a, b, numerator1, numerator2, result = {
                        x: null,
                        y: null,
                        onLine1: false,
                        onLine2: false
                    };
                    denominator = ((line2EndY - line2StartY) * (line1EndX - line1StartX)) - ((line2EndX - line2StartX) * (line1EndY - line1StartY));
                    if (denominator == 0) {
                        return result;
                    }
                    a = line1StartY - line2StartY;
                    b = line1StartX - line2StartX;
                    numerator1 = ((line2EndX - line2StartX) * a) - ((line2EndY - line2StartY) * b);
                    numerator2 = ((line1EndX - line1StartX) * a) - ((line1EndY - line1StartY) * b);
                    a = numerator1 / denominator;
                    b = numerator2 / denominator;
                    result.x = line1StartX + (a * (line1EndX - line1StartX));
                    result.y = line1StartY + (a * (line1EndY - line1StartY));
                    if (a > 0 && a < 1) {
                        result.onLine1 = true;
                    }
                    if (b > 0 && b < 1) {
                        result.onLine2 = true;
                    }
                    return result;
                };
                Line.prototype.getIntersectPoint = function (other) {
                    var doesIntersect = false;
                    var coincidePoint;
                    var p1 = this.point1;
                    var q1 = this.point2;
                    var p2 = other.point1;
                    var q2 = other.point2;
                    var o1 = this.orientation(p1, q1, p2);
                    var o2 = this.orientation(p1, q1, q2);
                    var o3 = this.orientation(p2, q2, p1);
                    var o4 = this.orientation(p2, q2, q1);
                    if (o1 != o2 && o3 != o4) {
                        doesIntersect = true;
                    }
                    if (o1 == 0 && this.onSegment(p1, p2, q1)) {
                        coincidePoint = p2;
                    }
                    else if (o2 == 0 && this.onSegment(p1, q2, q1)) {
                        coincidePoint = q2;
                    }
                    else if (o3 == 0 && this.onSegment(p2, p1, q2)) {
                        coincidePoint = p1;
                    }
                    else if (o4 == 0 && this.onSegment(p2, q1, q2)) {
                        coincidePoint = q1;
                    }
                    if (coincidePoint)
                        doesIntersect = true;
                    if (!doesIntersect)
                        return undefined;
                    if (coincidePoint)
                        return coincidePoint;
                    var intersection = this.checkLineIntersection(this.x1, this.y1, this.x2, this.y2, other.x1, other.y1, other.x2, other.y2);
                    if (intersection.x !== null && intersection.y !== null)
                        return new point_1.Point(intersection.x, intersection.y);
                    return new point_1.Point((this.x1 + this.x2) / 2, (this.y1 + this.y2) / 2);
                };
                Object.defineProperty(Line.prototype, "slope", {
                    get: function () {
                        if (this.x1 == this.x2)
                            return NaN;
                        return (this.y1 - this.y2) / (this.x1 - this.x2);
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Line.prototype, "yInt", {
                    get: function () {
                        if (this.x1 === this.x2)
                            return this.y1 === 0 ? 0 : NaN;
                        if (this.y1 === this.y2)
                            return this.y1;
                        return this.y1 - this.slope * this.x1;
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Line.prototype, "xInt", {
                    get: function () {
                        var slope;
                        if (this.y1 === this.y2)
                            return this.x1 == 0 ? 0 : NaN;
                        if (this.x1 === this.x2)
                            return this.x1;
                        return (-1 * ((slope = this.slope * this.x1 - this.y1)) / this.slope);
                    },
                    enumerable: true,
                    configurable: true
                });
                return Line;
            }());
            exports_2("Line", Line);
        }
    };
});
System.register("hitData", [], function (exports_3, context_3) {
    "use strict";
    var __moduleName = context_3 && context_3.id;
    var HitData;
    return {
        setters: [],
        execute: function () {
            HitData = (function () {
                function HitData(normal, hitPoint) {
                    this.normal = normal;
                    this.hitPoint = hitPoint;
                }
                return HitData;
            }());
            exports_3("HitData", HitData);
        }
    };
});
System.register("shape", ["point", "line", "rect", "hitData"], function (exports_4, context_4) {
    "use strict";
    var __moduleName = context_4 && context_4.id;
    var point_2, line_1, rect_1, hitData_1, Shape;
    return {
        setters: [
            function (point_2_1) {
                point_2 = point_2_1;
            },
            function (line_1_1) {
                line_1 = line_1_1;
            },
            function (rect_1_1) {
                rect_1 = rect_1_1;
            },
            function (hitData_1_1) {
                hitData_1 = hitData_1_1;
            }
        ],
        execute: function () {
            Shape = (function () {
                function Shape(points, normals) {
                    this.minX = Infinity;
                    this.minY = Infinity;
                    this.maxX = -Infinity;
                    this.maxY = -Infinity;
                    this.points = points;
                    var isNormalsSet = true;
                    if (!normals) {
                        normals = [];
                        isNormalsSet = false;
                    }
                    for (var i = 0; i < this.points.length; i++) {
                        var p1 = this.points[i];
                        var p2 = (i == this.points.length - 1 ? this.points[0] : this.points[i + 1]);
                        if (!isNormalsSet) {
                            var v = new point_2.Point(p2.x - p1.x, p2.y - p1.y);
                            normals.push(v.leftNormal().normalize());
                        }
                        if (p1.x < this.minX)
                            this.minX = p1.x;
                        if (p1.y < this.minY)
                            this.minY = p1.y;
                        if (p1.x > this.maxX)
                            this.maxX = p1.x;
                        if (p1.y > this.maxY)
                            this.maxY = p1.y;
                    }
                    this.normals = normals;
                }
                Shape.prototype.getRect = function () {
                    if (this.points.length !== 4)
                        return undefined;
                    if (this.points[0].x === this.points[3].x && this.points[1].x === this.points[2].x && this.points[0].y === this.points[1].y && this.points[2].y === this.points[3].y) {
                        return rect_1.Rect.Create(this.points[0], this.points[2]);
                    }
                    return undefined;
                };
                Shape.prototype.getLines = function () {
                    var lines = [];
                    for (var i = 0; i < this.points.length; i++) {
                        var next = i + 1;
                        if (next >= this.points.length)
                            next = 0;
                        lines.push(new line_1.Line(this.points[i], this.points[next]));
                    }
                    return lines;
                };
                Shape.prototype.getNormals = function () {
                    return this.normals;
                };
                Shape.prototype.intersectsLine = function (line) {
                    var lines = this.getLines();
                    try {
                        for (var lines_1 = __values(lines), lines_1_1 = lines_1.next(); !lines_1_1.done; lines_1_1 = lines_1.next()) {
                            var myLine = lines_1_1.value;
                            if (myLine.getIntersectPoint(line)) {
                                return true;
                            }
                        }
                    }
                    catch (e_1_1) { e_1 = { error: e_1_1 }; }
                    finally {
                        try {
                            if (lines_1_1 && !lines_1_1.done && (_a = lines_1.return)) _a.call(lines_1);
                        }
                        finally { if (e_1) throw e_1.error; }
                    }
                    return false;
                    var e_1, _a;
                };
                Shape.prototype.getLineIntersectCollisions = function (line) {
                    var collideDatas = [];
                    var lines = this.getLines();
                    var normals = this.getNormals();
                    for (var i = 0; i < lines.length; i++) {
                        var myLine = lines[i];
                        var point = myLine.getIntersectPoint(line);
                        if (point) {
                            var normal = normals[i];
                            var collideData = new hitData_1.HitData(normal, point);
                            collideDatas.push(collideData);
                        }
                    }
                    return collideDatas;
                };
                Shape.prototype.intersectsShape = function (other, vel) {
                    var pointOutside = false;
                    try {
                        for (var _a = __values(this.points), _b = _a.next(); !_b.done; _b = _a.next()) {
                            var point = _b.value;
                            if (!other.containsPoint(point)) {
                                pointOutside = true;
                                break;
                            }
                        }
                    }
                    catch (e_2_1) { e_2 = { error: e_2_1 }; }
                    finally {
                        try {
                            if (_b && !_b.done && (_c = _a.return)) _c.call(_a);
                        }
                        finally { if (e_2) throw e_2.error; }
                    }
                    var pointOutside2 = false;
                    try {
                        for (var _d = __values(other.points), _e = _d.next(); !_e.done; _e = _d.next()) {
                            var point = _e.value;
                            if (!this.containsPoint(point)) {
                                pointOutside2 = true;
                                break;
                            }
                        }
                    }
                    catch (e_3_1) { e_3 = { error: e_3_1 }; }
                    finally {
                        try {
                            if (_e && !_e.done && (_f = _d.return)) _f.call(_d);
                        }
                        finally { if (e_3) throw e_3.error; }
                    }
                    if (!pointOutside || !pointOutside2) {
                        return new hitData_1.HitData(undefined, undefined);
                    }
                    var lines1 = this.getLines();
                    var lines2 = other.getLines();
                    var hitNormals = [];
                    try {
                        for (var lines1_1 = __values(lines1), lines1_1_1 = lines1_1.next(); !lines1_1_1.done; lines1_1_1 = lines1_1.next()) {
                            var line1 = lines1_1_1.value;
                            var normals = other.getNormals();
                            for (var i = 0; i < lines2.length; i++) {
                                var line2 = lines2[i];
                                if (line1.getIntersectPoint(line2)) {
                                    if (!vel) {
                                        return new hitData_1.HitData(normals[i], undefined);
                                    }
                                    else {
                                        hitNormals.push(normals[i]);
                                    }
                                }
                            }
                        }
                    }
                    catch (e_4_1) { e_4 = { error: e_4_1 }; }
                    finally {
                        try {
                            if (lines1_1_1 && !lines1_1_1.done && (_g = lines1_1.return)) _g.call(lines1_1);
                        }
                        finally { if (e_4) throw e_4.error; }
                    }
                    if (hitNormals.length === 0) {
                        return undefined;
                    }
                    try {
                        for (var hitNormals_1 = __values(hitNormals), hitNormals_1_1 = hitNormals_1.next(); !hitNormals_1_1.done; hitNormals_1_1 = hitNormals_1.next()) {
                            var normal = hitNormals_1_1.value;
                            var ang = vel.times(-1).angleWith(normal);
                            if (ang < 90) {
                                return new hitData_1.HitData(normal, undefined);
                            }
                        }
                    }
                    catch (e_5_1) { e_5 = { error: e_5_1 }; }
                    finally {
                        try {
                            if (hitNormals_1_1 && !hitNormals_1_1.done && (_h = hitNormals_1.return)) _h.call(hitNormals_1);
                        }
                        finally { if (e_5) throw e_5.error; }
                    }
                    if (hitNormals.length > 0) {
                        return new hitData_1.HitData(hitNormals[0], undefined);
                    }
                    return undefined;
                    var e_2, _c, e_3, _f, e_4, _g, e_5, _h;
                };
                Shape.prototype.containsPoint = function (point) {
                    var x = point.x;
                    var y = point.y;
                    var vertices = this.points;
                    var inside = false;
                    for (var i = 0, j = vertices.length - 1; i < vertices.length; j = i++) {
                        var xi = vertices[i].x, yi = vertices[i].y;
                        var xj = vertices[j].x, yj = vertices[j].y;
                        var intersect = ((yi > y) !== (yj > y))
                            && (x < (xj - xi) * (y - yi) / (yj - yi) + xi);
                        if (intersect)
                            inside = !inside;
                    }
                    return inside;
                };
                Shape.prototype.getIntersectPoint = function (point, dir) {
                    if (this.containsPoint(point)) {
                        return point;
                    }
                    var intersections = [];
                    var pointLine = new line_1.Line(point, point.add(dir));
                    try {
                        for (var _a = __values(this.getLines()), _b = _a.next(); !_b.done; _b = _a.next()) {
                            var line = _b.value;
                            var intersectPoint = line.getIntersectPoint(pointLine);
                            if (intersectPoint) {
                                intersections.push(intersectPoint);
                            }
                        }
                    }
                    catch (e_6_1) { e_6 = { error: e_6_1 }; }
                    finally {
                        try {
                            if (_b && !_b.done && (_c = _a.return)) _c.call(_a);
                        }
                        finally { if (e_6) throw e_6.error; }
                    }
                    if (intersections.length === 0)
                        return undefined;
                    return _.minBy(intersections, function (intersectPoint) {
                        return intersectPoint.distanceTo(point);
                    });
                    var e_6, _c;
                };
                Shape.prototype.getClosestPointOnBounds = function (point) {
                };
                Shape.prototype.minMaxDotProd = function (normal) {
                    var min = null, max = null;
                    try {
                        for (var _a = __values(this.points), _b = _a.next(); !_b.done; _b = _a.next()) {
                            var point = _b.value;
                            var dp = point.dotProduct(normal);
                            if (min === null || dp < min)
                                min = dp;
                            if (max === null || dp > max)
                                max = dp;
                        }
                    }
                    catch (e_7_1) { e_7 = { error: e_7_1 }; }
                    finally {
                        try {
                            if (_b && !_b.done && (_c = _a.return)) _c.call(_a);
                        }
                        finally { if (e_7) throw e_7.error; }
                    }
                    return [min, max];
                    var e_7, _c;
                };
                Shape.prototype.checkNormal = function (other, normal) {
                    var aMinMax = this.minMaxDotProd(normal);
                    var bMinMax = other.minMaxDotProd(normal);
                    var overlap = 0;
                    if (aMinMax[0] > bMinMax[0] && aMinMax[1] < bMinMax[1]) {
                        overlap = aMinMax[1] - aMinMax[0];
                    }
                    if (bMinMax[0] > aMinMax[0] && bMinMax[1] < aMinMax[1]) {
                        overlap = bMinMax[1] - bMinMax[0];
                    }
                    if (overlap > 0) {
                        var mins = Math.abs(aMinMax[0] - bMinMax[0]);
                        var maxs = Math.abs(aMinMax[1] - bMinMax[1]);
                        if (mins < maxs) {
                            overlap += mins;
                        }
                        else {
                            overlap += maxs;
                        }
                        var correction = normal.times(overlap);
                        return correction;
                    }
                    if (aMinMax[0] <= bMinMax[1] && aMinMax[1] >= bMinMax[0]) {
                        var correction = normal.times(bMinMax[1] - aMinMax[0]);
                        return correction;
                    }
                    return undefined;
                };
                Shape.prototype.getMinTransVector = function (b) {
                    var correctionVectors = [];
                    var thisNormals;
                    var bNormals;
                    var dir = undefined;
                    if (dir) {
                        thisNormals = [dir];
                        bNormals = [dir];
                    }
                    else {
                        thisNormals = this.getNormals();
                        bNormals = b.getNormals();
                    }
                    try {
                        for (var thisNormals_1 = __values(thisNormals), thisNormals_1_1 = thisNormals_1.next(); !thisNormals_1_1.done; thisNormals_1_1 = thisNormals_1.next()) {
                            var normal = thisNormals_1_1.value;
                            var result = this.checkNormal(b, normal);
                            if (result)
                                correctionVectors.push(result);
                        }
                    }
                    catch (e_8_1) { e_8 = { error: e_8_1 }; }
                    finally {
                        try {
                            if (thisNormals_1_1 && !thisNormals_1_1.done && (_a = thisNormals_1.return)) _a.call(thisNormals_1);
                        }
                        finally { if (e_8) throw e_8.error; }
                    }
                    try {
                        for (var bNormals_1 = __values(bNormals), bNormals_1_1 = bNormals_1.next(); !bNormals_1_1.done; bNormals_1_1 = bNormals_1.next()) {
                            var normal = bNormals_1_1.value;
                            var result = this.checkNormal(b, normal);
                            if (result)
                                correctionVectors.push(result);
                        }
                    }
                    catch (e_9_1) { e_9 = { error: e_9_1 }; }
                    finally {
                        try {
                            if (bNormals_1_1 && !bNormals_1_1.done && (_b = bNormals_1.return)) _b.call(bNormals_1);
                        }
                        finally { if (e_9) throw e_9.error; }
                    }
                    if (correctionVectors.length > 0) {
                        return _.minBy(correctionVectors, function (correctionVector) {
                            return correctionVector.magnitude;
                        });
                    }
                    return undefined;
                    var e_8, _a, e_9, _b;
                };
                Shape.prototype.getMinTransVectorDir = function (b, dir) {
                    dir = dir.normalize();
                    var mag = 0;
                    var maxMag = 0;
                    try {
                        for (var _a = __values(this.points), _b = _a.next(); !_b.done; _b = _a.next()) {
                            var point = _b.value;
                            var line = new line_1.Line(point, point.add(dir.times(10000)));
                            try {
                                for (var _c = __values(b.getLines()), _d = _c.next(); !_d.done; _d = _c.next()) {
                                    var bLine = _d.value;
                                    var intersectPoint = bLine.getIntersectPoint(line);
                                    if (intersectPoint) {
                                        mag = point.distanceTo(intersectPoint);
                                        if (mag > maxMag) {
                                            maxMag = mag;
                                        }
                                    }
                                }
                            }
                            catch (e_10_1) { e_10 = { error: e_10_1 }; }
                            finally {
                                try {
                                    if (_d && !_d.done && (_e = _c.return)) _e.call(_c);
                                }
                                finally { if (e_10) throw e_10.error; }
                            }
                        }
                    }
                    catch (e_11_1) { e_11 = { error: e_11_1 }; }
                    finally {
                        try {
                            if (_b && !_b.done && (_f = _a.return)) _f.call(_a);
                        }
                        finally { if (e_11) throw e_11.error; }
                    }
                    try {
                        for (var _g = __values(b.points), _h = _g.next(); !_h.done; _h = _g.next()) {
                            var point = _h.value;
                            var line = new line_1.Line(point, point.add(dir.times(-10000)));
                            try {
                                for (var _j = __values(this.getLines()), _k = _j.next(); !_k.done; _k = _j.next()) {
                                    var myLine = _k.value;
                                    var intersectPoint = myLine.getIntersectPoint(line);
                                    if (intersectPoint) {
                                        mag = point.distanceTo(intersectPoint);
                                        if (mag > maxMag) {
                                            maxMag = mag;
                                        }
                                    }
                                }
                            }
                            catch (e_12_1) { e_12 = { error: e_12_1 }; }
                            finally {
                                try {
                                    if (_k && !_k.done && (_l = _j.return)) _l.call(_j);
                                }
                                finally { if (e_12) throw e_12.error; }
                            }
                        }
                    }
                    catch (e_13_1) { e_13 = { error: e_13_1 }; }
                    finally {
                        try {
                            if (_h && !_h.done && (_m = _g.return)) _m.call(_g);
                        }
                        finally { if (e_13) throw e_13.error; }
                    }
                    if (maxMag === 0) {
                        return undefined;
                    }
                    return dir.times(maxMag);
                    var e_11, _f, e_10, _e, e_13, _m, e_12, _l;
                };
                Shape.prototype.getSnapVector = function (b, dir) {
                    var mag = 0;
                    var minMag = Infinity;
                    try {
                        for (var _a = __values(this.points), _b = _a.next(); !_b.done; _b = _a.next()) {
                            var point = _b.value;
                            var line = new line_1.Line(point, point.add(dir.times(10000)));
                            try {
                                for (var _c = __values(b.getLines()), _d = _c.next(); !_d.done; _d = _c.next()) {
                                    var bLine = _d.value;
                                    var intersectPoint = bLine.getIntersectPoint(line);
                                    if (intersectPoint) {
                                        mag = point.distanceTo(intersectPoint);
                                        if (mag < minMag) {
                                            minMag = mag;
                                        }
                                    }
                                }
                            }
                            catch (e_14_1) { e_14 = { error: e_14_1 }; }
                            finally {
                                try {
                                    if (_d && !_d.done && (_e = _c.return)) _e.call(_c);
                                }
                                finally { if (e_14) throw e_14.error; }
                            }
                        }
                    }
                    catch (e_15_1) { e_15 = { error: e_15_1 }; }
                    finally {
                        try {
                            if (_b && !_b.done && (_f = _a.return)) _f.call(_a);
                        }
                        finally { if (e_15) throw e_15.error; }
                    }
                    if (mag === 0) {
                        return undefined;
                    }
                    return dir.times(minMag);
                    var e_15, _f, e_14, _e;
                };
                Shape.prototype.clone = function (x, y) {
                    var points = [];
                    for (var i = 0; i < this.points.length; i++) {
                        var point = this.points[i];
                        points.push(new point_2.Point(point.x + x, point.y + y));
                    }
                    return new Shape(points, this.normals);
                };
                return Shape;
            }());
            exports_4("Shape", Shape);
        }
    };
});
System.register("rect", ["point", "shape"], function (exports_5, context_5) {
    "use strict";
    var __moduleName = context_5 && context_5.id;
    var point_3, shape_1, Rect;
    return {
        setters: [
            function (point_3_1) {
                point_3 = point_3_1;
            },
            function (shape_1_1) {
                shape_1 = shape_1_1;
            }
        ],
        execute: function () {
            Rect = (function () {
                function Rect(x1, y1, x2, y2) {
                    this.topLeftPoint = new point_3.Point(x1, y1);
                    this.botRightPoint = new point_3.Point(x2, y2);
                }
                Rect.Create = function (topLeftPoint, botRightPoint) {
                    return new Rect(topLeftPoint.x, topLeftPoint.y, botRightPoint.x, botRightPoint.y);
                };
                Rect.prototype.getShape = function () {
                    return new shape_1.Shape([this.topLeftPoint, new point_3.Point(this.x2, this.y1), this.botRightPoint, new point_3.Point(this.x1, this.y2)]);
                };
                Object.defineProperty(Rect.prototype, "midX", {
                    get: function () {
                        return (this.topLeftPoint.x + this.botRightPoint.x) * 0.5;
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Rect.prototype, "x1", {
                    get: function () {
                        return this.topLeftPoint.x;
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Rect.prototype, "y1", {
                    get: function () {
                        return this.topLeftPoint.y;
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Rect.prototype, "x2", {
                    get: function () {
                        return this.botRightPoint.x;
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Rect.prototype, "y2", {
                    get: function () {
                        return this.botRightPoint.y;
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Rect.prototype, "w", {
                    get: function () {
                        return this.botRightPoint.x - this.topLeftPoint.x;
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Rect.prototype, "h", {
                    get: function () {
                        return this.botRightPoint.y - this.topLeftPoint.y;
                    },
                    enumerable: true,
                    configurable: true
                });
                Rect.prototype.getPoints = function () {
                    return [
                        new point_3.Point(this.topLeftPoint.x, this.topLeftPoint.y),
                        new point_3.Point(this.botRightPoint.x, this.topLeftPoint.y),
                        new point_3.Point(this.botRightPoint.x, this.botRightPoint.y),
                        new point_3.Point(this.topLeftPoint.x, this.botRightPoint.y),
                    ];
                };
                Rect.prototype.overlaps = function (other) {
                    if (this.x1 > other.x2 || other.x1 > this.x2)
                        return false;
                    if (this.y1 > other.y2 || other.y1 > this.y2)
                        return false;
                    return true;
                };
                Rect.prototype.clone = function (x, y) {
                    return new Rect(this.x1 + x, this.y1 + y, this.x2 + x, this.y2 + y);
                };
                return Rect;
            }());
            exports_5("Rect", Rect);
        }
    };
});
System.register("color", [], function (exports_6, context_6) {
    "use strict";
    var __moduleName = context_6 && context_6.id;
    var Color;
    return {
        setters: [],
        execute: function () {
            Color = (function () {
                function Color(r, g, b, a) {
                    if (r === undefined || g === undefined || b === undefined || a === undefined)
                        throw "Bad color";
                    this.r = r;
                    this.g = g;
                    this.b = b;
                    this.a = a;
                }
                Object.defineProperty(Color.prototype, "hex", {
                    get: function () {
                        return "#" + this.r.toString(16) + this.g.toString(16) + this.b.toString(16) + this.a.toString(16);
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Color.prototype, "number", {
                    get: function () {
                        var rString = this.r.toString(16);
                        var gString = this.g.toString(16);
                        var bString = this.b.toString(16);
                        if (rString.length === 1)
                            rString = "0" + rString;
                        if (gString.length === 1)
                            gString = "0" + gString;
                        if (bString.length === 1)
                            bString = "0" + bString;
                        var hex = "0x" + rString + gString + bString;
                        return parseInt(hex, 16);
                    },
                    enumerable: true,
                    configurable: true
                });
                return Color;
            }());
            exports_6("Color", Color);
        }
    };
});
System.register("helpers", ["rect", "point", "color", "../node_modules/@types/lodash/index"], function (exports_7, context_7) {
    "use strict";
    var __moduleName = context_7 && context_7.id;
    function inRect(x, y, rect) {
        var rx = rect.x1;
        var ry = rect.y1;
        var rx2 = rect.x2;
        var ry2 = rect.y2;
        return x >= rx && x <= rx2 && y >= ry && y <= ry2;
    }
    exports_7("inRect", inRect);
    function inCircle(x, y, circleX, circleY, r) {
        if (Math.sqrt(Math.pow(x - circleX, 2) + Math.pow(y - circleY, 2)) <= r) {
            return true;
        }
        return false;
    }
    exports_7("inCircle", inCircle);
    function toZero(num, inc, dir) {
        if (dir === 1) {
            num -= inc;
            if (num < 0)
                num = 0;
            return num;
        }
        else if (dir === -1) {
            num += inc;
            if (num > 0)
                num = 0;
            return num;
        }
        else {
            throw "Must pass in -1 or 1 for dir";
        }
    }
    exports_7("toZero", toZero);
    function incrementRange(num, min, max) {
        num++;
        if (num >= max)
            num = min;
        return num;
    }
    exports_7("incrementRange", incrementRange);
    function decrementRange(num, min, max) {
        num--;
        if (num < min)
            num = max - 1;
        return num;
    }
    exports_7("decrementRange", decrementRange);
    function clamp01(num) {
        if (num < 0)
            num = 0;
        if (num > 1)
            num = 1;
        return num;
    }
    exports_7("clamp01", clamp01);
    function randomRange(start, end) {
        return index_1.default.random(start, end);
    }
    exports_7("randomRange", randomRange);
    function clampMax(num, max) {
        return num < max ? num : max;
    }
    exports_7("clampMax", clampMax);
    function clampMin(num, min) {
        return num > min ? num : min;
    }
    exports_7("clampMin", clampMin);
    function clampMin0(num) {
        return clampMin(num, 0);
    }
    exports_7("clampMin0", clampMin0);
    function clamp(num, min, max) {
        if (num < min)
            return min;
        if (num > max)
            return max;
        return num;
    }
    exports_7("clamp", clamp);
    function sin(degrees) {
        var rads = degrees * Math.PI / 180;
        return Math.sin(rads);
    }
    exports_7("sin", sin);
    function cos(degrees) {
        var rads = degrees * Math.PI / 180;
        return Math.cos(rads);
    }
    exports_7("cos", cos);
    function atan(value) {
        return Math.atan(value) * 180 / Math.PI;
    }
    exports_7("atan", atan);
    function moveTo(num, dest, inc) {
        inc *= Math.sign(dest - num);
        num += inc;
        return num;
    }
    exports_7("moveTo", moveTo);
    function lerp(num, dest, timeScale) {
        num = num + (dest - num) * timeScale;
        return num;
    }
    exports_7("lerp", lerp);
    function lerpNoOver(num, dest, timeScale) {
        num = num + (dest - num) * timeScale;
        if (Math.abs(num - dest) < 1)
            num = dest;
        return num;
    }
    exports_7("lerpNoOver", lerpNoOver);
    function lerpAngle(angle, destAngle, timeScale) {
        var dir = 1;
        if (Math.abs(destAngle - angle) > 180) {
            dir = -1;
        }
        angle = angle + dir * (destAngle - angle) * timeScale;
        return to360(angle);
    }
    exports_7("lerpAngle", lerpAngle);
    function to360(angle) {
        if (angle < 0)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return angle;
    }
    exports_7("to360", to360);
    function getHex(r, g, b, a) {
        return "#" + r.toString(16) + g.toString(16) + b.toString(16) + a.toString(16);
    }
    exports_7("getHex", getHex);
    function roundEpsilon(num) {
        var numRound = Math.round(num);
        var diff = Math.abs(numRound - num);
        if (diff < 0.0001) {
            return numRound;
        }
        return num;
    }
    exports_7("roundEpsilon", roundEpsilon);
    function getAutoIncId() {
        autoInc++;
        return autoInc;
    }
    exports_7("getAutoIncId", getAutoIncId);
    function stringReplace(str, pattern, replacement) {
        return str.replace(new RegExp(pattern, 'g'), replacement);
    }
    exports_7("stringReplace", stringReplace);
    function noCanvasSmoothing(c) {
        c.webkitImageSmoothingEnabled = false;
        c.mozImageSmoothingEnabled = false;
        c.imageSmoothingEnabled = false;
    }
    exports_7("noCanvasSmoothing", noCanvasSmoothing);
    function drawImage(ctx, imgEl, sX, sY, sW, sH, x, y, flipX, flipY, options, alpha, scaleX, scaleY) {
        if (!sW) {
            ctx.drawImage(imgEl, (sX), sY);
            return;
        }
        ctx.globalAlpha = (alpha === null || alpha === undefined) ? 1 : alpha;
        helperCanvas.width = sW;
        helperCanvas.height = sH;
        helperCtx.save();
        scaleX = scaleX || 1;
        scaleY = scaleY || 1;
        flipX = (flipX || 1);
        flipY = (flipY || 1);
        helperCtx.scale(flipX * scaleX, flipY * scaleY);
        helperCtx.clearRect(0, 0, helperCanvas.width, helperCanvas.height);
        helperCtx.drawImage(imgEl, sX, sY, sW, sH, 0, 0, flipX * sW, flipY * sH);
        ctx.drawImage(helperCanvas, x, y);
        ctx.globalAlpha = 1;
        helperCtx.restore();
    }
    exports_7("drawImage", drawImage);
    function createAndDrawRect(container, rect, fillColor, strokeColor, strokeWidth, fillAlpha) {
        var rectangle = new PIXI.Graphics();
        if (fillAlpha === undefined)
            fillAlpha = 1;
        if (strokeColor) {
            rectangle.lineStyle(strokeWidth, strokeColor, fillAlpha);
        }
        if (fillColor !== undefined)
            rectangle.beginFill(fillColor, fillAlpha);
        rectangle.drawRect(rect.x1, rect.y1, rect.w, rect.h);
        if (fillColor !== undefined)
            rectangle.endFill();
        container.addChild(rectangle);
        return rectangle;
    }
    exports_7("createAndDrawRect", createAndDrawRect);
    function drawRect(ctx, rect, fillColor, strokeColor, strokeWidth, fillAlpha) {
        var rx = Math.round(rect.x1);
        var ry = Math.round(rect.y1);
        var rx2 = Math.round(rect.x2);
        var ry2 = Math.round(rect.y2);
        ctx.beginPath();
        ctx.rect(rx, ry, rx2 - rx, ry2 - ry);
        if (fillAlpha) {
            ctx.globalAlpha = fillAlpha;
        }
        if (strokeColor) {
            strokeWidth = strokeWidth ? strokeWidth : 1;
            ctx.lineWidth = strokeWidth;
            ctx.strokeStyle = strokeColor;
            ctx.stroke();
        }
        if (fillColor) {
            ctx.fillStyle = fillColor;
            ctx.fill();
        }
        ctx.globalAlpha = 1;
    }
    exports_7("drawRect", drawRect);
    function drawPolygon(ctx, shape, closed, fillColor, lineColor, lineThickness, fillAlpha) {
        var vertices = shape.points;
        if (fillAlpha) {
            ctx.globalAlpha = fillAlpha;
        }
        ctx.beginPath();
        ctx.moveTo(vertices[0].x, vertices[0].y);
        for (var i = 1; i < vertices.length; i++) {
            ctx.lineTo(vertices[i].x, vertices[i].y);
        }
        if (closed) {
            ctx.closePath();
            if (fillColor) {
                ctx.fillStyle = fillColor;
                ctx.fill();
            }
        }
        if (lineColor) {
            ctx.lineWidth = lineThickness;
            ctx.strokeStyle = lineColor;
            ctx.stroke();
        }
        ctx.globalAlpha = 1;
    }
    exports_7("drawPolygon", drawPolygon);
    function setTextGradient(text, isRed) {
        var col = "#6090D0";
        if (isRed)
            col = "#f44256";
        text.style.fill = [col, "#C8D8E8", col];
        text.style.fillGradientType = PIXI.TEXT_GRADIENT.LINEAR_VERTICAL;
        text.style.fillGradientStops = [0, 0.5, 1];
    }
    exports_7("setTextGradient", setTextGradient);
    function createAndDrawText(container, text, x, y, size, hAlign, vAlign, isRed, overrideColor) {
        var message = new PIXI.Text(text);
        size = size || 14;
        hAlign = hAlign || "center";
        vAlign = vAlign || "middle";
        var alignX = 1;
        var alignY = 1;
        if (hAlign === "left")
            alignX = 0;
        if (hAlign === "center")
            alignX = 0.5;
        if (hAlign === "right")
            alignX = 1;
        if (vAlign === "top")
            alignY = 0;
        if (vAlign === "middle")
            alignY = 0.5;
        if (vAlign === "bottom")
            alignY = 1;
        message.anchor.set(alignX, alignY);
        var style = new PIXI.TextStyle({
            fontFamily: "mmx_font",
            dropShadow: true,
            dropShadowColor: "#000000",
            dropShadowBlur: 0,
            dropShadowDistance: size / 2,
        });
        message.style = style;
        if (!overrideColor) {
            setTextGradient(message, isRed);
        }
        else {
            style.fill = overrideColor;
        }
        style.fontSize = size * 3;
        message.position.set(x, y);
        message.scale.set(1 / 3, 1 / 3);
        container.addChild(message);
        return message;
    }
    exports_7("createAndDrawText", createAndDrawText);
    function drawText(ctx, text, x, y, fillColor, outlineColor, size, hAlign, vAlign, font) {
        ctx.save();
        fillColor = fillColor || "black";
        size = size || 14;
        hAlign = hAlign || "center";
        vAlign = vAlign || "middle";
        font = font || "Arial";
        ctx.font = size + "px " + font;
        ctx.fillStyle = fillColor;
        ctx.textAlign = hAlign;
        ctx.textBaseline = vAlign;
        ctx.fillText(text, x, y);
        if (outlineColor) {
            ctx.lineWidth = 1;
            ctx.strokeStyle = outlineColor;
            ctx.strokeText(text, x, y);
        }
        ctx.restore();
    }
    exports_7("drawText", drawText);
    function drawCircle(ctx, x, y, r, fillColor, lineColor, lineThickness) {
        ctx.beginPath();
        ctx.arc(x, y, r, 0, 2 * Math.PI, false);
        if (fillColor) {
            ctx.fillStyle = fillColor;
            ctx.fill();
        }
        if (lineColor) {
            ctx.lineWidth = lineThickness;
            ctx.strokeStyle = lineColor;
            ctx.stroke();
        }
    }
    exports_7("drawCircle", drawCircle);
    function createAndDrawLine(container, x, y, x2, y2, color, thickness) {
        var line = new PIXI.Graphics();
        if (!thickness)
            thickness = 1;
        if (!color)
            color = 0x000000;
        line.lineStyle(thickness, color, 1);
        line.moveTo(x, y);
        line.lineTo(x2, y2);
        line.x = 0;
        line.y = 0;
        container.addChild(line);
        return line;
    }
    exports_7("createAndDrawLine", createAndDrawLine);
    function drawLine(ctx, x, y, x2, y2, color, thickness) {
        if (!thickness)
            thickness = 1;
        if (!color)
            color = 'black';
        ctx.beginPath();
        ctx.moveTo(x, y);
        ctx.lineTo(x2, y2);
        ctx.lineWidth = thickness;
        ctx.strokeStyle = color;
        ctx.stroke();
    }
    exports_7("drawLine", drawLine);
    function linepointNearestMouse(x0, y0, x1, y1, x, y) {
        function lerp(a, b, x) { return (a + x * (b - a)); }
        ;
        var dx = x1 - x0;
        var dy = y1 - y0;
        var t = ((x - x0) * dx + (y - y0) * dy) / (dx * dx + dy * dy);
        var lineX = lerp(x0, x1, t);
        var lineY = lerp(y0, y1, t);
        return new point_4.Point(lineX, lineY);
    }
    exports_7("linepointNearestMouse", linepointNearestMouse);
    function inLine(mouseX, mouseY, x0, y0, x1, y1) {
        var threshold = 4;
        var small_x = Math.min(x0, x1);
        var big_x = Math.max(x0, x1);
        if (mouseX < small_x - (threshold * 0.5) || mouseX > big_x + (threshold * 0.5)) {
            return false;
        }
        var linepoint = linepointNearestMouse(x0, y0, x1, y1, mouseX, mouseY);
        var dx = mouseX - linepoint.x;
        var dy = mouseY - linepoint.y;
        var distance = Math.abs(Math.sqrt(dx * dx + dy * dy));
        if (distance < threshold) {
            return true;
        }
        else {
            return false;
        }
    }
    exports_7("inLine", inLine);
    function getInclinePushDir(inclineNormal, pushDir) {
        var bisectingPoint = inclineNormal.normalize().add(pushDir.normalize());
        bisectingPoint = bisectingPoint.normalize();
        if (Math.abs(bisectingPoint.x) >= Math.abs(bisectingPoint.y)) {
            bisectingPoint.y = 0;
        }
        else {
            bisectingPoint.x = 0;
        }
        return bisectingPoint.normalize();
    }
    exports_7("getInclinePushDir", getInclinePushDir);
    function keyCodeToString(charCode) {
        if (charCode === 0)
            return "left mouse";
        if (charCode === 1)
            return "middle mouse";
        if (charCode === 2)
            return "right mouse";
        if (charCode === 3)
            return "wheel up";
        if (charCode === 4)
            return "wheel down";
        if (charCode == 8)
            return "backspace";
        if (charCode == 9)
            return "tab";
        if (charCode == 13)
            return "enter";
        if (charCode == 16)
            return "shift";
        if (charCode == 17)
            return "ctrl";
        if (charCode == 18)
            return "alt";
        if (charCode == 19)
            return "pause/break";
        if (charCode == 20)
            return "caps lock";
        if (charCode == 27)
            return "escape";
        if (charCode == 33)
            return "page up";
        if (charCode == 34)
            return "page down";
        if (charCode == 35)
            return "end";
        if (charCode == 36)
            return "home";
        if (charCode == 37)
            return "left arrow";
        if (charCode == 38)
            return "up arrow";
        if (charCode == 39)
            return "right arrow";
        if (charCode == 40)
            return "down arrow";
        if (charCode == 45)
            return "insert";
        if (charCode == 46)
            return "delete";
        if (charCode == 91)
            return "left window";
        if (charCode == 92)
            return "right window";
        if (charCode == 93)
            return "select key";
        if (charCode == 96)
            return "numpad 0";
        if (charCode == 97)
            return "numpad 1";
        if (charCode == 98)
            return "numpad 2";
        if (charCode == 99)
            return "numpad 3";
        if (charCode == 100)
            return "numpad 4";
        if (charCode == 101)
            return "numpad 5";
        if (charCode == 102)
            return "numpad 6";
        if (charCode == 103)
            return "numpad 7";
        if (charCode == 104)
            return "numpad 8";
        if (charCode == 105)
            return "numpad 9";
        if (charCode == 106)
            return "multiply";
        if (charCode == 107)
            return "add";
        if (charCode == 109)
            return "subtract";
        if (charCode == 110)
            return "decimal point";
        if (charCode == 111)
            return "divide";
        if (charCode == 112)
            return "F1";
        if (charCode == 113)
            return "F2";
        if (charCode == 114)
            return "F3";
        if (charCode == 115)
            return "F4";
        if (charCode == 116)
            return "F5";
        if (charCode == 117)
            return "F6";
        if (charCode == 118)
            return "F7";
        if (charCode == 119)
            return "F8";
        if (charCode == 120)
            return "F9";
        if (charCode == 121)
            return "F10";
        if (charCode == 122)
            return "F11";
        if (charCode == 123)
            return "F12";
        if (charCode == 144)
            return "num lock";
        if (charCode == 145)
            return "scroll lock";
        if (charCode == 186)
            return ";";
        if (charCode == 187)
            return "=";
        if (charCode == 188)
            return ",";
        if (charCode == 189)
            return "-";
        if (charCode == 190)
            return ".";
        if (charCode == 191)
            return "/";
        if (charCode == 192)
            return "`";
        if (charCode == 219)
            return "[";
        if (charCode == 220)
            return "\\";
        if (charCode == 221)
            return "]";
        if (charCode == 222)
            return "'";
        if (charCode == 32)
            return "space";
        return String.fromCharCode(charCode);
    }
    exports_7("keyCodeToString", keyCodeToString);
    function deserializeES6(obj) {
        if (Array.isArray(obj)) {
            for (var i = 0; i < obj.length; i++) {
                obj[i] = deserializeES6(obj[i]);
            }
        }
        else if (typeof obj === "object") {
            var className = obj.className;
            if (className) {
                var tempObj = Object.create(window[className].prototype);
                Object.assign(tempObj, obj);
                obj = tempObj;
            }
            if (obj.onDeserialize) {
                obj.onDeserialize();
            }
            for (var key in obj) {
                if (!obj.hasOwnProperty(key))
                    continue;
                obj[key] = deserializeES6(obj[key]);
            }
        }
        return obj;
    }
    exports_7("deserializeES6", deserializeES6);
    function serializeES6(obj) {
        var retStr = "";
        if (Array.isArray(obj)) {
            retStr += "[";
            for (var i = 0; i < obj.length; i++) {
                retStr += serializeES6(obj[i]) + ",";
            }
            if (retStr[retStr.length - 1] === ",")
                retStr = retStr.substring(0, retStr.length - 1);
            retStr += "]";
        }
        else if (typeof obj === "object") {
            retStr += "{";
            for (var key in obj) {
                if (key === "spritesheet")
                    continue;
                if (key === "background")
                    continue;
                if (key === "sprite")
                    continue;
                if (key === "nonSpriteImgEl")
                    continue;
                if (key === "obj")
                    continue;
                if (key === "propertiesJson")
                    continue;
                if (!obj.hasOwnProperty(key))
                    continue;
                retStr += '"' + key + '":' + serializeES6(obj[key]) + ",";
            }
            retStr += '"className":"' + obj.constructor.name + '"}';
        }
        else {
            if (obj === null || obj === undefined || obj === "") {
                retStr += '""';
            }
            else if (isNaN(obj)) {
                retStr = '"' + String(obj) + '"';
            }
            else {
                retStr = String(obj);
            }
        }
        return retStr;
    }
    exports_7("serializeES6", serializeES6);
    function get2DArrayFromImage(imageData) {
        var data = imageData.data;
        var arr = [];
        var row = [];
        for (var i = 0; i < data.length; i += 4) {
            if (i % (imageData.width * 4) === 0) {
                if (i > 0) {
                    arr.push(row);
                }
                row = [];
            }
            var red = data[i];
            var green = data[i + 1];
            var blue = data[i + 2];
            var alpha = data[i + 3];
            row.push(new PixelData(-1, -1, new color_1.Color(red, green, blue, alpha), []));
            if (i === data.length - 4) {
                arr.push(row);
            }
        }
        for (var i = 0; i < arr.length; i++) {
            for (var j = 0; j < arr[i].length; j++) {
                arr[i][j].x = j;
                arr[i][j].y = i;
            }
        }
        for (var i = 0; i < arr.length; i++) {
            for (var j = 0; j < arr[i].length; j++) {
                arr[i][j].neighbors.push(get2DArrayEl(arr, i - 1, j - 1));
                arr[i][j].neighbors.push(get2DArrayEl(arr, i - 1, j));
                arr[i][j].neighbors.push(get2DArrayEl(arr, i - 1, j + 1));
                arr[i][j].neighbors.push(get2DArrayEl(arr, i, j - 1));
                arr[i][j].neighbors.push(get2DArrayEl(arr, i, j));
                arr[i][j].neighbors.push(get2DArrayEl(arr, i, j + 1));
                arr[i][j].neighbors.push(get2DArrayEl(arr, i + 1, j - 1));
                arr[i][j].neighbors.push(get2DArrayEl(arr, i + 1, j));
                arr[i][j].neighbors.push(get2DArrayEl(arr, i + 1, j + 1));
                index_1.default.pull(arr[i][j].neighbors, null);
            }
        }
        return arr;
    }
    exports_7("get2DArrayFromImage", get2DArrayFromImage);
    function getPixelClumpRect(x, y, imageArr) {
        var selectedNode = imageArr[y][x];
        if (selectedNode.rgb.a === 0) {
            console.log("Clicked transparent pixel");
            return null;
        }
        var queue = [];
        queue.push(selectedNode);
        var minX = Infinity;
        var minY = Infinity;
        var maxX = -1;
        var maxY = -1;
        var num = 0;
        var visitedNodes = new Set();
        while (queue.length > 0) {
            var node = queue.shift();
            num++;
            if (node.x < minX)
                minX = node.x;
            if (node.y < minY)
                minY = node.y;
            if (node.x > maxX)
                maxX = node.x;
            if (node.y > maxY)
                maxY = node.y;
            try {
                for (var _a = __values(node.neighbors), _b = _a.next(); !_b.done; _b = _a.next()) {
                    var neighbor = _b.value;
                    if (visitedNodes.has(neighbor))
                        continue;
                    if (queue.indexOf(neighbor) === -1) {
                        queue.push(neighbor);
                    }
                }
            }
            catch (e_16_1) { e_16 = { error: e_16_1 }; }
            finally {
                try {
                    if (_b && !_b.done && (_c = _a.return)) _c.call(_a);
                }
                finally { if (e_16) throw e_16.error; }
            }
            visitedNodes.add(node);
        }
        return new rect_2.Rect(Math.round(minX), Math.round(minY), Math.round(maxX + 1), Math.round(maxY + 1));
        var e_16, _c;
    }
    exports_7("getPixelClumpRect", getPixelClumpRect);
    function getSelectedPixelRect(x, y, endX, endY, imageArr) {
        var minX = Infinity;
        var minY = Infinity;
        var maxX = -1;
        var maxY = -1;
        for (var i = y; i <= endY; i++) {
            for (var j = x; j <= endX; j++) {
                if (imageArr[i][j].rgb.a !== 0) {
                    if (i < minY)
                        minY = i;
                    if (i > maxY)
                        maxY = i;
                    if (j < minX)
                        minX = j;
                    if (j > maxX)
                        maxX = j;
                }
            }
        }
        if (!isFinite(minX) || !isFinite(minY) || maxX === -1 || maxY === -1)
            return;
        return new rect_2.Rect(Math.round(minX), Math.round(minY), Math.round(maxX + 1), Math.round(maxY + 1));
    }
    exports_7("getSelectedPixelRect", getSelectedPixelRect);
    function get2DArrayEl(arr, i, j) {
        if (i < 0 || i >= arr.length)
            return null;
        if (j < 0 || j >= arr[0].length)
            return null;
        if (arr[i][j].rgb.a === 0)
            return null;
        return arr[i][j];
    }
    exports_7("get2DArrayEl", get2DArrayEl);
    var rect_2, point_4, color_1, index_1, autoInc, helperCanvas, helperCtx, helperCanvas2, helperCtx2, helperCanvas3, helperCtx3, PixelData;
    return {
        setters: [
            function (rect_2_1) {
                rect_2 = rect_2_1;
            },
            function (point_4_1) {
                point_4 = point_4_1;
            },
            function (color_1_1) {
                color_1 = color_1_1;
            },
            function (index_1_1) {
                index_1 = index_1_1;
            }
        ],
        execute: function () {
            autoInc = 0;
            helperCanvas = document.createElement("canvas");
            helperCtx = helperCanvas.getContext("2d");
            noCanvasSmoothing(helperCtx);
            helperCanvas2 = document.createElement("canvas");
            helperCtx2 = helperCanvas2.getContext("2d");
            noCanvasSmoothing(helperCtx2);
            helperCanvas3 = document.createElement("canvas");
            helperCtx3 = helperCanvas3.getContext("2d");
            noCanvasSmoothing(helperCtx3);
            PixelData = (function () {
                function PixelData(x, y, rgb, neighbors) {
                    this.x = x;
                    this.y = y;
                    this.rgb = rgb;
                    this.neighbors = neighbors;
                }
                return PixelData;
            }());
        }
    };
});
System.register("point", ["helpers"], function (exports_8, context_8) {
    "use strict";
    var __moduleName = context_8 && context_8.id;
    var Helpers, Point;
    return {
        setters: [
            function (Helpers_1) {
                Helpers = Helpers_1;
            }
        ],
        execute: function () {
            Point = (function () {
                function Point(x, y) {
                    this.x = x;
                    this.y = y;
                }
                Object.defineProperty(Point.prototype, "ix", {
                    get: function () {
                        return Math.round(this.x);
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(Point.prototype, "iy", {
                    get: function () {
                        return Math.round(this.y);
                    },
                    enumerable: true,
                    configurable: true
                });
                Point.prototype.addxy = function (x, y) {
                    var point = new Point(this.x + x, this.y + y);
                    return point;
                };
                Point.prototype.normalize = function () {
                    this.x = Helpers.roundEpsilon(this.x);
                    this.y = Helpers.roundEpsilon(this.y);
                    if (this.x === 0 && this.y === 0)
                        return new Point(0, 0);
                    var mag = this.magnitude;
                    var point = new Point(this.x / mag, this.y / mag);
                    if (isNaN(point.x) || isNaN(point.y)) {
                        throw "NAN!";
                    }
                    point.x = Helpers.roundEpsilon(point.x);
                    point.y = Helpers.roundEpsilon(point.y);
                    return point;
                };
                Point.prototype.dotProduct = function (other) {
                    return (this.x * other.x) + (this.y * other.y);
                };
                Point.prototype.project = function (other) {
                    var dp = this.dotProduct(other);
                    return new Point((dp / (other.x * other.x + other.y * other.y)) * other.x, (dp / (other.x * other.x + other.y * other.y)) * other.y);
                };
                Point.prototype.rightNormal = function () {
                    return new Point(-this.y, this.x);
                };
                Point.prototype.leftNormal = function () {
                    return new Point(this.y, -this.x);
                };
                Point.prototype.perProduct = function (other) {
                    return this.dotProduct(other.rightNormal());
                };
                Point.prototype.add = function (other) {
                    var point = new Point(this.x + other.x, this.y + other.y);
                    return point;
                };
                Point.prototype.inc = function (other) {
                    this.x += other.x;
                    this.y += other.y;
                };
                Point.prototype.times = function (num) {
                    var point = new Point(this.x * num, this.y * num);
                    return point;
                };
                Point.prototype.multiply = function (num) {
                    this.x *= num;
                    this.y *= num;
                    return this;
                };
                Point.prototype.unitInc = function (num) {
                    return this.add(this.normalize().times(num));
                };
                Object.defineProperty(Point.prototype, "angle", {
                    get: function () {
                        var ang = Math.atan2(this.y, this.x);
                        ang *= 180 / Math.PI;
                        if (ang < 0)
                            ang += 360;
                        return ang;
                    },
                    enumerable: true,
                    configurable: true
                });
                Point.prototype.angleWith = function (other) {
                    var ang = Math.atan2(other.y, other.x) - Math.atan2(this.y, this.x);
                    ang *= 180 / Math.PI;
                    if (ang < 0)
                        ang += 360;
                    if (ang > 180)
                        ang = 360 - ang;
                    return ang;
                };
                Object.defineProperty(Point.prototype, "magnitude", {
                    get: function () {
                        var root = this.x * this.x + this.y * this.y;
                        if (root < 0)
                            root = 0;
                        var result = Math.sqrt(root);
                        if (isNaN(result))
                            throw "NAN!";
                        return result;
                    },
                    enumerable: true,
                    configurable: true
                });
                Point.prototype.clone = function () {
                    return new Point(this.x, this.y);
                };
                Point.prototype.distanceTo = function (other) {
                    return Math.sqrt(Math.pow(other.x - this.x, 2) + Math.pow(other.y - this.y, 2));
                };
                Point.prototype.subtract = function (other) {
                    return new Point(this.x - other.x, this.y - other.y);
                };
                Point.prototype.directionTo = function (other) {
                    return new Point(other.x - this.x, other.y - this.y);
                };
                Point.prototype.directionToNorm = function (other) {
                    return (new Point(other.x - this.x, other.y - this.y)).normalize();
                };
                Point.prototype.isAngled = function () {
                    return this.x !== 0 && this.y !== 0;
                };
                return Point;
            }());
            exports_8("Point", Point);
        }
    };
});
System.register("collider", ["point", "shape"], function (exports_9, context_9) {
    "use strict";
    var __moduleName = context_9 && context_9.id;
    var point_5, shape_2, Collider;
    return {
        setters: [
            function (point_5_1) {
                point_5 = point_5_1;
            },
            function (shape_2_1) {
                shape_2 = shape_2_1;
            }
        ],
        execute: function () {
            Collider = (function () {
                function Collider(points, isTrigger, isClimbable, isStatic, flag, offset) {
                    this.wallOnly = false;
                    this.isClimbable = true;
                    this.isStatic = false;
                    this.flag = 0;
                    this._shape = new shape_2.Shape(points);
                    this.isTrigger = isTrigger;
                    this.isClimbable = isClimbable;
                    this.isStatic = isStatic;
                    this.flag = flag;
                    this.offset = offset;
                }
                Object.defineProperty(Collider.prototype, "shape", {
                    get: function () {
                        var offset = new point_5.Point(0, 0);
                        return this._shape.clone(offset.x, offset.y);
                    },
                    enumerable: true,
                    configurable: true
                });
                return Collider;
            }());
            exports_9("Collider", Collider);
        }
    };
});
System.register("hitbox", ["point", "rect"], function (exports_10, context_10) {
    "use strict";
    var __moduleName = context_10 && context_10.id;
    var point_6, rect_3, Hitbox;
    return {
        setters: [
            function (point_6_1) {
                point_6 = point_6_1;
            },
            function (rect_3_1) {
                rect_3 = rect_3_1;
            }
        ],
        execute: function () {
            Hitbox = (function () {
                function Hitbox() {
                    this.tags = "";
                    this.width = 20;
                    this.height = 40;
                    this.offset = new point_6.Point(0, 0);
                }
                Hitbox.prototype.move = function (deltaX, deltaY) {
                    this.offset.x += deltaX;
                    this.offset.y += deltaY;
                };
                Hitbox.prototype.resizeCenter = function (w, h) {
                    this.width += w;
                    this.height += h;
                };
                Hitbox.prototype.getRect = function () {
                    return new rect_3.Rect(this.offset.x, this.offset.y, this.offset.x + this.width, this.offset.y + this.height);
                };
                return Hitbox;
            }());
            exports_10("Hitbox", Hitbox);
        }
    };
});
System.register("frame", [], function (exports_11, context_11) {
    "use strict";
    var __moduleName = context_11 && context_11.id;
    var Frame;
    return {
        setters: [],
        execute: function () {
            Frame = (function () {
                function Frame(rect, duration, offset) {
                    this.rect = rect;
                    this.duration = duration;
                    this.offset = offset;
                    this.hitboxes = [];
                    this.POIs = [];
                }
                Frame.prototype.getBusterOffset = function () {
                    if (this.POIs.length > 0)
                        return this.POIs[0];
                    return undefined;
                };
                return Frame;
            }());
            exports_11("Frame", Frame);
        }
    };
});
System.register("poi", [], function (exports_12, context_12) {
    "use strict";
    var __moduleName = context_12 && context_12.id;
    var POI;
    return {
        setters: [],
        execute: function () {
            POI = (function () {
                function POI(name, x, y) {
                    this.name = name;
                    this.x = x;
                    this.y = y;
                }
                return POI;
            }());
            exports_12("POI", POI);
        }
    };
});
System.register("selectable", [], function (exports_13, context_13) {
    "use strict";
    var __moduleName = context_13 && context_13.id;
    return {
        setters: [],
        execute: function () {
        }
    };
});
System.register("sprite", ["point", "helpers"], function (exports_14, context_14) {
    "use strict";
    var __moduleName = context_14 && context_14.id;
    var point_7, Helpers, Sprite;
    return {
        setters: [
            function (point_7_1) {
                point_7 = point_7_1;
            },
            function (Helpers_2) {
                Helpers = Helpers_2;
            }
        ],
        execute: function () {
            Sprite = (function () {
                function Sprite(name) {
                    this.freeForPool = false;
                    this.name = name;
                }
                Object.defineProperty(Sprite.prototype, "spritesheet", {
                    get: function () {
                        return PIXI.loader.resources[this.spritesheetPath].texture.baseTexture.source;
                    },
                    enumerable: true,
                    configurable: true
                });
                Sprite.prototype.getAnchor = function () {
                    var x, y;
                    if (this.alignment === "topleft") {
                        x = 0;
                        y = 0;
                    }
                    else if (this.alignment === "topmid") {
                        x = 0.5;
                        y = 0;
                    }
                    else if (this.alignment === "topright") {
                        x = 1;
                        y = 0;
                    }
                    else if (this.alignment === "midleft") {
                        x = 0;
                        y = 0.5;
                    }
                    else if (this.alignment === "center") {
                        x = 0.5;
                        y = 0.5;
                    }
                    else if (this.alignment === "midright") {
                        x = 1;
                        y = 0.5;
                    }
                    else if (this.alignment === "botleft") {
                        x = 0;
                        y = 1;
                    }
                    else if (this.alignment === "botmid") {
                        x = 0.5;
                        y = 1;
                    }
                    else if (this.alignment === "botright") {
                        x = 1;
                        y = 1;
                    }
                    return new point_7.Point(x, y);
                };
                Sprite.prototype.draw = function (ctx, frameIndex, x, y, flipX, flipY, options, alpha, scaleX, scaleY) {
                    flipX = flipX || 1;
                    flipY = flipY || 1;
                    var frame = this.frames[frameIndex];
                    var rect = frame.rect;
                    var offset = this.getAlignOffset(frameIndex, flipX, flipY);
                    Helpers.drawImage(ctx, this.spritesheet, rect.x1, rect.y1, rect.w, rect.h, x + offset.x, y + offset.y, flipX, flipY, options, alpha, scaleX, scaleY);
                };
                Sprite.prototype.getAlignOffset = function (frameIndex, flipX, flipY) {
                    var frame = this.frames[frameIndex];
                    var rect = frame.rect;
                    var offset = frame.offset;
                    return this.getAlignOffsetHelper(rect, offset, flipX, flipY);
                };
                Sprite.prototype.getAlignOffsetHelper = function (rect, offset, flipX, flipY) {
                    flipX = flipX || 1;
                    flipY = flipY || 1;
                    var w = rect.w;
                    var h = rect.h;
                    var halfW = w * 0.5;
                    var halfH = h * 0.5;
                    if (flipX > 0)
                        halfW = Math.floor(halfW);
                    else
                        halfW = Math.ceil(halfW);
                    if (flipY > 0)
                        halfH = Math.floor(halfH);
                    else
                        halfH = Math.ceil(halfH);
                    var x;
                    var y;
                    if (this.alignment === "topleft") {
                        x = 0;
                        y = 0;
                    }
                    else if (this.alignment === "topmid") {
                        x = -halfW;
                        y = 0;
                    }
                    else if (this.alignment === "topright") {
                        x = -w;
                        y = 0;
                    }
                    else if (this.alignment === "midleft") {
                        x = flipX === -1 ? -w : 0;
                        y = -halfH;
                    }
                    else if (this.alignment === "center") {
                        x = -halfW;
                        y = -halfH;
                    }
                    else if (this.alignment === "midright") {
                        x = flipX === -1 ? 0 : -w;
                        y = -halfH;
                    }
                    else if (this.alignment === "botleft") {
                        x = 0;
                        y = -h;
                    }
                    else if (this.alignment === "botmid") {
                        x = -halfW;
                        y = -h;
                    }
                    else if (this.alignment === "botright") {
                        x = -w;
                        y = -h;
                    }
                    return new point_7.Point(x + offset.x * flipX, y + offset.y * flipY);
                };
                return Sprite;
            }());
            exports_14("Sprite", Sprite);
        }
    };
});
System.register("spritesheet", [], function (exports_15, context_15) {
    "use strict";
    var __moduleName = context_15 && context_15.id;
    var Spritesheet;
    return {
        setters: [],
        execute: function () {
            Spritesheet = (function () {
                function Spritesheet(imgEl, path, imgArr) {
                    this.imgEl = imgEl;
                    this.path = path;
                    this.imgArr = imgArr;
                }
                return Spritesheet;
            }());
            exports_15("Spritesheet", Spritesheet);
        }
    };
});
System.register("spriteEditor", ["lodash", "sprite", "frame", "helpers", "hitbox", "rect", "point", "canvasUI"], function (exports_16, context_16) {
    "use strict";
    var __moduleName = context_16 && context_16.id;
    function init() {
        var data = new Data();
        var spriteCanvas = new canvasUI_1.CanvasUI("#canvas1");
        var spriteSheetCanvas = new canvasUI_1.CanvasUI("#canvas2");
        spriteCanvas.redraw = function () {
            var zoomScale = data.zoom;
            this.ctx.setTransform(zoomScale, 0, 0, zoomScale, -(zoomScale - 1) * this.ctx.width / 2, -(zoomScale - 1) * this.ctx.height / 2);
            this.ctx.clearRect(0, 0, this.canvasEl.width, this.canvasEl.height);
            Helpers.drawRect(this.ctx, new rect_4.Rect(0, 0, this.canvasEl.width, this.canvasEl.height), "lightgray", "", null);
            var frame = undefined;
            if (!data.isAnimPlaying) {
                if (data.selectedFrame && data.selectedSpritesheet && data.selectedSpritesheet.imgEl) {
                    frame = data.selectedFrame;
                }
            }
            else {
                frame = data.selectedSprite.frames[animFrameIndex];
            }
            var cX = this.canvasEl.width / 2;
            var cY = this.canvasEl.height / 2;
            var frameIndex = data.selectedSprite.frames.indexOf(frame);
            if (frame) {
                data.selectedSprite.draw(this.ctx, frameIndex, cX, cY, data.flipX ? -1 : 1, data.flipY ? -1 : 1);
                if (data.ghost) {
                    this.ctx.globalAlpha = 0.5;
                    data.ghost.sprite.draw(this.ctx, data.ghost.sprite.frames.indexOf(data.ghost.frame), cX, cY, data.flipX ? -1 : 1, data.flipY ? -1 : 1);
                    this.ctx.globalAlpha = 1;
                }
                if (!data.hideGizmos) {
                    try {
                        for (var _a = __values(getVisibleHitboxes()), _b = _a.next(); !_b.done; _b = _a.next()) {
                            var hitbox = _b.value;
                            var hx = void 0;
                            var hy = void 0;
                            var halfW = hitbox.width * 0.5;
                            var halfH = hitbox.height * 0.5;
                            var w = halfW * 2;
                            var h = halfH * 2;
                            if (data.selectedSprite.alignment === "topleft") {
                                hx = cX;
                                hy = cY;
                            }
                            else if (data.selectedSprite.alignment === "topmid") {
                                hx = cX - halfW;
                                hy = cY;
                            }
                            else if (data.selectedSprite.alignment === "topright") {
                                hx = cX - w;
                                hy = cY;
                            }
                            else if (data.selectedSprite.alignment === "midleft") {
                                hx = cX;
                                hy = cY - halfH;
                            }
                            else if (data.selectedSprite.alignment === "center") {
                                hx = cX - halfW;
                                hy = cY - halfH;
                            }
                            else if (data.selectedSprite.alignment === "midright") {
                                hx = cX - w;
                                hy = cY - halfH;
                            }
                            else if (data.selectedSprite.alignment === "botleft") {
                                hx = cX;
                                hy = cY - h;
                            }
                            else if (data.selectedSprite.alignment === "botmid") {
                                hx = cX - halfW;
                                hy = cY - h;
                            }
                            else if (data.selectedSprite.alignment === "botright") {
                                hx = cX - w;
                                hy = cY - h;
                            }
                            var offsetRect = new rect_4.Rect(hx + hitbox.offset.x, hy + hitbox.offset.y, hx + hitbox.width + hitbox.offset.x, hy + hitbox.height + hitbox.offset.y);
                            var strokeColor = void 0;
                            var strokeWidth = void 0;
                            if (data.selection === hitbox) {
                                strokeColor = "blue";
                                strokeWidth = 2;
                            }
                            Helpers.drawRect(this.ctx, offsetRect, "blue", strokeColor, strokeWidth, 0.25);
                        }
                    }
                    catch (e_17_1) { e_17 = { error: e_17_1 }; }
                    finally {
                        try {
                            if (_b && !_b.done && (_c = _a.return)) _c.call(_a);
                        }
                        finally { if (e_17) throw e_17.error; }
                    }
                    var len = 1000;
                    Helpers.drawLine(this.ctx, cX, cY - len, cX, cY + len, "red", 1);
                    Helpers.drawLine(this.ctx, cX - len, cY, cX + len, cY, "red", 1);
                    Helpers.drawCircle(this.ctx, cX, cY, 1, "red");
                }
            }
            var e_17, _c;
        };
        spriteCanvas.onMouseMove = function () {
        };
        spriteCanvas.onMouseDown = function (whichMouse) {
        };
        spriteCanvas.onMouseUp = function (whichMouse) {
        };
        spriteCanvas.onMouseWheel = function (delta) {
            data.zoom += delta;
            if (data.zoom < 1)
                data.zoom = 1;
            if (data.zoom > 5)
                data.zoom = 5;
            this.redraw();
            resetVue();
        };
        spriteCanvas.onMouseMove = function (deltaX, deltaY) {
            if (data.selection && this.mousedown) {
                data.selection.move(deltaX, deltaY);
            }
        };
        spriteCanvas.onMouseDown = function () {
            var found = false;
            try {
                for (var _a = __values(data.selectables), _b = _a.next(); !_b.done; _b = _a.next()) {
                    var selectable = _b.value;
                    if (Helpers.inRect(this.mouseX, this.mouseY, selectable.getRect())) {
                        data.selection = selectable;
                        found = true;
                    }
                }
            }
            catch (e_18_1) { e_18 = { error: e_18_1 }; }
            finally {
                try {
                    if (_b && !_b.done && (_c = _a.return)) _c.call(_a);
                }
                finally { if (e_18) throw e_18.error; }
            }
            if (!found)
                data.selection = null;
            this.redraw();
            var e_18, _c;
        };
        spriteCanvas.onKeyDown = function (keyCode, firstFrame) {
            if (keyCode === canvasUI_1.KeyCode.ESCAPE) {
                data.selection = null;
                data.ghost = null;
            }
            if (data.selectedFrame && !app1.isSelectedFrameAdded()) {
                if (keyCode === canvasUI_1.KeyCode.F) {
                    app1.addPendingFrame();
                }
            }
            if (data.selectedFrame) {
                if (keyCode === canvasUI_1.KeyCode.G) {
                    data.ghost = new Ghost(data.selectedSprite, data.selectedFrame);
                }
            }
            if (data.selection && firstFrame) {
                if (keyCode === canvasUI_1.KeyCode.A) {
                    data.selection.move(-1, 0);
                }
                else if (keyCode === canvasUI_1.KeyCode.D) {
                    data.selection.move(1, 0);
                }
                else if (keyCode === canvasUI_1.KeyCode.W) {
                    data.selection.move(0, -1);
                }
                else if (keyCode === canvasUI_1.KeyCode.S) {
                    data.selection.move(0, 1);
                }
                else if (keyCode === canvasUI_1.KeyCode.LEFT) {
                    data.selection.resizeCenter(-1, 0);
                }
                else if (keyCode === canvasUI_1.KeyCode.RIGHT) {
                    data.selection.resizeCenter(1, 0);
                }
                else if (keyCode === canvasUI_1.KeyCode.DOWN) {
                    data.selection.resizeCenter(0, -1);
                }
                else if (keyCode === canvasUI_1.KeyCode.UP) {
                    data.selection.resizeCenter(0, 1);
                }
            }
            else if (data.selectedFrame && firstFrame) {
                if (keyCode === canvasUI_1.KeyCode.A) {
                    data.selectedFrame.offset.x -= 1;
                }
                else if (keyCode === canvasUI_1.KeyCode.D) {
                    data.selectedFrame.offset.x += 1;
                }
                else if (keyCode === canvasUI_1.KeyCode.W) {
                    data.selectedFrame.offset.y -= 1;
                }
                else if (keyCode === canvasUI_1.KeyCode.S) {
                    data.selectedFrame.offset.y += 1;
                }
            }
            if (firstFrame) {
                if (keyCode === canvasUI_1.KeyCode.E) {
                    app1.selectNextFrame();
                }
                else if (keyCode === canvasUI_1.KeyCode.Q) {
                    app1.selectPrevFrame();
                }
            }
            this.redraw();
        };
        spriteSheetCanvas.onMouseDown = function (mouseButton) {
            if (mouseButton === canvasUI_1.MouseButton.LEFT) {
                if (data.selectedSprite === null)
                    return;
                try {
                    for (var _a = __values(data.selectedSprite.frames), _b = _a.next(); !_b.done; _b = _a.next()) {
                        var frame = _b.value;
                        if (Helpers.inRect(this.mouseX, this.mouseY, frame.rect)) {
                            data.selectedFrame = frame;
                            this.redraw();
                            return;
                        }
                    }
                }
                catch (e_19_1) { e_19 = { error: e_19_1 }; }
                finally {
                    try {
                        if (_b && !_b.done && (_c = _a.return)) _c.call(_a);
                    }
                    finally { if (e_19) throw e_19.error; }
                }
                if (data.selectedSpritesheet === null)
                    return;
                var rect = Helpers.getPixelClumpRect(this.mouseX, this.mouseY, data.selectedSpritesheet.imgArr);
                if (rect) {
                    data.selectedFrame = new frame_1.Frame(rect, 0.066, new point_8.Point(0, 0));
                    this.redraw();
                    spriteCanvas.redraw();
                }
                spriteCanvas.redraw();
                event.preventDefault();
            }
            var e_19, _c;
        };
        spriteSheetCanvas.onMouseUp = function (mouseButton) {
            if (mouseButton === canvasUI_1.MouseButton.LEFT) {
                getSelectedPixels();
            }
        };
        spriteSheetCanvas.onMouseMove = function () {
            if (this.mousedown) {
                this.redraw();
            }
        };
        spriteSheetCanvas.onMouseLeave = function () {
            this.redraw();
        };
        spriteSheetCanvas.redraw = function () {
            this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
            if (this.mousedown) {
                Helpers.drawRect(this.ctx, new rect_4.Rect(this.dragStartX, this.dragStartY, this.dragEndX, this.dragEndY), "", "blue", 1);
            }
            if (data.selectedSpritesheet && data.selectedSpritesheet.imgEl) {
                this.ctx.drawImage(data.selectedSpritesheet.imgEl, 0, 0);
            }
            if (data.selectedSprite) {
                var i = 0;
                try {
                    for (var _a = __values(data.selectedSprite.frames), _b = _a.next(); !_b.done; _b = _a.next()) {
                        var frame = _b.value;
                        Helpers.drawRect(this.ctx, frame.rect, "", "blue", 1);
                        Helpers.drawText(this.ctx, String(i + 1), frame.rect.x1, frame.rect.y1, "red", undefined, 12, "left", "Top", "Arial");
                        i++;
                    }
                }
                catch (e_20_1) { e_20 = { error: e_20_1 }; }
                finally {
                    try {
                        if (_b && !_b.done && (_c = _a.return)) _c.call(_a);
                    }
                    finally { if (e_20) throw e_20.error; }
                }
            }
            if (data.selectedFrame) {
                Helpers.drawRect(this.ctx, data.selectedFrame.rect, "", "green", 2);
            }
            var e_20, _c;
        };
        var methods = {
            onSpritesheetChange: function (newSheet, isNew) {
                var newSpriteAndSheetSel = isNew && (this.selectedSpritesheet !== undefined);
                if (newSpriteAndSheetSel) {
                    this.selectedSprite.spritesheet = this.selectedSpritesheet;
                    this.selectedSprite.spritesheetPath = this.selectedSpritesheet.path;
                    return;
                }
                this.selectedSpritesheet = newSheet;
                if (!this.selectedSpritesheet) {
                    spriteCanvas.redraw();
                    spriteSheetCanvas.redraw();
                    return;
                }
                if (this.selectedSprite) {
                    this.selectedSprite.spritesheet = newSheet;
                    this.selectedSprite.spritesheetPath = newSheet.path;
                }
                if (newSheet.imgEl) {
                    return;
                }
                var spritesheetImg = document.createElement("img");
                spritesheetImg.onload = function () {
                    spriteCanvas.canvasEl.width = spritesheetImg.width;
                    spriteCanvas.canvasEl.height = spritesheetImg.height;
                    spriteCanvas.ctx.drawImage(spritesheetImg, 0, 0);
                    var imageData = spriteCanvas.ctx.getImageData(0, 0, spriteCanvas.canvasEl.width, spriteCanvas.canvasEl.height);
                    newSheet.imgArr = Helpers.get2DArrayFromImage(imageData);
                    newSheet.imgEl = spritesheetImg;
                    spriteCanvas.redraw();
                    spriteSheetCanvas.redraw();
                };
                spritesheetImg.src = newSheet.path;
            },
            addSprite: function () {
                var spritename = prompt("Enter a sprite name");
                var newSprite = new sprite_1.Sprite(spritename);
                this.changeSprite(newSprite, true);
                this.sprites.push(newSprite);
                this.selectedFrame = null;
                this.selection = null;
                resetVue();
            },
            changeSprite: function (newSprite, isNew) {
                this.selectedSprite = newSprite;
                this.onSpritesheetChange(newSprite.spritesheet, isNew);
                this.selection = null;
                this.selectedFrame = this.selectedSprite.frames[0];
                spriteCanvas.redraw();
                spriteSheetCanvas.redraw();
            },
            addHitboxToSprite: function (sprite) {
                var hitbox = new hitbox_1.Hitbox();
                hitbox.width = this.selectedFrame.rect.w;
                hitbox.height = this.selectedFrame.rect.h;
                sprite.hitboxes.push(hitbox);
                this.selectHitbox(hitbox);
                spriteCanvas.redraw();
            },
            addHitboxToFrame: function (frame) {
                var hitbox = new hitbox_1.Hitbox();
                hitbox.width = this.selectedFrame.rect.w;
                hitbox.height = this.selectedFrame.rect.h;
                frame.hitboxes.push(hitbox);
                this.selectHitbox(hitbox);
                spriteCanvas.redraw();
            },
            selectHitbox: function (hitbox) {
                this.selection = hitbox;
                data.selection = hitbox;
                spriteCanvas.redraw();
            },
            deleteHitbox: function (hitboxArr, hitbox) {
                lodash_1.default.pull(hitboxArr, hitbox);
                resetVue();
                spriteCanvas.redraw();
            },
            isSelectedFrameAdded: function () {
                return lodash_1.default.includes(this.frames, this.selectedFrame);
            },
            addPendingFrame: function () {
                this.selectedSprite.frames.push(this.selectedFrame);
                spriteCanvas.redraw();
                spriteSheetCanvas.redraw();
            },
            selectFrame: function (frame) {
                this.selectedFrame = frame;
                spriteCanvas.redraw();
                spriteSheetCanvas.redraw();
                resetVue();
            },
            deleteFrame: function (frame) {
                lodash_1.default.pull(this.selectedSprite.frames, frame);
                this.selectedFrame = this.selectedSprite.frames[0];
                spriteCanvas.redraw();
                spriteSheetCanvas.redraw();
                resetVue();
            },
            selectNextFrame: function () {
                this.selection = null;
                var frameIndex = this.selectedSprite.frames.indexOf(this.selectedFrame);
                var selectedFrame = this.selectedSprite.frames[frameIndex + 1];
                if (!selectedFrame)
                    selectedFrame = this.selectedSprite.frames[0] || null;
                this.selectFrame(selectedFrame);
            },
            selectPrevFrame: function () {
                this.selection = null;
                var frameIndex = this.selectedSprite.frames.indexOf(this.selectedFrame);
                var selectedFrame = this.selectedSprite.frames[frameIndex - 1];
                if (!selectedFrame)
                    selectedFrame = this.selectedSprite.frames[this.selectedSprite.frames.length - 1] || null;
                this.selectFrame(selectedFrame);
            },
            playAnim: function () {
                this.isAnimPlaying = !this.isAnimPlaying;
            },
            saveSprite: function () {
                var jsonStr = Helpers.serializeES6(this.selectedSprite);
                $.post("save-sprite", JSON.parse(jsonStr)).then(function (response) {
                    console.log("Successfully saved sprite");
                }, function (error) {
                    console.log("Failed to save sprite");
                });
            },
            saveSprites: function () {
                var jsonStr = "[";
                try {
                    for (var _a = __values(this.sprites), _b = _a.next(); !_b.done; _b = _a.next()) {
                        var sprite = _b.value;
                        jsonStr += Helpers.serializeES6(sprite);
                        jsonStr += ",";
                    }
                }
                catch (e_21_1) { e_21 = { error: e_21_1 }; }
                finally {
                    try {
                        if (_b && !_b.done && (_c = _a.return)) _c.call(_a);
                    }
                    finally { if (e_21) throw e_21.error; }
                }
                if (jsonStr[jsonStr.length - 1] === ",")
                    jsonStr = jsonStr.slice(0, -1);
                jsonStr += "]";
                $.post("save-sprites", JSON.parse(jsonStr)).then(function (response) {
                    console.log("Successfully saved sprites");
                }, function (error) {
                    console.log("Failed to save sprites");
                });
                var e_21, _c;
            },
            onSpriteAlignmentChange: function () {
                spriteCanvas.redraw();
            },
            redraw: function () {
                spriteCanvas.redraw();
                spriteSheetCanvas.redraw();
            },
            onBulkDurationChange: function () {
                try {
                    for (var _a = __values(this.selectedSprite.frames), _b = _a.next(); !_b.done; _b = _a.next()) {
                        var frame = _b.value;
                        frame.duration = this.bulkDuration;
                    }
                }
                catch (e_22_1) { e_22 = { error: e_22_1 }; }
                finally {
                    try {
                        if (_b && !_b.done && (_c = _a.return)) _c.call(_a);
                    }
                    finally { if (e_22) throw e_22.error; }
                }
                resetVue();
                var e_22, _c;
            },
            onLoopStartChange: function () {
                resetVue();
            },
            onWrapModeChange: function () {
                spriteCanvas.redraw();
                resetVue();
            },
            reverseFrames: function () {
                lodash_1.default.reverse(this.selectedSprite.frames);
                spriteCanvas.redraw();
                resetVue();
            }
        };
        var computed = {
            displayZoom: {
                get: function () {
                    return this.zoom * 100;
                },
                set: function (value) {
                    this.zoom = value;
                }
            }
        };
        var app1 = new Vue({
            el: '#app1',
            data: data,
            computed: computed,
            methods: methods,
            created: function () {
                var _this = this;
                $.get("get-spritesheets").then(function (response) {
                    _this.spritesheets = lodash_1.default.map(response.data, function (spritesheet) {
                        return {
                            path: spritesheet
                        };
                    });
                    $.get("get-sprites").then(function (response) {
                        _this.sprites = Helpers.deserializeES6(response.data);
                    }, function (error) {
                        console.log("Error getting sprites");
                    });
                }, function (error) {
                    console.log("Error getting sprites");
                });
            }
        });
        var app2 = new Vue({
            el: '#app2',
            data: data,
            methods: methods,
            computed: computed
        });
        var app3 = new Vue({
            el: '#app3',
            data: data,
            methods: methods,
            computed: computed
        });
        function resetVue() {
            app1.$forceUpdate();
            app2.$forceUpdate();
            app3.$forceUpdate();
        }
        var animFrameIndex = 0;
        var animTime = 0;
        setInterval(mainLoop, 1000 / 60);
        function getSelectedPixels() {
            if (!data.selectedSpritesheet)
                return;
            var rect = Helpers.getSelectedPixelRect(spriteSheetCanvas.dragStartX, spriteSheetCanvas.dragStartY, spriteSheetCanvas.dragEndX, spriteSheetCanvas.dragEndY, data.selectedSpritesheet.imgArr);
            if (rect) {
                data.selectedFrame = new frame_1.Frame(rect, 0.066, new point_8.Point(0, 0));
                spriteCanvas.redraw();
                spriteSheetCanvas.redraw();
            }
        }
        function mainLoop() {
            if (data.isAnimPlaying) {
                animTime += 1000 / 60;
                var frames_1 = data.selectedSprite.frames;
                if (animTime >= frames_1[animFrameIndex].duration * 1000) {
                    animFrameIndex++;
                    if (animFrameIndex >= frames_1.length) {
                        animFrameIndex = 0;
                    }
                    animTime = 0;
                }
                spriteCanvas.redraw();
            }
        }
        function getVisibleHitboxes() {
            var hitboxes = [];
            if (data.selectedSprite) {
                hitboxes = hitboxes.concat(data.selectedSprite.hitboxes);
            }
            if (data.selectedFrame) {
                hitboxes = hitboxes.concat(data.selectedFrame.hitboxes);
            }
            return hitboxes;
        }
    }
    var lodash_1, sprite_1, frame_1, Helpers, hitbox_1, rect_4, point_8, canvasUI_1, Data, Ghost;
    return {
        setters: [
            function (lodash_1_1) {
                lodash_1 = lodash_1_1;
            },
            function (sprite_1_1) {
                sprite_1 = sprite_1_1;
            },
            function (frame_1_1) {
                frame_1 = frame_1_1;
            },
            function (Helpers_3) {
                Helpers = Helpers_3;
            },
            function (hitbox_1_1) {
                hitbox_1 = hitbox_1_1;
            },
            function (rect_4_1) {
                rect_4 = rect_4_1;
            },
            function (point_8_1) {
                point_8 = point_8_1;
            },
            function (canvasUI_1_1) {
                canvasUI_1 = canvasUI_1_1;
            }
        ],
        execute: function () {
            Data = (function () {
                function Data() {
                    this.sprites = undefined;
                    this.selectedSprite = undefined;
                    this.spritesheets = [];
                    this.selectedSpritesheet = undefined;
                    this.selection = undefined;
                    this.selectedFrame = undefined;
                    this.selectables = [];
                    this.isAnimPlaying = false;
                    this.addPOIMode = false;
                    this.alignments = ["topleft", "topmid", "topright", "midleft", "center", "midright", "botleft", "botmid", "botright"];
                    this.wrapModes = ["loop", "once", "pingpong", "pingpongonce"];
                    this.zoom = 5;
                    this.offsetX = 0;
                    this.offsetY = 0;
                    this.hideGizmos = false;
                    this.flipX = false;
                    this.flipY = false;
                    this.bulkDuration = 0;
                    this.newSpriteName = "";
                    this.selectedPOI = undefined;
                    this.ghost = undefined;
                }
                return Data;
            }());
            Ghost = (function () {
                function Ghost(sprite, frame) {
                    this.sprite = sprite;
                    this.frame = frame;
                }
                return Ghost;
            }());
        }
    };
});
//# sourceMappingURL=main.js.map