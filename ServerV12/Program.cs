using ServerV12.Types;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGraphQLServer().AddQueryType<Query>(d => d.BindFieldsImplicitly()).ModifyOptions(x => x.DefaultBindingBehavior = BindingBehavior.Explicit);

var app = builder.Build();
app.MapGraphQL();
app.Run();