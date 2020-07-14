/// <binding AfterBuild='minify' />
var gulp = require('gulp');
let uglifyjs = require('uglify-es');
//var concat = require('gulp-concat');
//var rimraf = require("rimraf");
//var merge = require('merge-stream');
let inject = require('gulp-inject-string');

var composer = require('gulp-uglify/composer');
var pump = require('pump');

var minify = composer(uglifyjs, console);

let entrada = ["wwwroot/scriptsJs/**/*.js"];
let entradaDiretorio = ["wwwroot/scriptsJs"];
let saida = "wwwroot/scriptsJsMin";

function marcarJsNaoMinified() {

    pump([
        gulp.src(entrada),
        inject.prepend('/* nao editar, arquivo compilado pelo typescript*/ \n'),
        gulp.dest(entradaDiretorio)
    ]);
}

gulp.task('minify', function (cb) {
    var options = {};

    marcarJsNaoMinified();
    pump([
        gulp.src(entrada),
        minify(options),
        gulp.dest(saida)
    ],
        cb
    );
});