using proyectop.Data.Models;
using proyectop.Data.Models.Request;

namespace proyectop.Domain;

public interface IUsuariosRepository
{
    IEnumerable<Usuario> Get();
    void createUser(Usuario usuario);
    Usuario GetUserLogin(LoginRQ login);
    Usuario VerifyIfUserExist(String userName);
}