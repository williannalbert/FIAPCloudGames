using Application.DTOs.Game;
using Application.Interfaces;

namespace Presentation.Seeders;

public static class GameSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        await CreateGame(scope, "The Witcher 3: Wild Hunt", "RPG de mundo aberto com narrativa profunda.", 59.99m);
        await CreateGame(scope, "God of War", "Aventura épica de Kratos e Atreus na mitologia nórdica.", 49.99m);
        await CreateGame(scope, "Red Dead Redemption 2", "Aventura no velho oeste com mundo aberto imersivo.", 69.99m);
        await CreateGame(scope, "Elden Ring", "RPG de ação em mundo aberto criado por Hidetaka Miyazaki e George R. R. Martin.", 69.99m);
        await CreateGame(scope, "Cyberpunk 2077", "RPG futurista em Night City.", 39.99m);
        await CreateGame(scope, "Horizon Forbidden West", "Aventuras de Aloy em um mundo pós-apocalíptico.", 59.99m);
        await CreateGame(scope, "Resident Evil 4 Remake", "Remake do clássico survival horror.", 59.99m);
        await CreateGame(scope, "Minecraft", "Jogo sandbox de construção e aventura.", 26.95m);
        await CreateGame(scope, "Fortnite", "Battle Royale multiplayer.", 0.00m);
        await CreateGame(scope, "League of Legends", "MOBA de sucesso mundial.", 0.00m);
        await CreateGame(scope, "Valorant", "FPS tático competitivo da Riot Games.", 0.00m);
        await CreateGame(scope, "Counter-Strike 2", "FPS tático clássico renovado.", 0.00m);
        await CreateGame(scope, "Grand Theft Auto V", "Mundo aberto cheio de possibilidades.", 29.99m);
        await CreateGame(scope, "Dark Souls III", "RPG de ação desafiador.", 59.99m);
        await CreateGame(scope, "Sekiro: Shadows Die Twice", "Ação intensa e desafiadora no Japão feudal.", 59.99m);
        await CreateGame(scope, "Call of Duty: Modern Warfare III", "FPS de guerra moderna.", 69.99m);
        await CreateGame(scope, "Assassin's Creed Valhalla", "Aventuras vikings em mundo aberto.", 59.99m);
        await CreateGame(scope, "Far Cry 6", "Ação em mundo aberto em Yara.", 59.99m);
        await CreateGame(scope, "Death Stranding", "Aventura de entrega em um mundo pós-apocalíptico.", 49.99m);
        await CreateGame(scope, "FIFA 24", "Simulação de futebol realista.", 69.99m);
        await CreateGame(scope, "NBA 2K24", "Simulação de basquete.", 69.99m);
        await CreateGame(scope, "Rocket League", "Futebol com carros.", 0.00m);
        await CreateGame(scope, "The Legend of Zelda: Breath of the Wild", "Aventura épica em Hyrule.", 59.99m);
        await CreateGame(scope, "Super Mario Odyssey", "Aventura em 3D com Mario.", 59.99m);
        await CreateGame(scope, "Metroid Dread", "Exploração e ação intensa com Samus.", 59.99m);
        await CreateGame(scope, "Splatoon 3", "Batalhas de tinta em multiplayer.", 59.99m);
        await CreateGame(scope, "Animal Crossing: New Horizons", "Simulador de vida em uma ilha.", 59.99m);
        await CreateGame(scope, "Pokémon Scarlet and Violet", "RPG de captura de monstros.", 59.99m);
        await CreateGame(scope, "Final Fantasy XVI", "Novo capítulo da clássica série de RPG.", 69.99m);
        await CreateGame(scope, "Persona 5 Royal", "RPG japonês com estilo único.", 59.99m);
        await CreateGame(scope, "Monster Hunter Rise", "Caça a monstros gigantes.", 59.99m);
        await CreateGame(scope, "Apex Legends", "Battle Royale gratuito.", 0.00m);
        await CreateGame(scope, "Destiny 2", "FPS de ação em mundo compartilhado.", 0.00m);
        await CreateGame(scope, "Overwatch 2", "FPS multiplayer em equipes.", 0.00m);
        await CreateGame(scope, "Diablo IV", "RPG de ação sombria.", 69.99m);
        await CreateGame(scope, "Baldur's Gate 3", "RPG baseado em D&D.", 69.99m);
        await CreateGame(scope, "Starfield", "Exploração espacial da Bethesda.", 69.99m);
        await CreateGame(scope, "Alan Wake 2", "Terror psicológico cinematográfico.", 59.99m);
        await CreateGame(scope, "Lies of P", "RPG de ação inspirado em Pinóquio.", 59.99m);
        await CreateGame(scope, "Mortal Kombat 1", "Luta brutal e cinematográfica.", 69.99m);
        await CreateGame(scope, "Street Fighter 6", "Clássico jogo de luta renovado.", 59.99m);
        await CreateGame(scope, "Tekken 8", "Luta com gráficos incríveis.", 69.99m);
        await CreateGame(scope, "Cuphead", "Plataforma com visual de desenho animado dos anos 30.", 19.99m);
        await CreateGame(scope, "Hades", "Roguelike aclamado pela crítica.", 24.99m);
        await CreateGame(scope, "Celeste", "Plataforma desafiadora sobre superação.", 19.99m);
        await CreateGame(scope, "Undertale", "RPG independente com narrativa única.", 9.99m);
        await CreateGame(scope, "Stardew Valley", "Simulador de fazenda.", 14.99m);
        await CreateGame(scope, "Terraria", "Aventura sandbox 2D.", 9.99m);
        await CreateGame(scope, "Dead Cells", "Roguelike de ação rápida.", 24.99m);
        await CreateGame(scope, "Among Us", "Multiplayer social de dedução.", 4.99m);

    }

    private static async Task CreateGame(IServiceScope scope, string name, string description, decimal price)
    {
        var gameService = scope.ServiceProvider.GetRequiredService<IGameService>();
        if (await gameService.GetByNameAsync(name) is null)
            await gameService.CreateAsync(new CreateGameDTO()
            {
                Name = name,
                Description = description,
                Price = price
            });
        }
}
