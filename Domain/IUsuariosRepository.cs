using proyectop.Data.Entities;
using proyectop.Data.Models;

namespace proyectop.Domain;

public interface IUsuariosRepository
{
    IEnumerable<UsuarioEntity> Get();
    void createUser(UsuarioEntity usuarioEntity);
    UsuarioEntity GetUserLogin(Credentials login);
    UsuarioEntity VerifyIfUserExist(String userName);
}