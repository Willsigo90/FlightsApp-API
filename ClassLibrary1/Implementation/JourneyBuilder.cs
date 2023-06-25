using BusinessLayer.Interfaz;
using DataAccess.DTOs;
using DataAccess.Models;
using DataAccess.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class JourneyBuilder : IJourneyBuilder
    {
        public Task<Journey> buildJourney(string origin, string destination, List<FlightDto> goingFlightList)
        {
            List<Flight> flightList = goingFlightList.Select(flightDto => new Flight(
                new Transport(flightDto.FlightCarrier, flightDto.FlightNumber),
                    flightDto?.DepartureStation,
                    flightDto?.ArrivalStation,
                    flightDto.Price
            )).ToList();

            var journey = new Journey(origin, destination, flightList.Sum(c => c.Price), flightList);

            var journeyValidator = new JourneyValidator();

            var validation = journeyValidator.Validate(journey);

            if (validation.IsValid == false)
            {
                var errorMessages = "Error in server site: " + validation.ToString();
                throw new ValidationException(errorMessages);
            }

            return Task.FromResult(journey);
        }
    }
}
