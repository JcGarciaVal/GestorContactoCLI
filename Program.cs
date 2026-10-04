using GestorContactoCLI;

ContactoServices services = new();

bool sistema = true;
int opcion = 0;

Console.WriteLine("¡Bienvenido a Agenda de Contactos!");

while (sistema)
{
    Console.WriteLine("Seleccione una opcion:");
    Console.WriteLine("\t1. Crear contacto.");
    Console.WriteLine("\t2. Ver contactos.");
    Console.WriteLine("\t3. Buscar contacto.");
    Console.WriteLine("\t4. Eliminar contacto.");
    Console.WriteLine("\t5. Salir.");

    Console.Write("opcion seleccionada: ");

    while(!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 5)
    {
        Console.WriteLine("\t[!] Opcion incorrecta");
        Console.Write("nueva opcion: ");
    }
    try{
    switch (opcion)
    {
        case 1:
            Console.WriteLine("Creacion de nuevo contacto:");
            Console.Write("\tNombre: ");
            string nombre = Console.ReadLine() ?? "";
            Console.Write("\tApellido: ");
            string apellido = Console.ReadLine() ?? "";
            Console.Write("\tCorreo: ");
            string correo = Console.ReadLine() ?? "";

            var nuevoContacto = new Contacto
            {
                Nombre = nombre,
                Apellido = apellido,
                Correo = correo 
            };

            var validacionCreacion = services.RegistrarContacto(nuevoContacto)
                ? "¡Contacto creado con exito!"
                : throw new Exception("Problema al crear el contacto.\n");

            Console.WriteLine(validacionCreacion + "\n");
        break;

        case 2:
            Console.WriteLine($"\n{"-----------------Lista Contacto------------------", 5}\n");
            var contactos = services.DevolverContactos();
            foreach(var contacto in contactos)
            {
                Console.WriteLine($"{contacto.Nombre, 15} | {contacto.Apellido, -15} | {contacto.Correo, 20}");
            }
            Console.WriteLine("----------------------------------------\n");
        break;

        case 3:
            Console.Write("\nbusqueda de contacto: ");
            string buscar = Console.ReadLine() ?? "";
            var listabuscada = services.BuscarNombreApellido(buscar);
            Console.WriteLine($"\n{"-----------------Lista Contacto------------------", 5}\n");
            foreach(var contacto in listabuscada)
            {
                Console.WriteLine($"{contacto.Nombre, 15} | {contacto.Apellido, -15} | {contacto.Correo, 20}");
            }
            Console.WriteLine("----------------------------------------\n");
        break;

        case 4:
            int idSeleccionar;
            Console.WriteLine($"\n{"-----------------Lista Contacto------------------", 5}\n");
            var contactosEliminar = services.DevolverContactos();
            foreach(var contacto in contactosEliminar)
            {
                Console.WriteLine($"{contacto.Id, 3} | {contacto.Nombre, 15} | {contacto.Apellido, -15} | {contacto.Correo, 20}");
            }
            Console.WriteLine("----------------------------------------\n");

            Console.Write("Seleccione id de contacto a eliminar: ");
                while (!int.TryParse(Console.ReadLine(), out  idSeleccionar)
                    || idSeleccionar <= 0 )
                {
                    Console.WriteLine("[!] ID inválido.");
                    Console.Write("Seleccione ID: ");
                }
                var contactoEliminado = services.EliminarContacto(idSeleccionar)
                ? "Contacto eliminado."
                : throw new Exception("Problema al eliminar contacto.");
                Console.WriteLine(contactoEliminado);
        break;

        case 5:
            sistema = false;
            break;
    }
    }
    catch(InvalidOperationException ex)
    {
        Console.WriteLine("[!]Error: " + ex.Message);
    }
    catch(Exception ex)
    {
        Console.WriteLine("\n[!]Error: " + ex.Message);
    }
}