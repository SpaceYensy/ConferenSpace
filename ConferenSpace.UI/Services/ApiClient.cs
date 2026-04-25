using System.Net.Http.Json;
using ConferenSpace.UI.Models;

namespace ConferenSpace.UI.Services;

public class ApiClient
{
    private readonly HttpClient _http;

    public ApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Salon>> GetSalonesAsync()
    {
        var response = await _http.GetFromJsonAsync<List<Salon>>("api/salones");
        return response ?? new();
    }

    public async Task<Salon?> GetSalonByIdAsync(int id) =>
        await _http.GetFromJsonAsync<Salon>($"api/salones/{id}");

    public async Task CreateSalonAsync(Salon salon)
    {
        var response = await _http.PostAsJsonAsync("api/salones", salon);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateSalonAsync(int id, Salon salon)
    {
        var response = await _http.PutAsJsonAsync($"api/salones/{id}", salon);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteSalonAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/salones/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Solicitante>> GetSolicitantesAsync()
    {
        var response = await _http.GetFromJsonAsync<List<Solicitante>>("api/solicitantes");
        return response ?? new();
    }

    public async Task<Solicitante?> GetSolicitanteByIdAsync(int id) =>
        await _http.GetFromJsonAsync<Solicitante>($"api/solicitantes/{id}");

    public async Task CreateSolicitanteAsync(Solicitante solicitante)
    {
        var response = await _http.PostAsJsonAsync("api/solicitantes", solicitante);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateSolicitanteAsync(int id, Solicitante solicitante)
    {
        var response = await _http.PutAsJsonAsync($"api/solicitantes/{id}", solicitante);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteSolicitanteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/solicitantes/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Recurso>> GetRecursosAsync()
    {
        var response = await _http.GetFromJsonAsync<List<Recurso>>("api/recursos");
        return response ?? new();
    }

    public async Task<Recurso?> GetRecursoByIdAsync(int id) =>
        await _http.GetFromJsonAsync<Recurso>($"api/recursos/{id}");

    public async Task CreateRecursoAsync(Recurso recurso)
    {
        var response = await _http.PostAsJsonAsync("api/recursos", recurso);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateRecursoAsync(int id, Recurso recurso)
    {
        var response = await _http.PutAsJsonAsync($"api/recursos/{id}", recurso);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteRecursoAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/recursos/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Reserva>> GetReservasAsync()
    {
        var response = await _http.GetFromJsonAsync<List<Reserva>>("api/reservas");
        return response ?? new();
    }

    public async Task<Reserva?> GetReservaByIdAsync(int id) =>
        await _http.GetFromJsonAsync<Reserva>($"api/reservas/{id}");

    public async Task CreateReservaAsync(ReservaCrearDTO reserva)
    {
        var response = await _http.PostAsJsonAsync("api/reservas", reserva);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateReservaAsync(int id, ReservaCrearDTO reserva)
    {
        var response = await _http.PutAsJsonAsync($"api/reservas/{id}", reserva);
        response.EnsureSuccessStatusCode();
    }

    public async Task CancelarReservaAsync(int id)
    {
        var response = await _http.PatchAsync($"api/reservas/{id}/cancelar", null);
        response.EnsureSuccessStatusCode();
    }
}