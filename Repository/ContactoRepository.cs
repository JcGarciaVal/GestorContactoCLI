using System.Text.Json;

namespace GestorContactoCLI;

public class ContactoRepository
{
    private readonly string _ruta = 
        Path.Combine(Directory.GetCurrentDirectory(), "contacto.json");

    public List<Contacto> DevolverJson()
    {
        if (!File.Exists(_ruta))
        {
            return new List<Contacto>();
        }

        var json = File.ReadAllText(_ruta);

        return JsonSerializer.Deserialize<List<Contacto>>(json)
            ?? new List<Contacto>() ;
    }

    public void GuardarJson(List<Contacto> contactos)
    {
        var json = JsonSerializer.Serialize(contactos, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_ruta, json);
    }
}




   

    