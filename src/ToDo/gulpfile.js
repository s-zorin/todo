/// <binding BeforeBuild='build' Clean='clean' />
"use strict";

const { src } = require("gulp");
const { dest } = require("gulp");
const { parallel } = require("gulp");
const { series } = require("gulp");
const cssmin = require("gulp-cssmin");
const rename = require("gulp-rename");
const del = require("del");

var paths = {
    webroot: "./wwwroot/",
    modules: "./node_modules/"
};

paths.dest = paths.webroot + "dest/";
paths.src = paths.webroot + "src/";

function css() {
    return src(`${paths.src}**/*.css`)
        .pipe(cssmin())
        .pipe(rename(function (path) {
            path.extname = ".min.css";
        }))
        .pipe(dest(paths.dest))
        .pipe(src(`${paths.src}**/*.css`))
        .pipe(dest(paths.dest));
}

function modules(done) {
    function font_awesome(done) {
        function css() {
            return src(`${paths.modules}@fortawesome/fontawesome-free/css/*`)
                .pipe(dest(`${paths.dest}font-awesome/css`))
        }

        function fonts() {
            return src(`${paths.modules}@fortawesome/fontawesome-free/webfonts/*`)
                .pipe(dest(`${paths.dest}font-awesome/webfonts`))
        }

        return parallel(css, fonts)(done);
    }

    return parallel(font_awesome)(done);
}

function clean() {
    return del(paths.dest);
}

exports.build = parallel(css, modules);
exports.clean = clean;