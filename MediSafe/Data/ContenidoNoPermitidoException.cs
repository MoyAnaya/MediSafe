namespace MediSafe.Data
{
    public class ContenidoNoPermitidoException : Exception
    {
        public ContenidoNoPermitidoException()
            : base("No se permite guardar contenido HTML o código ejecutable.")
        {
        }
    }
}

