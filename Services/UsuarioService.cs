using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services
{
    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;

        // Constructor que inyecta el contexto de la base de datos
        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para obtener un usuario por su documento
        public Usuario? ObtenerUsuarioPorDocumento(string documento)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Documento == documento);
        }

        // Método para verificar si un usuario es administrador
        public bool EsAdministrador(string documento)
        {
            var usuario = ObtenerUsuarioPorDocumento(documento);
            return usuario != null && usuario.EsAdministrador;
        }

        // Método para verificar si un usuario es empleado
        public bool EsEmpleado(string documento)
        {
            var usuario = ObtenerUsuarioPorDocumento(documento);
            return usuario != null && usuario.EsEmpleado;
        }

        // Método adicional: Obtener todos los usuarios (opcional)
        public IEnumerable<Usuario> ObtenerTodosLosUsuarios()
        {
            return _context.Usuarios.ToList();
        }

        // Método adicional: Crear un nuevo usuario
        public void CrearUsuario(Usuario nuevoUsuario)
        {
            _context.Usuarios.Add(nuevoUsuario);
            _context.SaveChanges();
        }

        // Método adicional: Actualizar un usuario existente
        public void ActualizarUsuario(Usuario usuarioActualizado)
        {
            _context.Usuarios.Update(usuarioActualizado);
            _context.SaveChanges();
        }

        // Método adicional: Eliminar un usuario por su documento
        public void EliminarUsuario(string documento)
        {
            var usuario = ObtenerUsuarioPorDocumento(documento);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
        }
    }
}