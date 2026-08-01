using System.Drawing;
using GeradorDeProvas.Dominio.Modulos.ModuloDisciplina;
using GeradorDeProvas.Dominio.Modulos.ModuloProva;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GeradorDeProvas.Aplicacao.Modulos.ModuloProva;

public sealed class GeradorPdfProva()
{
    public byte[] Gerar(DetalhesProvaDto prova, bool incluirGabarito)
    {
        return CriarDocumento(prova, incluirGabarito).GeneratePdf();
    }

    private static IDocument CriarDocumento(DetalhesProvaDto prova, bool incluirGabarito)
    {
        return Document.Create(document =>
        {
            document.Page(pagina =>
            {
                pagina.Size(PageSizes.A4);
                pagina.Margin(2, Unit.Centimetre);
                pagina.PageColor(Colors.White);
                pagina.DefaultTextStyle(style => style.FontSize(12));

                pagina.Header()
                    .Column(header =>
                    {
                        header.Item()
                            .Text(prova.Titulo)
                            .Bold()
                            .FontSize(18)
                            .FontColor(Colors.Blue.Darken2);


                        header.Item()
                            .PaddingTop(4)
                            .Text(texto =>
                            {
                                texto.Span($"Disciplina: {prova.NomeDisciplina}     ");
                                texto.Span(prova.ProvaRecuperacao ? "Prova de recuperação" : $"Matéria: {prova.NomeMateria}     ");
                                texto.Span($"Série: {prova.Serie}");
                            });

                        header.Item()
                            .PaddingTop(8)
                            .LineHorizontal(1)
                            .LineColor(Colors.Grey.Lighten1);
                    });

                pagina.Content()
                    .PaddingVertical(15)
                    .Column(content =>
                    {
                        content.Spacing(12);

                        for (int indice = 0; indice < prova.Questoes.Count; indice++)
                        {
                            QuestaoProvaDto questaoDto = prova.Questoes[indice];

                            content.Item()
                                .PreventPageBreak()
                                .Column(questao => {
                                    questao.Spacing(5);

                                    questao.Item().Text(texto =>
                                    {
                                        texto.Span($"{indice + 1}").Bold();
                                        texto.Span(questaoDto.Enunciado);

                                    });

                                    foreach(AlternativaProvaDto alternativaDto in questaoDto.Alternativas)
                                    {
                                        string marcador = incluirGabarito && alternativaDto.Correta
                                            ? "[X]"
                                            : "[  ]";
                                        questao.Item()
                                            .PaddingLeft(15)
                                            .Text($"{marcador} {alternativaDto.Texto}");
                                    }
                                    
                                });
                        }
                    });

                pagina.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Page ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
            });
        });
    }
}