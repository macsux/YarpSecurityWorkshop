using System.Net.Http.Json;
using Common;
using Darnton.Blazor.DeviceInterop.Geolocation;
using MetarParserCore.Enums;
using MetarParserCore.Objects;
using Microsoft.AspNetCore.Components;

namespace WeatherApp.Pages;

public partial class Weather
{
    [Inject]
    GeolocationService geolocationService { get; set; }
    [Inject]
    HttpClient httpClient { get; set; }
    private DateTime currentTime;
    private decimal lon;
    private decimal lat;
    GeolocationResult CurrentPositionResult;
    Metar metar = new ();
    Station station = new ();
    private Timer _timer;
    private int RelativeHumidity => 100 - 5 * (metar.Temperature?.Value - metar.Temperature?.DewPoint ?? 0);

    
    protected override async Task OnInitializedAsync()
    {
        _timer = new Timer(OnTimerTick, null, 0, 1000);
    
        CurrentPositionResult = await geolocationService.GetCurrentPosition();
        var coords = CurrentPositionResult.Position.Coords;
        station = await httpClient.GetFromJsonAsync<Station>($"http://localhost:5228/station/closest?latitude={coords.Latitude}&longitude={coords.Longitude}") ?? new();
        metar = await httpClient.GetFromJsonAsync<Metar>($"http://localhost:5130/WeatherForecast/forecast/{station.StationId}") ?? new();
    }

    private void OnTimerTick(object? state)
    {
        currentTime = DateTime.Now;
        StateHasChanged();
    
    } 
}