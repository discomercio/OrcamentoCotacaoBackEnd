using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco
{
    public class ContextoBdGravacaoOpcoes
    {
        public ContextoBdGravacaoOpcoes(bool TRATAMENTO_ACESSO_CONCORRENTE_LOCK_EXCLUSIVO_MANUAL_HABILITADO)
        {
            this.TRATAMENTO_ACESSO_CONCORRENTE_LOCK_EXCLUSIVO_MANUAL_HABILITADO = TRATAMENTO_ACESSO_CONCORRENTE_LOCK_EXCLUSIVO_MANUAL_HABILITADO;
        }

        public bool TRATAMENTO_ACESSO_CONCORRENTE_LOCK_EXCLUSIVO_MANUAL_HABILITADO { get; private set; } = true;
    }
}
