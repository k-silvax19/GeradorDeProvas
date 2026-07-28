using GeradorDeProvas.Dominio.Compartilhado.Identity;

namespace GeradorDeProvas.Testes.Integracao.ModuloProva;

public sealed class ProvedorDeUsuarioFake(Guid userid) : IProvedorDeUsuario
{
    public Guid? Id => userid;

    public bool EstaAutenticado => true;
}
