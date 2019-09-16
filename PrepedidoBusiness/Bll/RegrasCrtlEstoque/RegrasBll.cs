using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Bll.Regras
{
    public class RegrasBll
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public bool St_Regra { get; set; }
        public t_WMS_REGRA_CD TwmsRegraCd { get; set; }
        public t_WMS_REGRA_CD_X_UF TwmsRegraCdXUf { get; set; }
        public t_WMS_REGRA_CD_X_UF_X_PESSOA TwmsRegraCdXUfXPessoa { get; set; }
        public List<t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD> TwmsCdXUfXPessoaXCd { get; set; }
    }
    public class t_WMS_REGRA_CD
    {
        public int Id { get; set; }
        public byte St_inativo { get; set; }
        public string Apelido { get; set; }
        public string Descricao { get; set; }
    }

    public class t_WMS_REGRA_CD_X_UF
    {
        public int Id { get; set; }
        public int Id_wms_regra_cd { get; set; }
        public string Uf { get; set; }
        public byte St_inativo { get; set; }
    }

    public class t_WMS_REGRA_CD_X_UF_X_PESSOA
    {
        public int Id { get; set; }
        public int Id_wms_regra_cd_x_uf { get; set; }
        public string Tipo_pessoa { get; set; }
        public byte St_inativo { get; set; }
        public int Spe_id_nfe_emitente { get; set; }
    }

    public class t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD
    {
        public int Id { get; set; }
        public int Id_wms_regra_cd_x_uf_x_pessoa { get; set; }
        public int Id_nfe_emitente { get; set; }
        public int Ordem_prioridade { get; set; }
        public byte St_inativo { get; set; }

        //informações de estoque de produto
        public string Estoque_Fabricante { get; set; }
        public string Estoque_Produto { get; set; }
        public string Estoque_Descricao { get; set; }
        public string Estoque_DescricaoHtml { get; set; }
        public short Estoque_Qtde_Solicitado { get; set; }
        public short Estoque_Qtde { get; set; }
    }

}
