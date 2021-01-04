

declare function swal(header, texto): any;

export class ErrorModal {  
    
    //Criei este método para poder incluir tag's html 
    /* Para usar este método precisamos instanciar a classe dentro da função onde iremos utilizar
     * EX:
     *  let erroModal = new ErrorModal();
     *  erroModal.ModalInnerHTML(msg);
     */
    public ModalInnerHTML(texto: string):void {        
        let headerErro: string = "Campos inválidos";
        swal(headerErro, texto);
        let t: string = $('.sweet-alert p')[0].textContent;
        $('.sweet-alert p').html(t);
    }

    public MostrarMsg(msg: any): void {
        $('#teste').empty().append(msg);
    }
}

