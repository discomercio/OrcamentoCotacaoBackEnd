export class StringUtils {
    public static retorna_so_digitos(msg: string): string {
        return msg.replace(/\D/g, "");
    }

    //não é 100% confiável, mas funciona
    //se tiver entrada de usuário, devemos converter por algum element oculto no browser (setando o html e pegando o texto)
    //para casos onde a origem seja confiável, podemos usar esta rotina
    public static TextoDeHtml(html: string): string {
        if (!html)
            return "";
        return html.replace(/<[^>]*>?/gm, '');
    }
}