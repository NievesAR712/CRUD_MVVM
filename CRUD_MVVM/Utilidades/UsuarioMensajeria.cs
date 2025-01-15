using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CRUD_MVVM.Utilidades
{
    public class UsuarioMensajeria : ValueChangedMessage<UsuarioMensaje>
    {
        public UsuarioMensajeria(UsuarioMensaje value): base(value) 
        {
            
        }
    }
}
