using Microsoft.EntityFrameworkCore;
using proyectop.Data.Models;
using proyectop.Data.Models.Request;
using proyectop.Domain;

namespace proyectop.Data.Repository;

public class UsuariosRepository: IUsuariosRepository
{
    DataBaseContext _context;

    public UsuariosRepository(DataBaseContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Usuario> Get()
    {
        return _context.Usuario.Include(u => u.Role);
    }

    public void createUser(Usuario usuario)
    {
        _context.Usuario.Add(usuario); 
        _context.SaveChanges();
    }

    public Usuario GetUserLogin(LoginRQ login)
    {
        return _context.Usuario.Include(u => u.Role).FirstOrDefault(x => x.Username == login.username);
    }

    public Usuario VerifyIfUserExist(String userName)
    {
        return _context.Usuario.FirstOrDefault(x => x.Username == userName);

    }
}