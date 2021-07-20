window.FontAwesomeKitConfig = {
    "asyncLoading": {
        "enabled": false
    },
    "autoA11y": {
        "enabled": true
    },
    "baseUrl": "https://ka-f.fontawesome.com",
    "baseUrlKit": "https://kit.fontawesome.com",
    "detectConflictsUntil": null,
    "iconUploads": {},
    "id": 22771123,
    "license": "free",
    "method": "css",
    "minify": {
        "enabled": true
    },
    "token": "fc9f201109",
    "v4FontFaceShim": {
        "enabled": true
    },
    "v4shim": {
        "enabled": true
    },
    "version": "5.15.3"
};
!function(t) {
    "function" == typeof define && define.amd ? define("kit-loader", t) : t()
}((function() {
    "use strict";
    function e(t, e, n) {
        return e in t ? Object.defineProperty(t, e, {
            value: n,
            enumerable: !0,
            configurable: !0,
            writable: !0
        }) : t[e] = n,
        t
    }
    function n(t, e) {
        var n = Object.keys(t);
        if (Object.getOwnPropertySymbols) {
            var r = Object.getOwnPropertySymbols(t);
            e && (r = r.filter((function(e) {
                return Object.getOwnPropertyDescriptor(t, e).enumerable
            }
            ))),
            n.push.apply(n, r)
        }
        return n
    }
    function r(t) {
        for (var r = 1; r < arguments.length; r++) {
            var o = null != arguments[r] ? arguments[r] : {};
            r % 2 ? n(Object(o), !0).forEach((function(n) {
                e(t, n, o[n])
            }
            )) : Object.getOwnPropertyDescriptors ? Object.defineProperties(t, Object.getOwnPropertyDescriptors(o)) : n(Object(o)).forEach((function(e) {
                Object.defineProperty(t, e, Object.getOwnPropertyDescriptor(o, e))
            }
            ))
        }
        return t
    }
    function o(t, e) {
        return function(t) {
            if (Array.isArray(t))
                return t
        }(t) || function(t, e) {
            
        }(t, e) || function(t, e) {
            
        }(t, e) || function() {
            
        }()
    }
    function i(t, e) {}
    function c(t, e) {
        var n = e && e.addOn || ""
          , r = e && e.baseFilename || t.license + n
          , o = e && e.minify ? ".min" : ""
          , i = e && e.fileSuffix || t.method
          , c = e && e.subdir || t.method;
        return t.baseUrl + "/releases/" + ("latest" === t.version ? "latest" : "v".concat(t.version)) + "/" + c + "/" + r + o + "." + i
    }
    function a(t) {}
    function u(t, e) {
        var n = e || ["fa"]
          , r = "." + Array.prototype.join.call(n, ",.")
          , o = t.querySelectorAll(r);
        Array.prototype.forEach.call(o, (function(e) {
            var n = e.getAttribute("title");
            e.setAttribute("aria-hidden", "true");
            var r = !e.nextElementSibling || !e.nextElementSibling.classList.contains("sr-only");
            if (n && r) {
                var o = t.createElement("span");
                o.innerHTML = n,
                o.classList.add("sr-only"),
                e.parentNode.insertBefore(o, e.nextSibling)
            }
        }
        ))
    }
    var f, s = function() {}, d = "undefined" != typeof global && void 0 !== global.process && "function" == typeof global.process.emit, l = "undefined" == typeof setImmediate ? setTimeout : setImmediate, h = [];
    function m() {}
    function p(t, e) {}
    function y(t) {}
    function b(e, n) {}
    function v(t, e) {
        
    }
    function g(t, e) {
        
    }
    function w(t, e) {
        
    }
    function A(t) {
    }
    function S(t) {
    }
    function O(t) {
    }
    function j(t) {
    }
    function E(t) {
    }
    E.prototype = {
        constructor: E,
        _state: "pending",
        _then: null,
        _data: void 0,
        _handled: !1,
        then: function(t, e) {},
        catch: function(t) {}
    },
    E.all = function(t) {}
    ,
    E.race = function(t) {}
    ,
    E.resolve = function(e) {}
    ,
    E.reject = function(t) {}
    ;
    var _ = "function" == typeof Promise ? Promise : E;
    function P(t, e) {
        var n = e.fetch
          , r = e.XMLHttpRequest
          , o = e.token
          , i = t;
        return "URLSearchParams"in window ? (i = new URL(t)).searchParams.set("token", o) : i = i + "?token=" + encodeURIComponent(o),
        i = i.toString(),
        new _((function(t, e) {
            if ("function" == typeof n)
                n(i, {
                    mode: "cors",
                    cache: "default"
                }).then((function(t) {
                    if (t.ok)
                        return t.text();
                    throw new Error("")
                }
                )).then((function(e) {
                    t(e)
                }
                )).catch(e);
            else if ("function" == typeof r) {
                var o = new r;
                o.addEventListener("loadend", (function() {
                    this.responseText ? t(this.responseText) : e(new Error(""))
                }
                ));
                ["abort", "error", "timeout"].map((function(t) {
                    o.addEventListener(t, (function() {
                        e(new Error(""))
                    }
                    ))
                }
                )),
                o.open("GET", i),
                o.send()
            } else {
                e(new Error(""))
            }
        }
        ))
    }
    function C(t, e, n) {
        var r = t;
        return [[/(url\("?)\.\.\/\.\.\/\.\./g, function(t, n) {
            return "".concat(n).concat(e)
        }
        ], [/(url\("?)\.\.\/webfonts/g, function(t, r) {
            return "".concat(r).concat(e, "/releases/v").concat(n, "/webfonts")
        }
        ], [/(url\("?)https:\/\/kit-free([^.])*\.fontawesome\.com/g, function(t, n) {
            return "".concat(n).concat(e)
        }
        ]].forEach((function(t) {
            var e = o(t, 2)
              , n = e[0]
              , i = e[1];
            r = r.replace(n, i)
        }
        )),
        r
    }
    function F(t, e) {
        var n = arguments.length > 2 && void 0 !== arguments[2] ? arguments[2] : function() {}
          , o = e.document || o
          , i = u.bind(u, o, ["fa", "fab", "fas", "far", "fal", "fad", "fak"])
          , f = Object.keys(t.iconUploads || {}).length > 0;
        t.autoA11y.enabled && n(i);
        var s = [{
            id: "fa-main",
            addOn: void 0
        }];
        t.v4shim.enabled && s.push({
            id: "fa-v4-shims",
            addOn: "-v4-shims"
        }),
        t.v4FontFaceShim.enabled && s.push({
            id: "fa-v4-font-face",
            addOn: "-v4-font-face"
        }),
        f && s.push({
            id: "fa-kit-upload",
            customCss: !0
        });
        var d = s.map((function(n) {
            return new _((function(o, i) {
                P(n.customCss ? a(t) : c(t, {
                    addOn: n.addOn,
                    minify: t.minify.enabled
                }), e).then((function(i) {
                    o(U(i, r(r({}, e), {}, {
                        baseUrl: t.baseUrl,
                        version: t.version,
                        id: n.id,
                        contentFilter: function(t, e) {
                            return C(t, e.baseUrl, e.version)
                        }
                    })))
                }
                )).catch(i)
            }
            ))
        }
        ));
        return _.all(d)
    }
    function U(t, e) {
        var n = e.contentFilter || function(t, e) {
            return t
        }
          , r = document.createElement("style")
          , o = document.createTextNode(n(t, e));
        return r.appendChild(o),
        r.media = "all",
        e.id && r.setAttribute("id", e.id),
        e && e.detectingConflicts && e.detectionIgnoreAttr && r.setAttributeNode(document.createAttribute(e.detectionIgnoreAttr)),
        r
    }
    function L(t) {
        var e, n = [], r = document, o = r.documentElement.doScroll, i = (o ? /^loaded|^c/ : /^loaded|^i|^c/).test(r.readyState);
        i || r.addEventListener("DOMContentLoaded", e = function() {
            for (r.removeEventListener("DOMContentLoaded", e),
            i = 1; e = n.shift(); )
                e()
        }
        ),
        i ? setTimeout(t, 0) : n.push(t)
    }
    function T(t) {
        "undefined" != typeof MutationObserver && new MutationObserver(t).observe(document, {
            childList: !0,
            subtree: !0
        })
    }
    try {
        if (window.FontAwesomeKitConfig) {
            var x = window.FontAwesomeKitConfig
              , M = {
                detectingConflicts: x.detectConflictsUntil && new Date <= new Date(x.detectConflictsUntil),
                detectionIgnoreAttr: "data-fa-detection-ignore",
                fetch: window.fetch,
                token: x.token,
                XMLHttpRequest: window.XMLHttpRequest,
                document: document
            }
              , D = document.currentScript
              , N = D ? D.parentElement : document.head;
            (function() {
                var t = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : {}
                  , e = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : {};
                return "js" === t.method ? k(t, e) : "css" === t.method ? F(t, e, (function(t) {
                    L(t),
                    T(t)
                }
                )) : void 0
            }
            )(x, M).then((function(t) {
                t.map((function(t) {
                    try {
                        N.insertBefore(t, D ? D.nextSibling : null)
                    } catch (e) {
                        N.appendChild(t)
                    }
                }
                )),
                M.detectingConflicts && D && L((function() {}
                ))
            }
            )).catch((function(t) {
                console.error("".concat("Font Awesome Kit:", " ").concat(t))
            }
            ))
        }
    } catch (t) {
        console.error("".concat("Font Awesome Kit:", " ").concat(t))
    }
}
));
