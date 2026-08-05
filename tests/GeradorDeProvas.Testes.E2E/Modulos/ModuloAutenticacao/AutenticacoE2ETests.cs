using System.Runtime.Intrinsics.Arm;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace GeradorDeProvas.Testes.E2E.Modulos;

[TestClass]
public sealed class AutenticacoE2ETests : PageTest
{
    [TestMethod]
    public async Task Deve_Exibir_TelaDeLogin_ParaUsuarioAnonimo()
    {
        //Arrange
        string urlBase = "http://localhost:8001";

        // Act
        await Page.GotoAsync($"{urlBase}/");

        // Assert
        await Expect(Page).ToHaveTitleAsync(new Regex("Entrar"));
    }

    [TestMethod]
    public async Task Deve_RegistrarEAutenticar_Usuario()
    {
        // Arrange

        string urlBase = "http://localhost:8001";

        const string email = "novo.usuario@teste.local";
        const string senha = "Senha123!";

        await Page.GotoAsync($"{urlBase}/Autenticacao/Registrar");

        // Act
        await Page.GetByLabel("E-mail").FillAsync(email);
        await Page.GetByLabel("Senha", new() { Exact = true }).FillAsync(senha);
        await Page.GetByLabel("Confirmar Senha").FillAsync(senha);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Criar Conta" }).ClickAsync();

        // Assert
        string rotaAbsoluta = new Uri(Page.Url).AbsolutePath;

        Assert.AreEqual("/", rotaAbsoluta);
    }
}
