namespace Pay.Recorrencia.Gestao.Domain.Services
{
    public interface IPushService
    {
        /// <summary>
        /// Envia notificação push.
        /// </summary>
        /// <param name="titulo">Título da notificação</param>
        /// <param name="texto">Texto da notificação</param>
        /// <param name="destinatarios">Array de IDs do pagador no banco do Crefisa Mais</param>
        /// <returns></returns>
        public Task<bool> EnviarPush(string titulo, string texto, string[]? destinatarios);
    }
}
