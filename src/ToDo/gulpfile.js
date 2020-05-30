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
paths.dest_scripts = `${paths.dest}scripts/`;

paths.src = "./";
paths.src_css = `${paths.src}css/**/*.css`;
paths.src_scripts = [`${paths.src}scripts/**/*.ts`, `${paths.src}scripts/**/*.js`, `${paths.src}scripts/**/*.map`];

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

function scripts() {
    return src(paths.src_scripts)
        .pipe(dest(paths.dest_scripts));
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

function clean(done) {
    del.sync([`${paths.dest}**`, `!${paths.dest}`]);
    done();
}

exports.build = parallel(css, scripts, modules);
exports.clean = clean;