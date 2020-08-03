﻿using System;
using TechTalk.SpecFlow;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido
{
    [Binding, Scope(Tag = "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido")]
    public class CadastrarPrepedidoVerificarQueExecutouSteps
    {
        [Then(@"Verificar que executou ""(.*)""")]
        public void ThenVerificarQueExecutou(string especificacao)
        {
            switch (especificacao)
            {
                case "Especificacao.Comuns.Api.Autenticacao":
                    Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Autenticacao.ThenVerificarQueExecutou(especificacao);
                    break;
                default:
                    throw new ArgumentException($"especificacao {especificacao} desconhecida");

            }
        }
    }
}
