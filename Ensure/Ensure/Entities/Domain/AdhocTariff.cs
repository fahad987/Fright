namespace Ensure.Entities.Domain;

public class AdhocTariff
{
    public Guid id { get; set; }  
    public Guid originId { get; set; }  = Guid.Empty;
    public Guid destinationId { get; set; }  = Guid.Empty;
    public string min { get; set; } = string.Empty;
    public string n { get; set; } = string.Empty;
    public float w450000 { get; set; } =0;
    public float w100000 { get; set; } =0; 
    public float w200000 { get; set; } =0; 
    public float w300000 { get; set; } =0; 
    public float w500000 { get; set; } =0; 
    public float w1000000 { get; set; }=0;
    public float w500Document { get; set; } = 0;
    public float w100Document { get; set; } = 0;
    public float w1000 { get; set; }  =0;
    public float w2000 { get; set; }  =0;
    public float w3000 { get; set; }  =0;
    public float w4000 { get; set; }  =0;
    public float w50000 { get; set; } =0; 
    public float w6000 { get; set; }  =0;
    public float w7000 { get; set; }  =0;
    public float w8000 { get; set; }  =0;
    public float w9000 { get; set; }  =0;
    public float w15000 { get; set; } =0; 
    public float w20000 { get; set; } =0; 
    public float w25000 { get; set; } =0; 
    public float w30000 { get; set; } =0; 
    public float w35000 { get; set; } =0; 
    public float w40000 { get; set; } =0; 
    public Guid createBy { get; set; }= Guid.Empty; 
    public DateTime createDate { get; set; }
    public string city { get; set; } = string.Empty;
    public string country = string.Empty;

}