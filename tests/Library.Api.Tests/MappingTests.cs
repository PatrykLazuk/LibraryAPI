using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Api.DTOs;
using Library.Api.Extensions;
using Library.Api.Models;
using Xunit;

namespace Library.Api.Tests
{
    public class MappingTests
    {
        [Fact(DisplayName = "Given a Book entity - When mapped to DTO - Then all fields match")]
        public void Entity_ToDto_AllFieldsMatch()
        {
            //Given
            var entity = new Book { Id = 42, Title = "T", Author = "A", Isbn = "123" };
            entity.ChangeStatus(BookStatus.Borrowed);

            //When
            var dto = entity.ToDto();

            //Then 
            dto.Should().BeEquivalentTo(entity, opt => opt.ExcludingNonBrowsableMembers());
        }

        [Fact(DisplayName = "Given a CreateBookDto - When converted to entity - Then status defaults to OnShelf")]
        public void CreateDto_ToEntity_StatusDefault()
        {
            //Given
            var dto = new CreateBookDto { Title = "T", Author = "A", Isbn = "321" };

            //When
            var entity = dto.ToEntity();

            //Then
            entity.Status.Should().Be(BookStatus.OnShelf);
        }
    }
}