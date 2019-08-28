export class DetalhesFormaPagamentos
{
    FormaPagto: string;
    InfosAnaliseCredito: string;
    StatusPagto: string;
    VlTotalFamilia: number;
    VlPago: number;
    VlDevolucao: number;
    VlPerdas: number | null;
    SaldoAPagar: number | null;
    AnaliseCredito: string;
    DataColeta: Date | string | null;
    Transportadora: string;
    VlFrete: number;
    Ocorrencias: string;
    BlocoNotas: string;
}
