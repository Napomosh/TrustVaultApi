using Microsoft.EntityFrameworkCore;
using Moq;
using Testcontainers.PostgreSql;
using TrustDrop.Common.Database;
using TrustDrop.Common.Jwt;

namespace TrustDropTest;

public abstract class BaseTest
{
    protected AppDbContext dbContext = null!;
    protected Mock<ITransactional> mockTransactional = null!;
    
    private static PostgreSqlContainer PgContainer { get; set; } = null!;
    
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        JwtAuth.JwtSettings = new JwtSettingsType
        {
            Key = "test-secret-key-with-at-least-32-characters-for-HS256",
            Issuer = "test-issuer",
            Audience = "test-audience",
            Expiration = 3600
        };
        
        PgContainer = new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        await PgContainer.StartAsync();

        await OneTimeConcreteSetUp();
    }
    
    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        await PgContainer.StopAsync();
        await PgContainer.DisposeAsync();

        await OneTimeConcreteTearDown();
    }

    [SetUp]
    public async Task SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(PgContainer.GetConnectionString())
            .Options;

        dbContext = new AppDbContext(options);
        await dbContext.Database.EnsureDeletedAsync(); // очистка между тестами
        await dbContext.Database.EnsureCreatedAsync();
        
        await ConcreteSetUp();
    }
    
    [TearDown]
    public async Task TearDown()
    {
        await dbContext.DisposeAsync();
        
        await ConcreteTearDown();
    }

    protected abstract Task OneTimeConcreteSetUp();
    protected abstract Task OneTimeConcreteTearDown();
    protected abstract Task ConcreteSetUp();
    protected abstract Task ConcreteTearDown();
}