using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Testes.Labs.InfraBancoLab
{
    /*
     * o acesso ao banco é feito através do InfraBanco
     * 
     * aqui verificamos como usar os contextos e as transações
     * 
     * */
    class AcessoBanco
    {
        private void ImprimirConclusoes()
        {
            var msg = @"Conclusões anteriores:

";
            msg += @"Leitura: usar um contexto diferente para cada acesso é BEM mais rápido se a gente usar o await só quando precisar.
Nota: esses tempos variam BASTANTE. Executar novamente pode dar conclusões diferentes, mas essa é a conclusão correta.

";
            msg += @"Gravação: usar using (var dbContextTransaction = dbGravacao.Database.BeginTransaction()) e fazer todas as gravações na dbGravacao.
Pode-se fazer leituras na dbGravacao (recomendável) ou em outro contexto (que estarão fora da transação)

";
            msg += @"Números:

Com o mesmo contexto
async em apralelo 00:00:01.83
async uma depois da outra 00:00:01.51
async em apralelo 00:00:01.83
async uma depois da outra 00:00:01.79
Com contextos diferentes
async em apralelo 00:00:00.93
async uma depois da outra 00:00:01.74
async em apralelo 00:00:00.73
async uma depois da outra 00:00:01.88";

            Console.WriteLine(msg);
        }

        public Contextos contextos = new Contextos();

        public void Executar()
        {
            Console.WriteLine("Testes.Labs.InfraBancoLab.AcessoBanco.Executar iniciando");
            ImprimirConclusoes();
            Console.WriteLine("------------------------------------");
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Executando....");

            //acesso simples, só para testar que conecta
            TesteBasico();
            //rodamos duas vezes para "esquentar" o banco de dados, e a cronometragem ficar mais estável
            TesteBasico();

            //transacao para gravacao
            GravacaoAsync gravacaoAsync= new GravacaoAsync(contextos, DadosExemplo.filtroClientes());
            gravacaoAsync.Executar().Wait();

            //vamos testar com async, somente leitura
            LeituraAsync leituraAsync = new LeituraAsync(contextos, DadosExemplo.filtroClientes());
            leituraAsync.Executar().Wait();

        }


        private void TesteBasico(bool log = true)
        {
            var db = contextos.ContextoNovo();

            //Buscar dados do cliente
            var clienteTask = (from c in db.Tclientes
                               select c.Id);


            /*
             * uma lista de clientes: vamos gerar com 2k clientes
             * 
             * */
            //deixamos com 20 registros só pra acelerar a carga
            var clienteListaIds = clienteTask.Take(20).ToList();
            var jsonClientes = JsonConvert.SerializeObject(clienteListaIds);
            var listaClientesGerada = JsonConvert.DeserializeObject<List<string>>(jsonClientes);

            //nosso filtro
            var filtroClientes = DadosExemplo.filtroClientes();

            //agora filtramos
            int tamanh1 = (from c in db.Tclientes where filtroClientes.Contains(c.Id) select c).ToList().Count;
            int tamanh2 = (from c in db.Tpedidos where filtroClientes.Contains(c.Id_Cliente) select c).ToList().Count;
            if (log)
                Console.WriteLine($"TesteBasico(), tamanh1: {tamanh1}, tamanh2: {tamanh2}, filtroClientes: {filtroClientes.Count}");
        }
    }

}
