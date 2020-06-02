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

paths.dest = paths.webroot;
paths.dest_css = `${paths.dest}css/`;
paths.dest_js = `${paths.dest}js/`;

paths.src = "./";
paths.src_css = `${paths.src}css/**/*.css`;
paths.src_js = [`${paths.src}scripts/**/*.ts`, `${paths.src}scripts/**/*.js`, `${paths.src}scripts/**/*.map`];

function css() {
    return src(paths.src_css)
        .pipe(cssmin())
        .pipe(rename(function (path) {
            path.extname = ".min.css";
        }))
        .pipe(dest(paths.dest_css))
        .pipe(src(paths.src_css))
        .pipe(dest(paths.dest_css));
}

function js() {
    return src(paths.src_js)
        .pipe(dest(paths.dest_js));
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

    function focus_trap() {
        return src(`${paths.modules}focus-trap/dist/*`)
            .pipe(dest(`${paths.dest}focus-trap`))
    }

    function requirejs() {
        return src(`${paths.modules}requirejs/require.js`)
            .pipe(dest(`${paths.dest}requirejs`))
    }

    return parallel(font_awesome, focus_trap, requirejs)(done);
}

function clean(done) {
    del.sync([`${paths.dest}**`, `!${paths.dest}`]);
    done();
}

exports.build = parallel(css, js, modules);
exports.clean = clean;