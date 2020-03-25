import { CoeficienteDto } from 'src/app/dto/Produto/CoeficienteDto';
import { DadosPagtoComponent } from './dados-pagto.component';
import { PrepedidoBuscarService } from 'src/app/servicos/prepedido/prepedido-buscar.service';
import { NovoPrepedidoDadosService } from '../novo-prepedido-dados.service';
import { AlertaService } from 'src/app/utils/alert-dialog/alerta.service';
import { Constantes } from 'src/app/dto/Constantes';
import { ProdutosCalculados } from './tipo-forma-pagto';
import { MoedaUtils } from 'src/app/utils/moedaUtils';
import { getMatTooltipInvalidPositionError } from '@angular/material';

export class RecalcularComCoeficiente {

  constructor(public readonly prepedidoBuscarService: PrepedidoBuscarService,
    public readonly novoPrepedidoDadosService: NovoPrepedidoDadosService,
    public readonly alertaService: AlertaService) { }

  dadosPagto: DadosPagtoComponent;
  lstCoeficientesFornecdores: any[] = new Array();
  //vamos montar a lista de parcelamentos
  //irá retornar uma lista de string com a qtde de pareclas e o valor total
  public montarListaParcelamento() {


  }



  buscarCoeficienteFornecedores(callback: (coefciente: CoeficienteDto[][]) => void): void {
    const distinct = (value, index, self) => {
      return self.indexOf(value) === index;
    }
    let fornecedores: string[] = new Array();

    this.novoPrepedidoDadosService.prePedidoDto.ListaProdutos.forEach(element => {
      fornecedores.push(element.Fabricante);
    });

    let fornecDistinct = fornecedores.filter(distinct);

    let f: string[] = new Array();

    this.prepedidoBuscarService.buscarCoeficienteFornecedores(fornecDistinct).subscribe({
      next: (r: any[]) => {
        if (!!r) {
          callback(r);
        }
        else {
          this.alertaService.mostrarMensagem("Erro ao carregar a lista de coeficientes dos fabricantes")
        }
      },
      error: (r: CoeficienteDto) => this.alertaService.mostrarErroInternet(r)
    });

  }
  constantes = new Constantes();
  lstProdutosCalculados: ProdutosCalculados[];
  ProdutosCalculados: ProdutosCalculados;
  CalcularTotalProdutosComCoeficiente(enumFP: number, coeficienteDtoNovo: CoeficienteDto[][],
    tipoFormaPagto: string, qtdeParcVisa: number, vlEntrada: number): string[] {

    this.lstProdutosCalculados = new Array();
    this.ProdutosCalculados = new ProdutosCalculados();
    let lstMsg: string[] = new Array();
    let lstCoeficiente = coeficienteDtoNovo;
    let lstProdutos = this.novoPrepedidoDadosService.prePedidoDto.ListaProdutos;
    let alterouValorRA: boolean = false;

    let totalProduto = 0;

    let coeficienteFornec: CoeficienteDto[];
    //vamos calcular os produtos com os respectivos coeficientes e atribuir a uma variavel de total do prepedido

    let cont = 0;
    if (lstProdutos.length > 0) {
      lstCoeficiente.forEach(element => {
        lstProdutos.forEach(p => {

          if (!!tipoFormaPagto && !!enumFP) {

            //precisamos verificar se o valor de ra foi alterado para calcular as parcelas com base no valor digitado


            //A vista
            if (tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA &&
              enumFP.toString() == this.constantes.COD_FORMA_PAGTO_A_VISTA) {
              coeficienteFornec = element.filter(x => x.Fabricante == p.Fabricante);
              if (!!coeficienteFornec[0]) {
                if (coeficienteFornec[0].Fabricante == p.Fabricante) {
                  this.ProdutosCalculados = new ProdutosCalculados();

                  if (!!this.novoPrepedidoDadosService.prePedidoDto.PermiteRAStatus &&
                    this.novoPrepedidoDadosService.prePedidoDto.PermiteRAStatus == 1) {
                    p.VlTotalItem = p.AlterouValorRa && p.AlterouValorRa != undefined ? p.Preco_Lista * p.Qtde : p.Preco * p.Qtde;
                    this.ProdutosCalculados.QtdeParcela = 1;
                    this.ProdutosCalculados.Valor = p.VlTotalItem;
                    this.lstProdutosCalculados.push(this.ProdutosCalculados);
                  } else {
                    if (p.Desconto > 0) {
                      let totalproduto = p.Preco;
                      let desconto = (totalproduto - p.VlUnitario) * 100 / p.Preco;
                      p.VlTotalItem = (totalproduto - ((totalproduto * desconto) / 100)) * p.Qtde;
                      this.ProdutosCalculados.QtdeParcela = 1;
                      this.ProdutosCalculados.Valor = p.VlTotalItem;
                      this.lstProdutosCalculados.push(this.ProdutosCalculados);
                    } else {
                      p.VlTotalItem = p.Preco * p.Qtde;
                      this.ProdutosCalculados.QtdeParcela = 1;
                      this.ProdutosCalculados.Valor = p.VlTotalItem;
                      this.lstProdutosCalculados.push(this.ProdutosCalculados);
                    }
                  }

                  return;

                }
              }
            }

            //Parcela única
            if (tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA &&
              enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELA_UNICA) {
              totalProduto = (p.Preco * p.Qtde) * (1 - p.Desconto / 100);

              coeficienteFornec = element.filter(x => x.Fabricante == p.Fabricante &&
                x.TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA);

              if (!!coeficienteFornec[0]) {
                if (coeficienteFornec[0].Fabricante == p.Fabricante) {
                  this.ProdutosCalculados = new ProdutosCalculados();
                  if (!!this.novoPrepedidoDadosService.prePedidoDto.PermiteRAStatus &&
                    this.novoPrepedidoDadosService.prePedidoDto.PermiteRAStatus == 1) {
                    p.VlTotalItem = p.AlterouValorRa && p.AlterouValorRa != undefined ?
                      (p.Preco_Lista * p.Qtde) : (p.Preco * coeficienteFornec[0].Coeficiente) * p.Qtde;
                    this.ProdutosCalculados.QtdeParcela = coeficienteFornec[0].QtdeParcelas;
                    this.ProdutosCalculados.Valor = p.VlTotalItem;
                    this.lstProdutosCalculados.push(this.ProdutosCalculados);
                  }
                  else {
                    if (p.Desconto > 0) {
                      let totalproduto = p.Preco * coeficienteFornec[0].Coeficiente;
                      let desconto = (totalproduto - p.VlUnitario) * 100 /
                        (p.Preco * coeficienteFornec[0].Coeficiente);
                      p.VlTotalItem = (totalproduto - ((totalproduto * desconto) / 100)) * p.Qtde;
                      this.ProdutosCalculados.QtdeParcela = coeficienteFornec[0].QtdeParcelas;
                      this.ProdutosCalculados.Valor = p.VlTotalItem;
                      this.lstProdutosCalculados.push(this.ProdutosCalculados);
                    }
                    else {
                      p.VlTotalItem = (p.Preco * coeficienteFornec[0].Coeficiente) * p.Qtde;
                      this.ProdutosCalculados.QtdeParcela = coeficienteFornec[0].QtdeParcelas;
                      this.ProdutosCalculados.Valor = p.VlTotalItem;
                      this.lstProdutosCalculados.push(this.ProdutosCalculados);

                    }
                  }

                  return;
                }
              }
            }

            if (tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA &&
              enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA) {

              if (!!vlEntrada && vlEntrada != 0.00) {

                this.vlEntrada = vlEntrada;

                totalProduto = (p.Preco * p.Qtde) * (1 - p.Desconto / 100);

                coeficienteFornec = element.filter(x => x.Fabricante == p.Fabricante &&
                  x.TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA);

                coeficienteFornec.forEach(x => {
                  this.ProdutosCalculados = new ProdutosCalculados();
                  if (!!this.novoPrepedidoDadosService.prePedidoDto.PermiteRAStatus &&
                    this.novoPrepedidoDadosService.prePedidoDto.PermiteRAStatus == 1) {

                    p.VlTotalItem = p.AlterouValorRa && p.AlterouValorRa != undefined ? (p.Preco_Lista * p.Qtde) :
                      (p.Preco * x.Coeficiente) * p.Qtde;
                    this.ProdutosCalculados.QtdeParcela = x.QtdeParcelas;
                    this.ProdutosCalculados.Valor = p.VlTotalItem;
                    this.lstProdutosCalculados.push(this.ProdutosCalculados);
                  } else {
                    if (p.Desconto > 0) {
                      let totalproduto = p.Preco * x.Coeficiente;
                      let desconto = (totalproduto - p.VlUnitario) * 100 /
                        (p.Preco * x.Coeficiente);
                      p.VlTotalItem = (totalproduto - ((totalproduto * desconto) / 100)) * p.Qtde;
                      this.ProdutosCalculados.QtdeParcela = x.QtdeParcelas;
                      this.ProdutosCalculados.Valor = p.VlTotalItem;
                      this.lstProdutosCalculados.push(this.ProdutosCalculados);
                    }
                    else {
                      p.VlTotalItem = (p.Preco * x.Coeficiente) * p.Qtde;
                      this.ProdutosCalculados.QtdeParcela = x.QtdeParcelas;
                      this.ProdutosCalculados.Valor = p.VlTotalItem;
                      this.lstProdutosCalculados.push(this.ProdutosCalculados);

                    }
                  }


                });
              }
            }

            //cartão internet e maquineta
            if (tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA &&
              (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO ||
                enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)) {

              coeficienteFornec = element.filter(x => x.Fabricante == p.Fabricante &&
                x.TipoParcela == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA);

              coeficienteFornec.forEach(x => {
                this.ProdutosCalculados = new ProdutosCalculados();

                if (!!this.novoPrepedidoDadosService.prePedidoDto.PermiteRAStatus &&
                  this.novoPrepedidoDadosService.prePedidoDto.PermiteRAStatus == 1) {
                  p.VlTotalItem = p.AlterouValorRa && p.AlterouValorRa != undefined ? (p.Preco_Lista * p.Qtde) :
                    (p.Preco * x.Coeficiente) * p.Qtde;
                  this.ProdutosCalculados.QtdeParcela = x.QtdeParcelas;
                  this.ProdutosCalculados.Valor = p.VlTotalItem;
                  this.lstProdutosCalculados.push(this.ProdutosCalculados);
                } else {
                  if (p.Desconto > 0) {
                    let totalproduto = p.Preco * x.Coeficiente;
                    let desconto = (totalproduto - p.VlUnitario) * 100 /
                      (p.Preco * x.Coeficiente);
                    p.VlTotalItem = (totalproduto - ((totalproduto * desconto) / 100)) * p.Qtde;
                    this.ProdutosCalculados.QtdeParcela = x.QtdeParcelas;
                    this.ProdutosCalculados.Valor = p.VlTotalItem;
                    this.lstProdutosCalculados.push(this.ProdutosCalculados);
                  }
                  else {
                    p.VlTotalItem = (p.Preco * x.Coeficiente) * p.Qtde;
                    this.ProdutosCalculados.QtdeParcela = x.QtdeParcelas;
                    this.ProdutosCalculados.Valor = p.VlTotalItem;
                    this.lstProdutosCalculados.push(this.ProdutosCalculados);

                  }
                }


              });
            }
          }

        })
        //vamos verificar se é avista para sair do foreach do coeficiente que não é utilizado para este tipo
        if (!!enumFP)
          if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_A_VISTA) {
            return false;
          }

      })
    }
    else {
      this.alertaService.mostrarMensagem("Favor selecionar um Produto!");
      return;
    }
    //precisamos retornar a lista de string para mostrar no Parcelamento
    lstMsg = this.CalcularTotalOrcamento(qtdeParcVisa, enumFP);

