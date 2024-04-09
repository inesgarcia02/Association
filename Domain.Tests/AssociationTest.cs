using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;

namespace Domain.Tests
{
    public class AssociationTest
    {
        [Fact]
        public void WhenPassingCorrectDataWithDataFim_ThenAssociationIsInstanciated()
        {
            // arrange
            Mock<IPeriod> doublePeriod = new Mock<IPeriod>();
            long colaboratorId = 1;
            long projectId = 1;

            // act
            Association a = new Association(colaboratorId, projectId, doublePeriod.Object);

            // assert
            Assert.NotNull(a);
        }

        // [Theory]
        // [InlineData("2022-10-01", "2022-10-01")]
        // [InlineData("2022-10-01", "2022-09-30")]
        // public void WhenPassingInvalidDates_ThenThrowsException(string inicio, string fim)
        // {
        //     // arrange
        //     Mock<IColaborator> doubleColaborator = new Mock<IColaborator>();
        //     DateOnly dataInicio = DateOnly.Parse(inicio);
        //     DateOnly dataFim = DateOnly.Parse(fim);

        //     // assert
        //     var ex = Assert.Throws<ArgumentException>(() =>

        //         // act
        //         new Association(doubleColaborator.Object, dataInicio, dataFim)
        //     );

        //     Assert.Equal("Invalid arguments.", ex.Message);
        // }


        [Theory]
        [InlineData(1)]
        public void WhenRequestingIsColaboratorInAssociation_ThenReturnTrue(long id)
        {
            // arrange
            Mock<IPeriod> doublePeriod = new Mock<IPeriod>();
            long colaboratorId = 1;
            long projectId = 1;

            Association association = new Association(colaboratorId, projectId, doublePeriod.Object);

            // act
            bool actual = association.IsColaboratorInAssociation(id);

            // assert
            Assert.True(actual);
        }

        [Theory]
        [InlineData(2)]
        public void WhenRequestingIsColaboradorInAssociation_ThenReturnFalse(long id)
        {
            // arrange
            Mock<IPeriod> doublePeriod = new Mock<IPeriod>();
            long colaboratorId = 1;
            long projectId = 1;

            Association association = new Association(colaboratorId, projectId, doublePeriod.Object);

            // act
            bool actual = association.IsColaboratorInAssociation(id);

            // assert
            Assert.False(actual);
        }


        // [Fact]
        // public void WhenRequestingAddColaboradorEmPeriodo_ThenReturnList()
        // {
        //     // arrange
        //     List<IColaborator> colaboradores = new List<IColaborator>();

        //     Mock<IColaborator> colabDouble = new Mock<IColaborator>();
        //     DateOnly dataInicio = new DateOnly(2024, 3, 1);
        //     DateOnly dataFim = new DateOnly(2024, 3, 15);

        //     Association Association = new Association(colabDouble.Object, dataInicio, dataFim);

        //     DateOnly inicioPeriodo = new DateOnly(2024, 1, 1);
        //     DateOnly fimPeriodo = new DateOnly(2024, 12, 31);

        //     // act
        //     List<IColaborator> listaFinal = Association.AddColaboradorEmPeriodo(colaboradores, inicioPeriodo, fimPeriodo);

        //     int expected = 1;
        //     int actual = listaFinal.Count;

        //     // assert
        //     Assert.Equal(expected, actual);
        // }


        [Theory]
        [InlineData("2024-07-01", "2024-07-15")] // _startDate = dataInicio _endDate = dataFim
        [InlineData("2024-07-01", "2024-07-07")] //_startDate = dataInicio _endDate > dataFim
        [InlineData("2024-07-01", "2024-07-16")] //_startDate = dataInicio _endDate < dataFim
        [InlineData("2024-06-30", "2024-07-07")] //_startDate > dataInicio _endDate > dataFim
        [InlineData("2024-06-30", "2024-07-15")] //_startDate > dataInicio _endDate = dataFim
        [InlineData("2024-06-30", "2024-07-16")] //_startDate > dataInicio _endDate < dataFim
        [InlineData("2024-07-07", "2024-07-15")] // _startDate < dataInicio _endDate = dataFim
        [InlineData("2024-07-07", "2024-07-10")] // _startDate < dataInicio _endDate > dataFim
        [InlineData("2024-07-07", "2024-07-16")] // _startDate < dataInicio _endDate < dataFim
        public void WhenRequestingIsAssociationInPeriod_ThenReturnTrue(string inicio, string fim)
        {
            // arrange
            DateOnly startDate = DateOnly.Parse(inicio);
            DateOnly endDate = DateOnly.Parse(fim);

            DateOnly startAssociation = new DateOnly(2024, 7, 1);
            DateOnly endAssociation = new DateOnly(2024, 7, 15);

            Mock<IPeriod> doublePeriod = new Mock<IPeriod>();
            doublePeriod.Setup(p => p.StartDate).Returns(startAssociation);
            doublePeriod.Setup(p => p.EndDate).Returns(endAssociation);

            long colaboratorId = 1;
            long projectId = 1;
            Association association = new Association(colaboratorId, projectId, doublePeriod.Object);

            // act
            bool actual = association.IsAssociationInPeriod(startDate, endDate);

            // assert
            Assert.True(actual);
        }


