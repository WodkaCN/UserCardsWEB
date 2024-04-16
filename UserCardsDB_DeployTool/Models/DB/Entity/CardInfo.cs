namespace UserCardsDB_DeployTool.Models.DB.Entity;

#nullable disable

public partial class CardInfo
{
    public Int64? ID { get; set; }
    public String UID { get; set; }
    public String FIOLat { get; set; }
    public String CardNumber { get; set; }
    public String CardAccount { get; set; }
    public DateTime Expire { get; set; }
    public String CVC { get; set; }
}
