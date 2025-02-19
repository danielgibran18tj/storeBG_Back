using BG.Data.Entities;
using BG.Data.Models;

namespace BG.Domain;

public interface IUsuariosRepository
{
    IEnumerable<UsuarioEntity> Get();
    void createUser(UsuarioEntity usuarioEntity);
    UsuarioEntity GetUserLogin(Credentials login);
    UsuarioEntity VerifyIfUserExist(String userName);
}