        [Theory]
        [InlineData("2024-06-01", "2024-06-15")] // _startDate < dataInicio _endDate < dataInicio
        [InlineData("2024-06-30", "2024-07-01")] //_startDate < dataInicio _endDate = dataInicio
        [InlineData("2024-08-30", "2024-10-01")] //_startDate > dataFim _endDate > dataFim
        [InlineData("2024-07-15", "2024-10-01")] //_startDate = dataFim _endDate > dataFim
        public void WhenRequestingIsAssociationEmPeriodo_ThenReturnFalse(string inicio, string fim)
        {
            // arrange
            DateOnly startDate = DateOnly.Parse(inicio);
            DateOnly endDate = DateOnly.Parse(fim);

            DateOnly startAssociation = new DateOnly(2024, 7, 1);
            DateOnly endAssociation = new DateOnly(2024, 7, 15);

            Mock<IPeriod> doublePeriod = new Mock<IPeriod>();
            doublePeriod.Setup(p => p.StartDate).Returns(startAssociation);
            doublePeriod.Setup(p => p.EndDate).Returns(endAssociation);

            long colaboratorId = 1;
            long projectId = 1;
            Association association = new Association(colaboratorId, projectId, doublePeriod.Object);

            // act
            bool actual = association.IsAssociationInPeriod(startDate, endDate);

            // assert
            Assert.False(actual);
        }



        [Theory]
        [InlineData("2024-01-01", "2024-12-31", "2024-02-01", "2024-04-30")] // dataInicio < _dataInicio  dataFim > _dataFim
        [InlineData("2024-01-01", "2024-03-31", "2024-02-01", "2024-03-31")] // dataInicio < _dataInicio  dataFim < _dataFim
        [InlineData("2024-01-01", "2024-04-30", "2024-02-01", "2024-04-30")] // dataInicio < _dataInicio  dataFim = _dataFim
        [InlineData("2024-02-01", "2024-12-31", "2024-02-01", "2024-04-30")] // dataInicio = _dataInicio  dataFim > _dataFim
        [InlineData("2024-02-01", "2024-04-30", "2024-02-01", "2024-04-30")] // dataInicio = _dataInicio  dataFim = _dataFim
        [InlineData("2024-02-01", "2024-03-31", "2024-02-01", "2024-03-31")] // dataInicio = _dataInicio  dataFim < _dataFim
        [InlineData("2024-03-01", "2024-04-30", "2024-03-01", "2024-04-30")] // dataInicio > _dataInicio  dataFim = _dataFim
        [InlineData("2024-03-01", "2024-05-31", "2024-03-01", "2024-04-30")] // dataInicio > _dataInicio  dataFim > _dataFim
        [InlineData("2024-03-01", "2024-03-31", "2024-03-01", "2024-03-31")] // dataInicio > _dataInicio  dataFim < _dataFim
        public void WhenRequestingGetDatesAssociationInPeriod_ThenReturnTuple(string start, string end, string expectedStart, string expectedEnd)
        {
            // arrange
            DateOnly startDate = DateOnly.Parse(start);
            DateOnly endDate = DateOnly.Parse(end);

            DateOnly startExpected = DateOnly.Parse(expectedStart);
            DateOnly endExpected = DateOnly.Parse(expectedEnd);

            DateOnly startAssociation = new DateOnly(2024, 2, 1);
            DateOnly endAssociation = new DateOnly(2024, 4, 30);

            Mock<IPeriod> doublePeriod = new Mock<IPeriod>();
            doublePeriod.Setup(p => p.StartDate).Returns(startAssociation);
            doublePeriod.Setup(p => p.EndDate).Returns(endAssociation);

            long colaboratorId = 1;
            long projectId = 1;

            Association association = new Association(colaboratorId, projectId, doublePeriod.Object);

            // act
            var datesFinal = association.GetDatesAssociationInPeriod(startDate, endDate);
            DateOnly actualStart = datesFinal.start;
            DateOnly actualEnd = datesFinal.end;

            // assert
            Assert.Equal(startExpected, actualStart);
            Assert.Equal(endExpected, actualEnd);
        }

        [Theory]
        [InlineData("2025-01-01", "2025-12-31")]
        [InlineData("2024-04-30", "2025-12-31")]
        [InlineData("2023-04-30", "2023-12-31")]
        [InlineData("2023-04-30", "2024-02-01")]
        public void WhenRequestingGetDatasAssociationEmPeriodo_ThenReturnMinData(string start, string end)
        {
            // arrange
            DateOnly startDate = DateOnly.Parse(start);
            DateOnly endDate = DateOnly.Parse(end);

            Mock<IPeriod> doublePeriod = new Mock<IPeriod>();
            long colaboratorId = 1;
            long projectId = 1;

            Association association = new Association(colaboratorId, projectId, doublePeriod.Object);

            // act
            var datesFinal = association.GetDatesAssociationInPeriod(startDate, endDate);
            DateOnly actualStart = datesFinal.start;
            DateOnly actualEnd = datesFinal.end;

            // assert
            Assert.Equal(DateOnly.MinValue, actualStart);
            Assert.Equal(DateOnly.MinValue, actualEnd);
        }

    }
}