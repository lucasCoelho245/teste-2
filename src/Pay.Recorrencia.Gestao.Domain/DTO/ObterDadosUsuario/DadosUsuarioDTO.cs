namespace Pay.Recorrencia.Gestao.Domain.DTO.ObterDadosUsuario
{
    public class DadosUsuarioDTO
    {
        public string id { get; set; }
        public string cpf { get; set; }
        public string cellphone { get; set; }
        public string email { get; set; }
        public string nickname { get; set; }
        public int status { get; set; }
        public int fingerPrintStatus { get; set; }
        public int faceIdStatus { get; set; }
        public DateTime? totpActivationDate { get; set; }
        public List<PasswordDTO> passwords { get; set; }
    }
}
