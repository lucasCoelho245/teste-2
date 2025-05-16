using System.Security.Cryptography;
using System.Text;

namespace Pay.Recorrencia.Gestao.Domain.Helpers
{
    public static class IdInformacaoStatusGenerator
    {
        public static string Gerar(string ispb)
        {
            // Garantir que o ISPB tenha exatamente 8 caracteres (preenche com zeros à esquerda, se necessário)
            ispb = (ispb ?? string.Empty).PadLeft(8, '0').Substring(0, 8);

            // Data no formato yyyyMMdd
            var data = DateTime.UtcNow.ToString("yyyyMMdd");

            // Sequencial alfanumérico de 11 caracteres únicos
            var sequencial = GerarSequenciaAlfanumerica(11);

            return $"IS{ispb}{data}{sequencial}";
        }

        private static string GerarSequenciaAlfanumerica(int tamanho)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = RandomNumberGenerator.Create();
            var bytes = new byte[tamanho];
            random.GetBytes(bytes);

            var result = new StringBuilder(tamanho);
            foreach (var b in bytes)
            {
                result.Append(chars[b % chars.Length]);
            }

            return result.ToString();
        }
    }
}
