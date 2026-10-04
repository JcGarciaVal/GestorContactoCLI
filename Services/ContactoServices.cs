namespace GestorContactoCLI;

public class ContactoServices
{
    private readonly ContactoRepository _data = new();


    //Requerimientos:
    //*Registrar contacto
    //*consultar todos los contactos
    //*buscar un contacto por el nombre o por el apellido
    //*eliminar registros que no se necesite
    //Regla de negocios:
    //*no duplicar correos al registrar
    //*los contactos deben pertenecer

    public bool RegistrarContacto(Contacto contacto)
    {
        var listaContacto = _data.DevolverJson();
        
        var ultimoId = listaContacto.Any() 
            ? listaContacto.Max(i => i.Id) 
            : 0;
        
        //validaciones de entrada
        if(
            string.IsNullOrWhiteSpace(contacto.Nombre)
            || string.IsNullOrWhiteSpace(contacto.Apellido)
            || string.IsNullOrWhiteSpace(contacto.Correo)
        )
            return false;

        if(!contacto.Correo.Contains('@'))
            return false;

        if(
            !contacto.Correo.EndsWith(".com")
            && !contacto.Correo.EndsWith(".cl"))
                return false;
        
        //corroborar si existe un correo igual
        var correoExistente = listaContacto
            .FirstOrDefault(g=> g.Correo.Equals
                (contacto.Correo, StringComparison.OrdinalIgnoreCase));

        if(correoExistente is not null)
            return false;

        //crear nuevo objeto
        var nuevoContacto = new Contacto
        {
            Id = ultimoId + 1,
            Nombre = char.ToUpper(contacto.Nombre[0]) + contacto.Nombre.Trim().ToLower()[1..],
            Apellido = char.ToUpper(contacto.Apellido[0]) + contacto.Apellido.Trim().ToLower()[1..],
            Correo = contacto.Correo.Trim().ToLower()
        };

        listaContacto.Add(nuevoContacto);

        _data.GuardarJson(listaContacto);

        return true;
    }

    public List<Contacto> DevolverContactos()
    {
        return _data.DevolverJson();
    }

    public List<Contacto> BuscarNombreApellido(string buscar)
    {
        var listaContacto = _data.DevolverJson();

        if(string.IsNullOrWhiteSpace(buscar))
            throw new InvalidOperationException("Nombre o apellido invalido.\n");

        //por nombre o apellido
        //que pasa si tengo mas usuarios con el mismo apellido o nombre

        var listaBuscada = listaContacto
            .Where(n=> n.Nombre.Equals(buscar, StringComparison.OrdinalIgnoreCase) 
            || n.Apellido.Equals(buscar, StringComparison.OrdinalIgnoreCase))
            .ToList();

        //que pasa si el contacto no existe
        if(listaBuscada.Count == 0)
            throw new InvalidOperationException("contacto no existe.\n");

        return listaBuscada;
    }

    public bool EliminarContacto(int id)
    {
        var listaContacto = _data.DevolverJson();

        if(id <= 0)
            return false;

        var contactoEliminar = listaContacto
            .FirstOrDefault(c => c.Id == id);

        if(contactoEliminar is null)
            return false;

        listaContacto.Remove(contactoEliminar);

        _data.GuardarJson(listaContacto);
        return true;
    }
}