using System.Threading.Channels;
using Common.Models;
using Data.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.api.BackgroundServices;
using TrickingLibrary.api.Common.Utilities;
using TrickingLibrary.api.Dtos.Video;

var builder = WebApplication.CreateBuilder(args);

const string allCors = "all";
const string database = "in-memory-development";
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers(options =>
    options.Conventions.Add(
        new RouteTokenTransformerConvention(new SlugifyPath())
    ));
builder.Services.AddCors(options =>
    options.AddPolicy(allCors,
        policyBuilder => policyBuilder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
    ));
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(database));

builder.Services.AddHostedService<VideoEditingService>();
builder.Services.AddSingleton(() => Channel.CreateUnbounded<VideoEditingDto>());

// builder.Services.AddSingleton<TricksStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();
    var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    ctx.AddRange([
        new Difficulty
        {
            Id = "easy",
            Name = "Easy",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Difficulty
        {
            Id = "intermediate",
            Name = "Intermediate",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Difficulty
        {
            Id = "hard",
            Name = "Hard",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
    ]);
    ctx.AddRange([
        new Category
        {
            Id = "roll",
            Name = "Roll",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Category
        {
            Id = "flip",
            Name = "Flip",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Category
        {
            Id = "kick",
            Name = "Kick",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Category
        {
            Id = "jump",
            Name = "Jump",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Category
        {
            Id = "turns",
            Name = "Turns",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Category
        {
            Id = "jab",
            Name = "Jab",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Category
        {
            Id = "hook",
            Name = "Hook",
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        }
    ]);
    ctx.AddRange([
        new Trick
        {
            Id = "front-roll",
            Name = "Front Roll",
            Description = "A basic forward roll used to develop spatial awareness and body control.",
            DifficultyId = "easy",
            TrickCategories = [new TrickCategory { CategoryId = "roll", TrickId = "front-roll" }],
            Prerequisites = [],
            Progressions = [],
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Trick
        {
            Id = "back-roll",
            Name = "Back Roll",
            Description = "A backward roll used to build confidence in backwards movement.",
            DifficultyId = "easy",
            TrickCategories = [new TrickCategory { CategoryId = "roll", TrickId = "back-roll" }],
            Prerequisites =
                [new PrerequisiteProgressionRelation { ProgressionId = "back-roll", PrerequisiteId = "front-roll" }],
            Progressions = [],
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Trick
        {
            Id = "front-flip",
            Name = "Front Flip",
            Description = "A forward somersault in the air.",
            DifficultyId = "intermediate",
            TrickCategories = [new TrickCategory { CategoryId = "flip", TrickId = "front-flip" }],
            Prerequisites =
                [new PrerequisiteProgressionRelation { ProgressionId = "front-flip", PrerequisiteId = "front-roll" }],
            Progressions =
            [
                new PrerequisiteProgressionRelation { ProgressionId = "front-flip-360", PrerequisiteId = "front-flip" }
            ],
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Trick
        {
            Id = "backflip",
            Name = "Backflip",
            Description = "A backward somersault launched from standing.",
            DifficultyId = "intermediate",
            TrickCategories = [new TrickCategory { CategoryId = "flip", TrickId = "back-flip" }],
            Prerequisites =
                [new PrerequisiteProgressionRelation { ProgressionId = "back-flip", PrerequisiteId = "back-roll" }],
            Progressions =
                [new PrerequisiteProgressionRelation { ProgressionId = "back-flip-360", PrerequisiteId = "back-flip" }],
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Trick
        {
            Id = "back-flip-360",
            Name = "Backflip 360",
            Description = "A backflip performed with a full 360-degree twist.",
            DifficultyId = "hard",
            TrickCategories =
            [
                new TrickCategory { CategoryId = "flip", TrickId = "back-flip-360" },
                new TrickCategory { CategoryId = "roll", TrickId = "back-flip-360" }
            ],
            Prerequisites = [],
            Progressions = [],
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Trick
        {
            Id = "front-flip-360",
            Name = "Front Flip 360",
            Description = "A front flip performed with a 360-degree twist.",
            DifficultyId = "hard",
            TrickCategories =
            [
                new TrickCategory { CategoryId = "flip", TrickId = "front-flip-360" },
                new TrickCategory { CategoryId = "roll", TrickId = "front-flip-360" }
            ],
            Prerequisites = [],
            Progressions = [],
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Trick
        {
            Id = "roundhouse-kick",
            Name = "Roundhouse Kick",
            Description = "A powerful circular kick executed by turning the hip over.",
            DifficultyId = "intermediate",
            TrickCategories = [new TrickCategory { CategoryId = "kick", TrickId = "roundhouse-kick" }],
            Prerequisites = [],
            Progressions =
            [
                new PrerequisiteProgressionRelation
                    { ProgressionId = "spinning-roundhouse-kick", PrerequisiteId = "roundhouse-kick" }
            ],
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Trick
        {
            Id = "spinning-roundhouse-kick",
            Name = "Spinning Roundhouse Kick",
            Description = "A roundhouse kick performed after a full spin for increased power.",
            DifficultyId = "hard",
            TrickCategories =
            [
                new TrickCategory { CategoryId = "flip", TrickId = "spinning-roundhouse-kick" },
                new TrickCategory { CategoryId = "kick", TrickId = "spinning-roundhouse-kick" }
            ],
            Prerequisites = [],
            Progressions = [],
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Trick
        {
            Id = "jab-cross",
            Name = "Jab Cross",
            Description = "Basic punching combination using a lead jab and rear cross.",
            DifficultyId = "easy",
            TrickCategories = [new TrickCategory { CategoryId = "jab", TrickId = "jab-cross" }],
            Prerequisites = [],
            Progressions =
            [
                new PrerequisiteProgressionRelation { ProgressionId = "jab-cross-hook", PrerequisiteId = "jab-cross" }
            ],
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Trick
        {
            Id = "jab-cross-hook",
            Name = "Jab Cross Hook",
            Description = "A common three-punch combination ending in a powerful hook.",
            DifficultyId = "intermediate",
            TrickCategories =
            [
                new TrickCategory { CategoryId = "jab", TrickId = "jab-cross-hook" },
                new TrickCategory { CategoryId = "hook", TrickId = "jab-cross-hook" }
            ],
            Prerequisites = [],
            Progressions = [],
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Trick
        {
            Id = "tornado-kick",
            Name = "Tornado Kick",
            Description = "A spinning jumping kick with full hip rotation.",
            DifficultyId = "hard",
            TrickCategories =
            [
                new TrickCategory { CategoryId = "flip", TrickId = "tornado-kick" },
                new TrickCategory { CategoryId = "kick", TrickId = "tornado-kick" }
            ],
            Prerequisites =
            [
                new PrerequisiteProgressionRelation
                    { ProgressionId = "tornado-kick", PrerequisiteId = "roundhouse-kick" }
            ],
            Progressions = [],
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        },
        new Trick
        {
            Id = "720-kick",
            Name = "720 Kick",
            Description = "A martial arts tricking kick involving two full spins in the air.",
            DifficultyId = "hard",
            TrickCategories =
            [
                new TrickCategory { CategoryId = "flip", TrickId = "720-kick" },
                new TrickCategory { CategoryId = "kick", TrickId = "720-kick" }
            ],
            Prerequisites =
                [new PrerequisiteProgressionRelation { ProgressionId = "720-kick", PrerequisiteId = "tornado-kick" }],
            Progressions = [],
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        }
    ]);
    ctx.SaveChanges();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(allCors);
app.MapControllers();

app.Run();