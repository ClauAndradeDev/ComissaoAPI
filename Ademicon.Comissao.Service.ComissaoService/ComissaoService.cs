using HtmlAgilityPack;
using Ademicon.Comissao.Models;

namespace Ademicon.Comissao.Service.ComissaoService;

public class ComissaoService
{
    public async Task<Comissaovenda?[]> Processar(String filePath)
    {
        var result = await Task.Run(() =>
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(filePath);

            string targetClass = "dataframe";
            var textNodeFilter = $"//table[contains(@class, '{targetClass}')]";
            var tables = htmlDoc.DocumentNode.SelectNodes(textNodeFilter);

            if (tables == null || tables.Count < 4)
            {
                throw new Exception("Arquivo com estrutura incorreta!");
            }


            var fourthTable = tables[3];

            var tbody = fourthTable.SelectSingleNode("tbody");
            var rows = tbody.SelectNodes("tr");

            var comissoes = rows.Select(row =>
            {
                var cells = row.SelectNodes("th|td");
                var cdRepres = cells.ElementAtOrDefault(0)?.InnerText;
                var cdNmCategComis = cells.ElementAtOrDefault(1)?.InnerText;
                var parcela = cells.ElementAtOrDefault(2)?.InnerText;
                var consorc = cells.ElementAtOrDefault(3)?.InnerText;
                var vendCota = cells.ElementAtOrDefault(4)?.InnerText;
                var equipe = cells.ElementAtOrDefault(5)?.InnerText;
                var percComissao = cells.ElementAtOrDefault(6)?.InnerText;
                var nroContrato = cells.ElementAtOrDefault(7)?.InnerText;
                var vlrBase = cells.ElementAtOrDefault(8)?.InnerText;
                var dataVenda =cells.ElementAtOrDefault(9)?.InnerText;
                var vlRBrutoComissao = cells.ElementAtOrDefault(10)?.InnerText;
                var observacao = cells.ElementAtOrDefault(11)?.InnerHtml;

                long cdRepresInt = 0;
                if (!Int64.TryParse(cdRepres, out cdRepresInt))
                {
                    return null;
                }

                var indexSeparadorGrupoNome = consorc?.IndexOf(" - ") ?? -1;
                var length = consorc?.Length ?? 0;

                Consorciado? consorciado = null;

                if (indexSeparadorGrupoNome > 0)
                {
                    consorciado = new Consorciado
                    {
                        Grupo = consorc?.Substring(0, 6),
                        Cota = consorc?.Substring(7, indexSeparadorGrupoNome - 7),
                        Nome = consorc?.Substring(indexSeparadorGrupoNome + 3)
                    };
                }

                var result = new Comissaovenda
                {
                    CdRepres = cdRepres,
                    CdNmCategComis = cdNmCategComis,
                    Parcela = parcela,
                    Consorciado = consorciado,
                    VendCota = vendCota,
                    Equipe = equipe,
                    PercComissao = percComissao,
                    NroContrato = nroContrato,
                    VlrBase = vlrBase,
                    DataVenda = dataVenda,
                    VlRBrutoComissao = vlRBrutoComissao,
                    Observacao = observacao
                };

                return result;
            });

            var comissoesValidas = comissoes.Where(c => c != null);
            var result = comissoesValidas.ToArray();
            return result;
        });

        return result;
    }
}
