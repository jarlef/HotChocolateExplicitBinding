var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGraphQLServer().AddTypes().ModifyOptions(x => x.DefaultBindingBehavior = BindingBehavior.Explicit);

var app = builder.Build();
app.MapGraphQL();
app.Run();