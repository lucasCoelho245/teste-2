//using Microsoft.AspNetCore.Http;
//using Pay.Recorrencia.Gestao.Application.Response;

//namespace Pay.Recorrencia.Gestao.Application.Helpers
//{
//    public static class PixReturnHelper
//    {

//        public MensagemPadraoResponseApprove(int statusCode, string codigoInterno, string mensagemErro)
//        {
//            Status = statusCode.Equals(200) ? "OK" : "NOK";
//            StatusCode = statusCode;
//            Error = new Erro
//            {
//                Code = codigoInterno,
//                Message = mensagemErro
//            };
//        }
//        /// <summary>
//        /// Solicitação criada com sucesso.
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponseApprove RetornoSucesso()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = StatusCodes.Status200OK.ToString(), MensagemErro = "Solicitação criada com sucesso." };
//        }

//        /// <summary>
//        /// Chave já existente na tabela SOLICITACAO_AUTORIZACAO_RECORRENCIA
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto001()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-001", MensagemErro = "Chave já existente na tabela SOLICITACAO_AUTORIZACAO_RECORRENCIA" };
//        }

//        /// <summary>
//        /// Campos não preenchidos corretamente
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto002()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-002", MensagemErro = "Campos não preenchidos corretamente" };
//        }

//        /// <summary>
//        /// Solicitação de recorrência não encontrada
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto003()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-003", MensagemErro = "Solicitação de recorrência não encontrada" };
//        }

//        /// <summary>
//        /// Solicitação não pode ser excluída devido ao status pendente de confirmação ou confirmado
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto004()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-004", MensagemErro = "Solicitação não pode ser excluída devido ao status pendente de confirmação ou confirmado" };
//        }

//        /// <summary>
//        /// Chave não encontrada na tabela SOLICITACAO_AUTORIZACAO_RECORRENCIA
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto005()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-005", MensagemErro = "Chave não encontrada na tabela SOLICITACAO_AUTORIZACAO_RECORRENCIA" };
//        }

//        /// <summary>
//        /// Dados enviados pelo canal divergentes dos da tabela SOLICITACAO_AUTORIZACAO_RECORRENCIA
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto006()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-006", MensagemErro = "Dados enviados pelo canal divergentes dos da tabela SOLICITACAO_AUTORIZACAO_RECORRENCIA" };
//        }

//        /// <summary>
//        /// Chave já existente na tabela AUTORIZACAO_RECORRENCIA
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto007()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-007", MensagemErro = "Chave já existente na tabela AUTORIZACAO_RECORRENCIA" };
//        }

//        /// <summary>
//        /// Chave já existente na tabela ATUALIZACOES_AUTORIZACOES_RECORRENCIA
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto008()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-008", MensagemErro = "Chave já existente na tabela ATUALIZACOES_AUTORIZACOES_RECORRENCIA" };
//        }

//        /// <summary>
//        /// Chave não encontrada na tabela AUTORIZACAO_RECORRENCIA
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto009()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-009", MensagemErro = "Chave não encontrada na tabela AUTORIZACAO_RECORRENCIA" };
//        }

//        /// <summary>
//        /// Chave não encontrada na tabela ATUALIZACOES_AUTORIZACOES_RECORRENCIA
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto010()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-010", MensagemErro = "Chave não encontrada na tabela ATUALIZACOES_AUTORIZACOES_RECORRENCIA" };
//        }

//        /// <summary>
//        /// Autorização de recorrência não encontrada
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto011()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-011", MensagemErro = "Autorização de recorrência não encontrada" };
//        }

//        /// <summary>
//        /// Autorização não pode ser excluída devido ao status pendente, em processamento ou confirmado
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto012()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-012", MensagemErro = "Autorização não pode ser excluída devido ao status pendente, em processamento ou confirmado" };
//        }

//        /// <summary>
//        /// Campos divergentes aos campos do QR Code
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto013()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-013", MensagemErro = "Campos divergentes aos campos do QR Code" };
//        }

//        /// <summary>
//        /// Campos do agendamento estão divergentes da recorrência
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto014()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-014", MensagemErro = "Campos do agendamento estão divergentes da recorrência" };
//        }

//        /// <summary>
//        /// Valor do agendamento diferente do valor autorizado pelo cliente
//        /// </summary>
//        /// <returns></returns>
//        public static MensagemPadraoResponse ErroPixAuto015()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-015", MensagemErro = "Valor do agendamento diferente do valor autorizado pelo cliente" };
//        }

//        public static MensagemPadraoResponse ErroPixAuto016()
//        {
//            return new MensagemPadraoResponse() { CodigoRetorno = "ERRO-PIXAUTO-016", MensagemErro = "(Descrição não informada)" };
//        }
//    }
//}