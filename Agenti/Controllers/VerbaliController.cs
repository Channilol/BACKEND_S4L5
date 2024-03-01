using Agenti.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Agenti.Controllers
{
    public class VerbaliController : Controller
    {
        public string connString = "Server=DESKTOP-E9R19JS\\SQLEXPRESS; Initial Catalog=DatabaseAgenti; Integrated Security=true; TrustServerCertificate=True";
        public IActionResult Index()
        {
            var conn = new SqlConnection(connString);
            List<VerbaliModel> listaVerbali = [];

            try
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT ti.IDVerbale, ti.IDAnagrafica, viol.DescrizioneViolazione, verb.DataViolazione, verb.IndirizzoViolazione, verb.IDAgente, verb.DataTrascrVerbale, verb.Importo, verb.DecurtamentoPunti FROM TabellaIntermedia ti  LEFT JOIN ANAGRAFE a ON ti.IDAnagrafica = a.IDAnagrafica  LEFT JOIN VERBALE verb on ti.IDVerbale = verb.IDVerbale  LEFT JOIN VIOLAZIONE viol on ti.IDViolazione = viol.IDViolazione  LEFT JOIN AGENTE ag on verb.IDAgente = ag.IDAgente", conn);

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var verbale = new VerbaliModel()
                        {
                            IdVerbale = (int)reader["IDVerbale"],
                            IdAnagrafica = (int)reader["IDAnagrafica"],
                            DescViolazione = reader["DescrizioneViolazione"].ToString(),
                            DataViolazione = reader["DataViolazione"].ToString(),
                            IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                            IdAgente = (int)reader["IDAgente"],
                            DataTrascrVerbale = reader["DataTrascrVerbale"].ToString(),
                            Importo = (Decimal)reader["Importo"],
                            DecurtamentoPunti = (int)reader["DecurtamentoPunti"]
                        };
                        listaVerbali.Add(verbale);
                    }
                }
                return View(listaVerbali);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Shared");
            }
            finally
            {
                conn.Close();
            }
          
        }

        public IActionResult Add()
        {
            var conn = new SqlConnection(connString);
            List<VerbaliModel> anagrafi = [];

            try
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT IDAnagrafica, Cognome, Nome FROM ANAGRAFE", conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows) 
                {
                    while (reader.Read())
                    {
                        var persona = new VerbaliModel()
                        {
                            IdAnagrafica = (int)reader["IDAnagrafica"],
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString()
                        };
                        anagrafi.Add(persona);
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

            return View(anagrafi);
        }

        [HttpPost]
        public IActionResult Add(int IdAnagrafica, int IdViolazione, DateTime DataViolazione, string IndirizzoViolazione, int IDAgente, DateTime DataTrascrVerbale, decimal Importo, int DecurtamentoPunti)
        {
            var conn = new SqlConnection(connString);
            int idNewVerbale = 0;

            try
            {
                conn.Open();
                //INSERT DEL VERBALE
                var cmdInsert = new SqlCommand("INSERT INTO VERBALE (DataViolazione, IndirizzoViolazione, IDAgente, DataTrascrVerbale, Importo, DecurtamentoPunti) VALUES (@DataViolazione, @IndirizzoViolazione, @IDAgente, @DataTrascrVerbale, @Importo, @DecurtamentoPunti)", conn);
                cmdInsert.Parameters.AddWithValue("@DataViolazione", DataViolazione.ToString());
                cmdInsert.Parameters.AddWithValue("@IndirizzoViolazione", IndirizzoViolazione);
                cmdInsert.Parameters.AddWithValue("@IDAgente", IDAgente);
                cmdInsert.Parameters.AddWithValue("@DataTrascrVerbale", DataTrascrVerbale.ToString());
                cmdInsert.Parameters.AddWithValue("@Importo", Importo);
                cmdInsert.Parameters.AddWithValue("@DecurtamentoPunti", DecurtamentoPunti);
                var nRows = cmdInsert.ExecuteNonQuery();
                if(nRows > 0)
                {

                    //SELECT DEL VERBALE APPENA AGGIUNTO PER RECUPERARE L'ID
                    var cmdRead = new SqlCommand("SELECT MAX(IDVerbale) as IDVerbale FROM VERBALE;", conn);
                    var reader = cmdRead.ExecuteReader();
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            idNewVerbale = (int)reader["IDVerbale"];
                        }
                    }

                    reader.Close();
                }

                //INSERT NELLA TABELLA INTERMEDIA
                if (idNewVerbale > 0)
                {
                    var cmdInsert2 = new SqlCommand("INSERT INTO TabellaIntermedia (IDAnagrafica, IDVerbale, IDViolazione) VALUES (@IDAnagrafica, @IDVerbale, @IDViolazione)", conn);
                    cmdInsert2.Parameters.AddWithValue("@IDAnagrafica", IdAnagrafica);
                    cmdInsert2.Parameters.AddWithValue("@IDVerbale", idNewVerbale);
                    cmdInsert2.Parameters.AddWithValue("@IDViolazione", IdViolazione);

                    var nRows2 = cmdInsert2.ExecuteNonQuery();
                }

                return RedirectToAction("Index", "Verbali");
                
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

        public IActionResult VerbaliPerAnagrafe()
        {
            List<VerbaliModel> verbaliPerAnagrafe = [];
            var conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT ti.IDAnagrafica, anag.Cognome, anag.Nome, verb.*  FROM TabellaIntermedia ti LEFT JOIN ANAGRAFE anag ON ti.IDAnagrafica = anag.IDAnagrafica  LEFT JOIN VERBALE verb on ti.IDVerbale = verb.IDVerbale  LEFT JOIN VIOLAZIONE viol on ti.IDViolazione = viol.IDViolazione  LEFT JOIN AGENTE ag on verb.IDAgente = ag.IDAgente  ORDER BY ti.IDAnagrafica ASC", conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var persona = new VerbaliModel()
                        {
                            IdAnagrafica = (int)reader["IDAnagrafica"],
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            IdVerbale = (int)reader["IDVerbale"],
                            DataViolazione = reader["DataViolazione"].ToString(),
                            IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                            IdAgente = (int)reader["IDAgente"],
                            DataTrascrVerbale = reader["DataTrascrVerbale"].ToString(),
                            Importo = (Decimal)reader["Importo"],
                            DecurtamentoPunti = (int)reader["DecurtamentoPunti"]
                        };
                        verbaliPerAnagrafe.Add(persona);
                    }
                }
                return View(verbaliPerAnagrafe);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
            finally
            {
                conn.Close();
            }
            
        }

        public IActionResult PuntiPerAnagrafe()
        {
            List<VerbaliModel> puntiPerAnagrafe = [];
            var conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT ti.IDAnagrafica, anag.Cognome, anag.Nome, sum(DecurtamentoPunti) as SommaPuntiDecurtati  FROM TabellaIntermedia ti  LEFT JOIN ANAGRAFE anag ON ti.IDAnagrafica = anag.IDAnagrafica  LEFT JOIN VERBALE verb on ti.IDVerbale = verb.IDVerbale  LEFT JOIN VIOLAZIONE viol on ti.IDViolazione = viol.IDViolazione  LEFT JOIN AGENTE ag on verb.IDAgente = ag.IDAgente  GROUP BY ti.IDAnagrafica, anag.Cognome, anag.Nome", conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var persona = new VerbaliModel()
                        {
                            IdAnagrafica = (int)reader["IDAnagrafica"],
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            SommaPuntiDecurtati = (int)reader["SommaPuntiDecurtati"]
                        };
                        puntiPerAnagrafe.Add(persona);
                    }
                }
                return View(puntiPerAnagrafe);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
            finally
            {
                conn.Close();
            }

        }

        public IActionResult VerbaleSopra10Punti()
        {
            List<VerbaliModel> verbaleSopra10Punti = [];
            var conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT verb.Importo, anag.Cognome, anag.Nome, verb.DataViolazione, verb.DecurtamentoPunti FROM TabellaIntermedia ti LEFT JOIN ANAGRAFE anag ON ti.IDAnagrafica = anag.IDAnagrafica  LEFT JOIN VERBALE verb on ti.IDVerbale = verb.IDVerbale  LEFT JOIN VIOLAZIONE viol on ti.IDViolazione = viol.IDViolazione  LEFT JOIN AGENTE ag on verb.IDAgente = ag.IDAgente  WHERE DecurtamentoPunti > 10", conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var persona = new VerbaliModel()
                        {
                            Importo = (Decimal)reader["Importo"],
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            DataViolazione = reader["DataViolazione"].ToString(),
                            DecurtamentoPunti = (int)reader["DecurtamentoPunti"]
                        };
                        verbaleSopra10Punti.Add(persona);
                    }
                }
                return View(verbaleSopra10Punti);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
            finally
            {
                conn.Close();
            }

        }

        public IActionResult VerbaleSopra400Euro()
        {
            List<VerbaliModel> verbaleSopra400Euro = [];
            var conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT ti.IDAnagrafica, a.Cognome, a.Nome, verb.*  FROM TabellaIntermedia ti LEFT JOIN ANAGRAFE a ON ti.IDAnagrafica = a.IDAnagrafica  LEFT JOIN VERBALE verb on ti.IDVerbale = verb.IDVerbale  LEFT JOIN VIOLAZIONE viol on ti.IDViolazione = viol.IDViolazione  LEFT JOIN AGENTE ag on verb.IDAgente = ag.IDAgente  WHERE verb.Importo > 400  ORDER BY ti.IDAnagrafica ASC", conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var persona = new VerbaliModel()
                        {
                            IdAnagrafica = (int)reader["IDAnagrafica"],
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            IdVerbale = (int)reader["IDVerbale"],
                            DataViolazione = reader["DataViolazione"].ToString(),
                            IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                            IdAgente = (int)reader["IDAgente"],
                            DataTrascrVerbale = reader["DataTrascrVerbale"].ToString(),
                            Importo = (Decimal)reader["Importo"],
                            DecurtamentoPunti = (int)reader["DecurtamentoPunti"]
                        };
                        verbaleSopra400Euro.Add(persona);
                    }
                }
                return View(verbaleSopra400Euro);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
