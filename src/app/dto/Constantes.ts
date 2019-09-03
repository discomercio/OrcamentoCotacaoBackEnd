export class Constantes {
	//' CÓDIGOS QUE IDENTIFICAM SE É PESSOA FÍSICA OU JURÍDICA
	public ID_PF = "PF";
	public ID_PJ = "PJ"

	//'	NÚMERO DE LINHAS DO CAMPO "OBS I" DO PEDIDO
	public MAX_LINHAS_OBS1 = 5;

	//'   NÚMERO DE LINHAS DO CAMPO "TEXTO CONSTAR NF" DO PEDIDO
	public MAX_LINHAS_NF_TEXTO_CONSTAR = 2;

	//' CÓDIGOS P/ ENTREGA IMEDIATA
	public COD_ETG_IMEDIATA_ST_INICIAL = 0;
	public COD_ETG_IMEDIATA_NAO = 1;
	public COD_ETG_IMEDIATA_SIM = 2;
	public COD_ETG_IMEDIATA_NAO_DEFINIDO = 10;  //' PEDIDOS ANTIGOS QUE JÁ ESTAVAM NA BASE

	// ' CÓDIGOS P/ FLAG "BEM DE USO/CONSUMO"
	public COD_ST_BEM_USO_CONSUMO_NAO = 0;
	public COD_ST_BEM_USO_CONSUMO_SIM = 1;
	public COD_ST_BEM_USO_CONSUMO_NAO_DEFINIDO = 10;//  ' PEDIDOS ANTIGOS QUE JÁ ESTAVAM NA BASE


	// '	CÓDIGOS PARA O CAMPO "INSTALADOR INSTALA"
	public COD_INSTALADOR_INSTALA_NAO_DEFINIDO = 0;
	public COD_INSTALADOR_INSTALA_NAO = 1;
	public COD_INSTALADOR_INSTALA_SIM = 2;

	//' CÓDIGOS P/ STATUS QUE INDICA SE CLIENTE É OU NÃO CONTRIBUINTE DO ICMS
	public COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL = 0;
	public COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO = 1;
	public COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM = 2;
	public COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO = 3;

	//' CÓDIGOS P/ STATUS QUE INDICA SE CLIENTE É OU NÃO PRODUTOR RURAL
	public COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL = 0;
	public COD_ST_CLIENTE_PRODUTOR_RURAL_NAO = 1;
	public COD_ST_CLIENTE_PRODUTOR_RURAL_SIM = 2;


}

