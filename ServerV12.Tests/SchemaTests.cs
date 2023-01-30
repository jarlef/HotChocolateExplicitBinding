using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using ServerV12.Types;

namespace ServerV12.Tests;

public class SchemaTests
{
    [Fact]
        public async Task DefaultRegistration_Book_ShouldHaveZeroFields()
        {
            var serviceBuilder = new ServiceCollection();

            var result = await Assert.ThrowsAsync<SchemaException>(async () =>
            {
                await serviceBuilder.AddGraphQLServer()
                    .ModifyOptions(x => x.DefaultBindingBehavior = BindingBehavior.Explicit)
                    .AddQueryType<Query>(d => d.BindFieldsImplicitly())
                    .BuildSchemaAsync();
            });

            result.Errors.Should().NotBeEmpty();
            var error = result.Errors.First();
            error.Message.Should().Be("The object type `Book` has to at least define one field in order to be valid.");
        }
    
        [Fact]
        public async Task AddBookType_Book_ShouldHaveZeroFields()
        {
            var serviceBuilder = new ServiceCollection();

            var result = await Assert.ThrowsAsync<SchemaException>(async () =>
            {
                await serviceBuilder.AddGraphQLServer()
                    .ModifyOptions(x => x.DefaultBindingBehavior = BindingBehavior.Explicit)
                    .AddQueryType<Query>(d => d.BindFieldsImplicitly())
                    .AddType<BookType>()
                    .BuildSchemaAsync();
            });

            result.Errors.Should().NotBeEmpty();
            var error = result.Errors.First();
            error.Message.Should().Be("The object type `Book` has to at least define one field in order to be valid.");
        }
    
        [Fact]
    
        public async Task AddBookTypeWithExplicitBinding_Book_ShouldHaveZeroFields()
        {
            var serviceBuilder = new ServiceCollection();

            var result = await Assert.ThrowsAsync<SchemaException>(async () =>
            {
                await serviceBuilder.AddGraphQLServer()
                    .ModifyOptions(x => x.DefaultBindingBehavior = BindingBehavior.Explicit)
                    .AddQueryType<Query>(d => d.BindFieldsImplicitly())
                    .AddType<BookTypeWithExplicitBinding>()
                    .BuildSchemaAsync();
            });

            result.Errors.Should().NotBeEmpty();
            var error = result.Errors.First();
            error.Message.Should().Be("The object type `Book` has to at least define one field in order to be valid.");

        }
        
        [Fact]
        public async Task AddBookTypeWithOneField_Book_ShouldHaveOneField()
        {
            var serviceBuilder = new ServiceCollection();

            var schema = await serviceBuilder.AddGraphQLServer()
                .ModifyOptions(x => x.DefaultBindingBehavior = BindingBehavior.Explicit)
                .AddQueryType<Query>(d => d.BindFieldsImplicitly())
                .AddType<BookTypeWithOneField>()
                .BuildSchemaAsync();

            var bookType = schema.GetType<ObjectType>("Book");
            bookType.Fields.Count.Should().Be(2); // title + __typename
            bookType.Fields.ContainsField("title").Should().BeTrue();
            bookType.Fields.ContainsField("authorName").Should().BeFalse();
        }
        
        [Fact]
        public async Task AddBookTypeWithAllFields_Book_ShouldHaveAllField()
        {
            var serviceBuilder = new ServiceCollection();

            var schema = await serviceBuilder.AddGraphQLServer()
                .ModifyOptions(x => x.DefaultBindingBehavior = BindingBehavior.Explicit)
                .AddQueryType<Query>(d => d.BindFieldsImplicitly())
                .AddType<BookTypeWithAllFields>()
                .BuildSchemaAsync();

            var bookType = schema.GetType<ObjectType>("Book");
            bookType.Fields.Count.Should().Be(3); // title + authorName + __typename
            bookType.Fields.ContainsField("title").Should().BeTrue();
            bookType.Fields.ContainsField("authorName").Should().BeTrue();
        }
    
        public class BookType : ObjectType<Book>
        {
            protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
            {
            }
        }
    
        public class BookTypeWithExplicitBinding : ObjectType<Book>
        {
            protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
            {
                descriptor.BindFieldsExplicitly();
            }
        }
        
        public class BookTypeWithOneField : ObjectType<Book>
        {
            protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
            {
                descriptor.Field(x => x.Title);
                // ignoring AuthorName
            }
        }
        
        public class BookTypeWithAllFields : ObjectType<Book>
        {
            protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
            {
                descriptor.Field(x => x.Title);
                descriptor.Field(x => x.AuthorName);
            }
        }
}