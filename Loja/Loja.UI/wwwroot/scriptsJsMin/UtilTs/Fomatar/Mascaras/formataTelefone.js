define(["require","exports","../../stringUtils/stringUtils"],function(t,e,n){"use strict";Object.defineProperty(e,"__esModule",{value:!0}),e.FormatarTelefone=void 0;var r=function(){function t(){}return t.telefone_formata=function(e){var r="";if(r=""+e,r=n.StringUtils.retorna_so_digitos(r),!t.telefone_ok(r))return"";var o=r.length-4;return r=r.substr(0,o)+"-"+r.substr(o)},t.telefone_ddd_formata=function(e,n){var r="";return""!=n.trim()&&(r="("+n.trim()+") "+r),r+t.telefone_formata(e)},t.telefone_ok=function(t){return 0==(t=n.StringUtils.retorna_so_digitos(t)).length||t.length>=6},t.ddd_ok=function(t){var e=""+t;return 0==(e=n.StringUtils.retorna_so_digitos(e)).length||2==e.length},t.mascaraTelefone=function(t){return n.StringUtils.retorna_so_digitos(t).length>10?["(",/\d/,/\d/,")"," ",/\d/,/\d/,/\d/,/\d/,/\d/,"-",/\d/,/\d/,/\d/,/\d/]:["(",/\d/,/\d/,")"," ",/\d/,/\d/,/\d/,/\d/,"-",/\d/,/\d/,/\d/,/\d/]},t.SepararTelefone=function(t){var e=n.StringUtils.retorna_so_digitos(t),r=new o;return r.Ddd=e.substr(0,2),r.Telefone="",e.length>2&&(r.Telefone=e.substr(2)),r},t}();e.FormatarTelefone=r;var o=function(){return function(){}}()});