using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Agenti.Models;
using System.Diagnostics;

namespace Agenti.Controllers
{
    public class AnagraficaController : Controller
    {
        public string connString = "Server=DESKTOP-E9R19JS\\SQLEXPRESS; Initial Catalog=DatabaseAgenti; Integrated Security=true; TrustServerCertificate=True";
        public IActionResult Index()
        {
            var conn = new SqlConnection(connString);
            List<AnagraficaModel> listaAnagrafica = [];

            try
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT * FROM ANAGRAFE", conn);

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var anagrafe = new AnagraficaModel()
                        {
                            IDAnagrafica = (int)reader["IDAnagrafica"],
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            Indirizzo = reader["Indirizzo"].ToString(),
                            Citta = reader["Citta"].ToString(),
                            Cap = (int)reader["CAP"],
                            CodiceFiscale = reader["CodiceFiscale"].ToString()
                        };
                        listaAnagrafica.Add(anagrafe);
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Shared");
            }
            finally
            {
                conn.Close();
            }
            return View(listaAnagrafica);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string Cognome, string Nome, string Indirizzo, string Citta, int Cap, string CodiceFiscale)
        {
            var conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO ANAGRAFE (Cognome, Nome, Indirizzo, Citta, CAP, CodiceFiscale) VALUES (@Cognome, @Nome, @Indirizzo, @Citta, @CAP, @CodiceFiscale)", conn);
                cmd.Parameters.AddWithValue("@Cognome", Cognome);
                cmd.Parameters.AddWithValue("@Nome", Nome);
                cmd.Parameters.AddWithValue("@Indirizzo", Indirizzo);
                cmd.Parameters.AddWithValue("@Citta", Citta);
                cmd.Parameters.AddWithValue("@CAP", Cap);
                cmd.Parameters.AddWithValue("@CodiceFiscale", CodiceFiscale);
                var nRows = cmd.ExecuteNonQuery();
                if (nRows > 0)
                {
                    TempData["AnagrafeAdd"] = $"L'anagrafica di {Cognome} {Nome} è stata inserita con successo!";
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return Redirect("Error");
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
