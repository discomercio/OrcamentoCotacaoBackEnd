/*
objetivo: fazer um busca e troca em um arquivo

parametros:
arquivo: arquivo onde substituir
busca: string de busca
arquivoTroca: arquivo com a string pela qual trocar
*/

var myArgs = process.argv.slice(2);
if (myArgs.length != 3) {
	console.log("Utilização: node CompilarPrepedidoTrocarVersaoApi.js arquivo busca arquivoTroca ");
	process.exit(1);
	return;
}

var arquivo = myArgs[0];
var busca = myArgs[1];
var arquivoTroca = myArgs[2];

var fs = require('fs');
fs.readFile(arquivoTroca, 'utf8', function (err, data) {

	//precismaos tirar os enters
	var textoTroca = data;
	textoTroca = textoTroca.replace('\r',"");
	textoTroca = textoTroca.replace('\n',"");
	fs.readFile(arquivo, 'utf8', function (err, data) {
		if (err) {
			console.log(err);
			process.exit(1);
			return;
		}
		var result = data.replace(busca, textoTroca);
		if (result == data) {
			console.log('Nenhuma troca realizada!');
			process.exit(1);
			return;
		}

		fs.writeFile(arquivo, result, 'utf8', function (err) {
			if (err) {
				console.log(err);
				process.exit(1);
				return;
			}
			console.log('Troca realizada');

		});
	});


});