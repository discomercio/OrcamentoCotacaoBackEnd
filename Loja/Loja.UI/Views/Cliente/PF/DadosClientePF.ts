import { CpfCnpjUtils } from "../../../UtilTs/CpfCnpjUtils/CpfCnpjUtils";
import { Constantes } from "../../../UtilTs/Constantes/Constantes";


declare var window: any;

let cpfCnpj = $('#cpf_cnpj').val().toString();
$('#cpf_cnpj').val(CpfCnpjUtils.cnpj_cpf_formata(cpfCnpj));
$("#cpf_cnpj").prop("readonly", true);
$(function () {
    ($("#nascimento") as any).mask("00/00/0000");
    //verificar produtor rural no inicio da tela
    if ($("#tipo").val() == Constantes.ID_PF) {
        if (Number($("#produtor").val()) == Constantes.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) {
            $("#div_ie").css('visibility', 'visible');
            $("#div_contribuinte").css('visibility', 'visible');
        }
    }
});
/* ========== CHAMADAS DIRETAS DA TELA ====================================*/
window.AjustaData = (el: JQuery<HTMLInputElement>) => {
    filtra_data();
    if (tem_info((el.val() as string)) && !IsDate(el)) {
        swal("Erro", "Data inválida!");

        $("#nascimento").val("");
        return false;
    }
}
/* ============ FIM DE CHAMADAS DIRETAS DA TELA ===========*/
/* ============ FUNÇÕES ===========*/
function filtra_data(): void {
    let letra: string;
    letra = String.fromCharCode(window.event.keyCode);
    if (!isDigit(letra) && (window.event.keyCode != 47) && (window.event.keyCode != 8) && (window.event.keyCode != 13)) {
        window.event.keyCode = 0;
    }
}
function isDigit(d: string): boolean {
    return ((d >= '0') && (d <= '9'))
}
function tem_info(texto: string): boolean {
    let s: string;
    s = "" + texto;
    s = s.trim();
    if (s.length > 0) return true;
    return false;
}
function IsDate(el: JQuery<HTMLInputElement>): boolean {
    let val: string, s: string;

    //d.value = trim(d.value);
    //val = d.value;
    val = (el.val() as string).trim();
    if (val == "") return true;

    /* SEPARADOR */
    let sep1: number = val.indexOf("/");
    let sep2: number = val.indexOf("/", sep1 + 1);
    if ((val.length == 6) && (sep1 == -1) && (sep2 == -1)) {
        val = val.substr(0, 2) + "/" + val.substr(2, 2) + "/" + val.substr(4, 2);
        sep1 = val.indexOf("/");
        sep2 = val.indexOf("/", sep1 + 1);
    }
    if ((val.length == 8) && (sep1 == -1) && (sep2 == -1)) {
        val = val.substr(0, 2) + "/" + val.substr(2, 2) + "/" + val.substr(4, 4);
        sep1 = val.indexOf("/");
        sep2 = val.indexOf("/", sep1 + 1);
    }

    let len: number = val.length;

    s = val.substr(0, sep1);
    if (s.length == 0) return false;
    let dd: number = parseInt(s, 10);

    s = val.substr(sep1 + 1, sep2 - sep1 - 1);
    if (s.length == 0) return false;
    let mm: number = parseInt(s, 10);

    s = val.substr(sep2 + 1, len - sep2 - 1);
    if (s.length == 0) return false;
    let yy: number = parseInt(s, 10);

    /* ANO */
    if (yy <= 90) yy += 2000;
    if ((yy > 90) && (yy < 100)) yy += 1900;
    if ((yy < 1900) || (yy > 2099)) {
        return false;
    }

    let leap: boolean = ((yy == (yy / 4 * 4)) && !(yy == (yy / 100 * 100)));

    /* MES */
    if (!((mm >= 1) && (mm <= 12))) {
        return false;
    }

    /* DIA */
    let dom: number;
    if ((mm == 2) && (leap)) dom = 29;
    if ((mm == 2) && !(leap)) dom = 28;
    if ((mm == 1) || (mm == 3) || (mm == 5) || (mm == 7) || (mm == 8) || (mm == 10) || (mm == 12)) dom = 31;
    if ((mm == 4) || (mm == 6) || (mm == 9) || (mm == 11)) dom = 30;
    if (dd > dom) {
        return false;
    }

    let dia: string;
    let mes: string;
    if (dd < 10) dia = '0' + dd; else dia = dd.toString();
    if (mm < 10) mes = '0' + mm;

    el.val(dia + '/' + mes + '/' + yy);

    return true;
}
/* ========== FIM DAS FUNÇÕES =========== */