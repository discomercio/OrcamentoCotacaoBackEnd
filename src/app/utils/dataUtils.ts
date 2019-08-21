export class DataUtils {
    public static formataParaFormulario(data: Date): string {
        //queremos o formato yyy-mm-dd, é o que o input date precisa
        //https://stackoverflow.com/questions/3552461/how-to-format-a-javascript-date
        return data.toISOString().slice(0, 10);
    }

    public static somarDias(data: Date, dias: number): Date {
        //define a data de 60 dias para trás
        //https://stackoverflow.com/questions/563406/add-days-to-javascript-date
        let ms = data.getTime() + (86400000 * dias);
        return new Date(ms);
    }


    public static formatarTela(data: Date): string {
        //para imprimir na tela
        //está vindo como string do c#!
        return new Date(Date.parse(data.toString())).toLocaleDateString();
    }
}