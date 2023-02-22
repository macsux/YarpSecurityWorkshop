using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Darnton.Blazor.DeviceInterop.Geolocation;
using MetarParserCore.Enums;
using MetarParserCore.Objects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace WeatherApp.Pages;

public partial class Weather
{
    [Inject] GeolocationService GeolocationService { get; set; } = null!;
    [Inject]
    HttpClient HttpClient { get; set; } = null!;

    
    private DateTime CurrentTime { get; set; }
    GeolocationResult? CurrentPositionResult { get; set; }
    // Metar metar = new ();
    Station _station = new ();
    private Timer _timer = null!;
    // private int RelativeHumidity => 100 - 5 * (metar.Temperature?.Value - metar.Temperature?.DewPoint ?? 0);
    // private int Temp => ConvertToF ? (int)(metar.Temperature.Value * 1.8 + 32) : metar?.Temperature?.Value ?? 0;
    private WeatherReport WeatherReport { get; set; } = null!;
    private int Temp => ConvertToF ? WeatherReport.TempF : WeatherReport.TempC;
    private bool ConvertToF { get; set; }

    [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; } = null!;
    private string? User { get; set; }
    private bool IsLoading { get; set; } = true;
    
    protected override async Task OnInitializedAsync()
    {
        User = (await AuthenticationState).User.Identity?.Name;
        _timer = new Timer(OnTimerTick, null, 0, 1000);
    
        CurrentPositionResult = await GeolocationService.GetCurrentPosition();
        var coords = CurrentPositionResult.Position.Coords;
        _station = await HttpClient.GetFromJsonAsync<Station>($"/api/station/closest?latitude={coords.Latitude}&longitude={coords.Longitude}") ?? new();
        WeatherReport = await HttpClient.GetFromJsonAsync<WeatherReport>($"/api/weatherforecast/{_station.StationId}") ?? new();
        
        IsLoading = false;
    }

    private void OnTimerTick(object? state)
    {
        CurrentTime = DateTime.Now;
    }

    private void SetUnitC()
    {
        ConvertToF = false;
    }
    private void SetUnitF()
    {
        ConvertToF = true;
    }
}