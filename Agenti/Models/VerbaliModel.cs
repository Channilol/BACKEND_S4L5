namespace Agenti.Models
{
    public class VerbaliModel
    {
        public int IdVerbale { get; set; }

        public int IdAnagrafica { get; set; }

        public string Cognome { get; set; }

        public string Nome { get; set; }

        public int IdViolazione { get; set; }

        public string DescViolazione { get; set; }

        public string DataViolazione { get; set; }

        public string IndirizzoViolazione { get; set; }

        public int IdAgente { get; set; }

        public string DataTrascrVerbale {  get; set; }

        public decimal Importo { get; set; }

        public int DecurtamentoPunti { get; set; }

        public int SommaPuntiDecurtati { get; set; }
    }
}