    return lstMsg;
  }

  //valor de entrada
  vlEntrada: number;

  moedaUtils = new MoedaUtils();
  CalcularTotalOrcamento(qtdeParcVisa: number, enumFP: number): string[] {
    let lstMsg: string[] = new Array();
    if (!!enumFP) {
      if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_A_VISTA) {

        let filtrarParcela = this.lstProdutosCalculados.filter(x => x.QtdeParcela == 1);
        let valorTotalParc = filtrarParcela.reduce((sum, prod) => sum + prod.Valor, 0);
        lstMsg.push(this.moedaUtils.formatarMoedaComPrefixo(valorTotalParc));
        // "1 X " +
      }
      else {
        if (!!qtdeParcVisa) {

          for (let i = 0; i <= qtdeParcVisa; i++) {
            let filtrarParcela = this.lstProdutosCalculados.filter(x => x.QtdeParcela == i);

            let valorTotalParc = filtrarParcela.reduce((sum, prod) => sum + prod.Valor, 0);

            if (!!valorTotalParc) {
              if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA) {
                //vamos testar para saber se parcela com entrada
                if (!!this.vlEntrada) {
                  if (this.vlEntrada > valorTotalParc) {
                    this.alertaService.mostrarMensagem("Valor da entrada é maior que o total do Pré-pedido!");
                    this.vlEntrada = null;
                    lstMsg = new Array();
                    return;
                  }
                  else {
                    valorTotalParc = valorTotalParc - this.vlEntrada;
                  }
                }
              }

              lstMsg.push(i + " X " +
                this.moedaUtils.formatarMoedaComPrefixo(valorTotalParc / i));
            }
          }
        }
      }
    }
    return lstMsg;
  }

  //vamos alterar os valores dos produtos que estão no serviço
  RecalcularListaProdutos(enumFP: number, coeficienteDtoNovo: CoeficienteDto[][],
    tipoFormaPagto: string, qtdeParc: number): void {

    let coeficiente: CoeficienteDto[];
    //Aqui iremos verificar se
    if (!!enumFP) {
      if (this.novoPrepedidoDadosService.prePedidoDto.ListaProdutos.length > 0) {
        coeficienteDtoNovo.forEach(coef => {

          //vamos alterar diretamente no serviço
          this.novoPrepedidoDadosService.prePedidoDto.ListaProdutos.forEach(produto => {

            //vamos verificar se é pagto á vista
            if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_A_VISTA &&
              tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA) {

              if (produto.AlterouVlVenda) {
                //devemos alterar o valor de desconto
                //valor com coeficiente - valor venda * 100 / valor com coeficiente
                produto.Desconto = (produto.Preco - produto.VlUnitario) * 100 / produto.Preco;
                produto.TotalItem = (produto.Preco * produto.Qtde) * (1 - produto.Desconto / 100);
                produto.VlTotalItem = (produto.Preco * produto.Qtde) * (1 - produto.Desconto / 100);
                produto.VlLista = produto.Preco;//só altera se calcular coeficiente

              } else {
                produto.VlUnitario = produto.Preco;
                produto.VlTotalItem = (produto.Preco * produto.Qtde);
                produto.VlLista = produto.Preco;//só altera se calcular coeficiente
                produto.TotalItem = (produto.Preco * produto.Qtde);
              }
              if (!produto.AlterouValorRa || produto.AlterouValorRa == undefined) {
                produto.Preco_Lista = produto.Preco;
              }
            }

            //vamos testar se é pagto parcela única
            if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELA_UNICA &&
              tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {

              //vamos filtrar o coeficiente
              coeficiente = coef.filter(x => x.Fabricante == produto.Fabricante &&
                x.TipoParcela == tipoFormaPagto && x.QtdeParcelas == qtdeParc);

              coeficiente.forEach(x => {
                if (produto.AlterouVlVenda) {

                  produto.Desconto = ((produto.Preco * x.Coeficiente) - produto.VlUnitario) * 100 /
                    (produto.Preco * x.Coeficiente);
                  produto.TotalItem = produto.VlUnitario * produto.Qtde;
                  produto.VlLista = (produto.Preco * x.Coeficiente);//só altera se calcular coeficiente
                }
                else {
                  produto.VlUnitario = (produto.Preco * x.Coeficiente);
                  produto.VlTotalItem = (produto.Preco * x.Coeficiente);
                  produto.VlLista = (produto.Preco * x.Coeficiente);//só altera se calcular coeficiente
                  produto.TotalItem = ((produto.Preco * produto.Qtde) * x.Coeficiente);
                }
                if (!produto.AlterouValorRa || produto.AlterouValorRa == undefined) {
                  produto.Preco_Lista = (produto.Preco * x.Coeficiente);
                }

              });
            }

            //vamos testar se é pagto com entrada 
            if (enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA &&
              tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA) {

              coeficiente = coef.filter(x => x.Fabricante == produto.Fabricante &&
                x.TipoParcela == tipoFormaPagto && x.QtdeParcelas == qtdeParc);

              coeficiente.forEach(x => {
                if (produto.AlterouVlVenda) {

                  produto.Desconto = ((produto.Preco * x.Coeficiente) - produto.VlUnitario) * 100 /
                    (produto.Preco * x.Coeficiente);
                  produto.TotalItem = produto.VlUnitario * produto.Qtde;
                  produto.VlLista = (produto.Preco * x.Coeficiente);//só altera se calcular coeficiente
                }
                else {
                  produto.VlUnitario = (produto.Preco * x.Coeficiente);
                  produto.VlTotalItem = (produto.Preco * x.Coeficiente);
                  produto.VlLista = (produto.Preco * x.Coeficiente);//só altera se calcular coeficiente
                  produto.TotalItem = ((produto.Preco * produto.Qtde) * x.Coeficiente);
                }
                if (!produto.AlterouValorRa || produto.AlterouValorRa == undefined) {
                  produto.Preco_Lista = (produto.Preco * x.Coeficiente);
                }

              });
            }

            //vamos testar se é pagto com Cartão e Maquininha
            if ((enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO ||
              enumFP.toString() == this.constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA) &&
              tipoFormaPagto == this.constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA) {


              //preciso pegar a qtde de parcelas que foi selecionado

              coeficiente = coef.filter(x => x.Fabricante == produto.Fabricante &&
                x.TipoParcela == tipoFormaPagto && x.QtdeParcelas == qtdeParc);

              coeficiente.forEach(x => {
                if (produto.AlterouVlVenda) {

                  produto.Desconto = ((produto.Preco * x.Coeficiente) - produto.VlUnitario) * 100 /
                    (produto.Preco * x.Coeficiente);
                  produto.TotalItem = produto.VlUnitario * produto.Qtde;
                  produto.VlLista = (produto.Preco * x.Coeficiente);//só altera se calcular coeficiente
                }
                else {
                  produto.VlUnitario = (produto.Preco * x.Coeficiente);
                  produto.VlTotalItem = (produto.Preco * x.Coeficiente);
                  produto.VlLista = (produto.Preco * x.Coeficiente);//só altera se calcular coeficiente
                  produto.TotalItem = ((produto.Preco * produto.Qtde) * x.Coeficiente);
                }
                if (!produto.AlterouValorRa || produto.AlterouValorRa == undefined) {
                  produto.Preco_Lista = (produto.Preco * x.Coeficiente);
                }

              });
            }
          });
        });
      }
    }
  }

}