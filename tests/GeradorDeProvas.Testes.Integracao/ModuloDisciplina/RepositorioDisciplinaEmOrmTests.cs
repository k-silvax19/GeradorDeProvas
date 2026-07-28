using GeradorDeProvas.Infra.Compartilhado.Orm;
using GeradorDeProvas.Infra.Modulos.ModuloDisciplina;
using GeradorDeProvas.Testes.Integracao.ModuloProva;
using Microsoft.EntityFrameworkCore;

namespace GeradorDeProvas.Testes.Integracao.ModuloDisciplina;


[TestClass]
public sealed class RepositorioDisciplinaEmOrmTests
{
    private GeradorDeProvasDbContext dbContext = null!;
    private RepositorioDisciplinaEmOrm repositorio = null!;

    [TestInitialize]
    public void InicializarRepositorio()
    {
        dbContext = CriarDbContext(Guid.NewGuid());

        repositorio = new RepositorioDisciplinaEmOrm(dbContext);
    }

    [TestCleanup]
    public void LimparContexto()
    {
        dbContext.Dispose();

    }



    private GeradorDeProvasDbContext CriarDbContext(Guid userid)
    {
        DbContextOptions<GeradorDeProvasDbContext> options =
            new DbContextOptionsBuilder<GeradorDeProvasDbContext>()
                .UseInMemoryDatabase("GeradorDeProvasTestDB_Memory")
                .Options;

        return new GeradorDeProvasDbContext(options, new ProvedorDeUsuarioFake(userid));
    }

}