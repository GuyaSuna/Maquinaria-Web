namespace Web_Equipment.Models
{

    public class PagoRevision
    {
        public int Id { get; set; }

        public string EquipmentId { get; set; } = "";

        public string? MercadoPagoOrderId { get; set; }

        public string Referencia { get; set; } = "";

        public decimal Monto { get; set; }

        public string Estado { get; set; } = "PENDIENTE";

        public DateTime Fecha { get; set; }
    }

}
