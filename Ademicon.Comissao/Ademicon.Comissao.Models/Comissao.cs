namespace Ademicon.Comissao.Models;

public class Comissao
{
    public string? CdRepres {get;set;} 
    public string? CdNmCategComis {get;set;}
    public string? Parcela {get;set;}
    public Consorciado? Consorciado{get;set;}
    public string? VendCota {get;set;}
    public string? Equipe {get;set;}
    public string? PercComissao{get;set;}
    public string? NroContrato {get;set;}
    public string? VlrBase {get;set;}
    public DateTime DataVenda {get;set;}
    public decimal VLRBrutoComissao {get;set;}

}
