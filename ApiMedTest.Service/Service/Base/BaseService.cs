using ApiMedTest.Service.Notifications;
using ApiMedTest.Service.Notifications.Interfaces;

namespace ApiMedTest.Service.Service.Base
{
    public abstract class BaseService
    {
        private readonly INotificador _notificador;

        protected BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected void Notificar(IEnumerable<string> mensagens)
        {
            foreach (var mensagem in mensagens)
            {
                Notificar(mensagem);
            }
        }
    }
}
