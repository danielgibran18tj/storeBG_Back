* La base de datos SQL Server se encuentra en el contenedor 'sql1'

``dotnet ef migrations add InitialCreate``

``dotnet ef database update``



* Despues de modificar las entidades actualizamos la db

``dotnet ef migrations add UpdatePassToUser``

``dotnet ef database update``

* tener creados los roles antes de crear un usuario "ADMINISTRADOR" y "RESTRINGIDO"

* body para crear usuario

``   {
"roleId": 1,
"username": "danielHV",
"email": "danielh@mail.com",
"password": "1234"
}``

``{
"roleId": 2,
"username": "josy",
"email": "josias@mail.com",
"password": "josy2345.H"
}``


* body para login `http://localhost:5024/api/login`
  \
  ``
  {"username": "danielHV", "password": "1234"}
  ``
  ``
  {"username": "josy", "password": "josy2345.H"}
  ``

* Para los metodos de los controladores de Productos, se requiere usar token

- Primeramente hay que agregar las categorias para las cuales hay un archivo json
- Y despues agregar los productos que tambien estan en archivo json