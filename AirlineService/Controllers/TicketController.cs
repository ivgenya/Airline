﻿using System.Net.Http.Headers;
using System.Text;
using AirlineService.DTO;
using AirlineService.Models;
using AirlineService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AirlineService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketController : ControllerBase
{
    private readonly ITicketService _service;
    private readonly HttpClient _httpClient;

    public TicketController(ITicketService ticketService, HttpClient httpClient)
    {
        _service = ticketService;
        _httpClient = httpClient;
    }
    
    [HttpPost("buy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> BuyTicket([FromBody]PassengerModel passengerModel, int flightId, int seatId)
    {
        HttpContext httpContext = HttpContext;
        string authorizationHeader = httpContext.Request.Headers["Authorization"];
        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            string bearerToken = authorizationHeader.Substring("Bearer ".Length);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }
        var seat = await _service.GetSeatByIdAsync(seatId);
        if (seat == null)
        {
            return BadRequest("Место не существует");
        }
        if (seat.Status.Equals("reserved"))
        {
            return BadRequest("Место занято");
        }
        var passenger = new Passenger
        {
            Surname = passengerModel.Surname,
            Name = passengerModel.Name,
            DocumentNumber = passengerModel.DocumentNumber,
            DateOfBirth = passengerModel.DateOfBirth,
            Gender = passengerModel.Gender,
            Email = passengerModel.Email
        };
        var ticketId = await _service.BuyTicketAsync(passenger, flightId, seatId);
        var ticket = await _service.GetTicketByIdAsync(ticketId);
        seat.Status = "reserved";
        await _service.UpdateSeatAsync(seat);

        var jsonData = new StringContent(JsonConvert.SerializeObject(ticketId), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://localhost:7066/api/UserTickets?ticketId=" + ticketId, jsonData);
        if (response.IsSuccessStatusCode)
        {
            await response.Content.ReadAsStringAsync();
        }
        else
        {
            Console.WriteLine("Ошибка при отправке POST-запроса");
        }
        if (ticket == null)
            return BadRequest("An error has occurred");

        return Ok(ticket);
    }
    
    [HttpPost("book/{ticketId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> BookTicket(int ticketId)
    {
        var bookingId = await _service.ReserveTicketAsync(ticketId);
        if (bookingId != -1)
        {
            return Ok(bookingId);
        }
        return BadRequest("Booking failed");
    }
    
    [HttpPost("pay/{ticketId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> PayForTicket(int ticketId, [FromBody] PaymentModel paymentInfo)
    {
        var paymentResult = await _service.MakePaymentAsync(ticketId, paymentInfo);
        if (paymentResult)
        {
            var ticket = await _service.GetBoardingPassAsync(ticketId);
            var pdfBytes = _service.GenerateTicket(ticket);
            return File(pdfBytes, "application/pdf", "ticket.pdf");
        }
        return BadRequest("Payment failed");
    }

    [HttpGet("booking/{code}")]
    public async Task<ActionResult<Booking>> GetBookingById(string code)
    {
        var booking = await _service.GetBookingByCodeAsync(code);
        if (booking == null)
        {
            return NotFound();
        }
        return Ok(booking);
    }
    
    [HttpPost("register")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> RegisterForFlight(string ticketCode)
    {
        try
        {
            var ticket = await _service.GetTicketByCodeAsync(ticketCode);
            var boardingPass = await _service.GetBoardingPassAsync(ticket.Id);
            if (DateTime.Now.Add(new TimeSpan(0, 3, 0, 0)) < boardingPass.Date.Add(boardingPass.DepartureTime))
            {
                return BadRequest("Регистрация не началась.");
            }
            if (DateTime.Now.Add(new TimeSpan(0, 1, 0, 0)) > boardingPass.Date.Add(boardingPass.DepartureTime))
            {
                return BadRequest("Регистрация закончилась.");
            }
            if (ticket.Status == "paid" )
            {
                ticket.State.Use(ticket);
                await _service.UpdateTicketAsync(ticket);
                var pdfBytes = _service.GenerateBoardingPass(boardingPass);
                return File(pdfBytes, "application/pdf", "boarding-pass.pdf");
            }
            return BadRequest("Невозможно зарегистрировать билет.");
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, "Произошла ошибка при регистрации на рейс: " + ex.Message);
        }
    }
    [HttpPost("cancel/{bookingId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> CancellBoking(int bookingId)
    {
        try
        {
            var booking = await _service.GetBookingByIdAsync(bookingId);
            var tickets = await _service.GetTicketsByBookingIdAsync(bookingId);

            if (booking == null)
            {
                return BadRequest("Не существует бронирования с таким номером");
            }

            if (!booking.Status.Equals("confirmed"))
            {
                return BadRequest("Бронирование нельзя отменить");
            }

            booking.State.Cancel(booking);
            await _service.UpdateBookingAsync(booking);
            foreach (var ticket in tickets)
            {
                ticket.State.Cancel(ticket);
                await _service.UpdateTicketAsync(ticket);
                var seat = await _service.GetSeatByIdAsync(ticket.SeatId);
                seat.Status = "available";
                await _service.UpdateSeatAsync(seat);
            }

            return Ok("Бронирование отменено");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest("Возникла ошибка при отмене бронирования");
        }
    }
    
    [HttpPost("GetTicketDetails")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> GetTicketDetails([FromBody] List<int> ticketIds)
    {
        if (ticketIds == null)
        {
            return BadRequest("Некорректные параметры запроса");
        }

        List<BoardingPassModel> ticketDetailsList = new List<BoardingPassModel>();

        foreach (var id in ticketIds)
        {
            var ticket = await _service.GetBoardingPassAsync(id);
            if (ticket != null)
            {
                ticketDetailsList.Add(ticket);
            }
        }
        return Ok(ticketDetailsList);
    }
    
    [HttpPost("GetBookingsDetails")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> GetBookingsDetails([FromBody] List<int> ticketIds)
    {
        if (ticketIds == null)
        {
            return BadRequest("Некорректные параметры запроса");
        }
        List<BoardingPassModel> ticketDetailsList = new List<BoardingPassModel>();

        foreach (var id in ticketIds)
        {
            var ticket = await _service.GetBoardingPassAsync(id);
            if (ticket != null && ticket.BookingStatus != null)
            {
                ticketDetailsList.Add(ticket);
            }
        }
        return Ok(ticketDetailsList);
    }
    
    [HttpGet("seats/{flightId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "client")]
    public async Task<ActionResult<Booking>> GetSeatsByFlightId(int flightId)
    {
        var seats = await _service.GetSeatByFlightIdAsync(flightId);
        return Ok(seats);
    }
 
     [HttpGet("GetTicketDetails/{ticketCode}")]
     [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
     [Authorize(Roles = "client")]
     public async Task<IActionResult> GetTicketDetails(string ticketCode)
     {
         var ticket = await _service.GetTicketByCodeAsync(ticketCode);
         var ticketDetails = await _service.GetBoardingPassAsync(ticket.Id);
         if (ticketDetails == null)
             return NotFound();

         return Ok(ticketDetails);
     }
     
     [HttpGet("GetTicketDetailsById/{ticketId}")]
     [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
     [Authorize(Roles = "client")]
     public async Task<IActionResult> GetTicketDetailsById(int ticketId)
     {
         var ticketDetails = await _service.GetBoardingPassAsync(ticketId);
         if (ticketDetails == null)
             return NotFound();

         return Ok(ticketDetails);
     }

     [HttpGet("GetTicketDetailsByBooking/{code}")]
     [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
     [Authorize(Roles = "client")]
     public async Task<IActionResult> GetTicketDetailsByBooking(string code)
     {
         var booking = await _service.GetBookingByCodeAsync(code);
         if (booking != null)
         {
             var tickets = await _service.GetTicketsByBookingIdAsync(booking.Id);
             List<BoardingPassModel> ticketDetailsList = new List<BoardingPassModel>();

             foreach (var ticket in tickets)
             {
                 var ticketDetails = await _service.GetBoardingPassAsync(ticket.Id);
                 ticketDetailsList.Add(ticketDetails);
             }
             return Ok(ticketDetailsList);
         }
         return NotFound("Бронирования с таким номером не существует");
     }
}