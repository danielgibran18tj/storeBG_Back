using Microsoft.EntityFrameworkCore;
using proyectop.Data.Entities;
using proyectop.Data.Models;
using proyectop.Domain;

namespace proyectop.Data.Repository;

public class UsuariosRepository: IUsuariosRepository
{
    DataBaseContext _context;

    public UsuariosRepository(DataBaseContext context)
    {
        _context = context;
    }
    
    public IEnumerable<UsuarioEntity> Get()
    {
        return _context.Usuario.Include(u => u.Role);
    }

    public void createUser(UsuarioEntity usuarioEntity)
    {
        _context.Usuario.Add(usuarioEntity); 
        _context.SaveChanges();
    }

    public UsuarioEntity GetUserLogin(Credentials login)
    {
        return _context.Usuario.Include(u => u.Role).FirstOrDefault(x => x.Username == login.username);
    }

    public UsuarioEntity VerifyIfUserExist(String userName)
    {
        return _context.Usuario.FirstOrDefault(x => x.Username == userName);

    }
}