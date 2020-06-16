﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FIT3077.Shared.Models;

namespace FIT3077.Client.Services
{
    public class AppStateService
    {
        public Dashboard Dashboard { get; } = new Dashboard();
        public IReadOnlyDictionary<string, Patient> Patients { get; private set; }
        public bool SearchInProgress { get; private set; }

        public event Action OnChange;

        private readonly HttpClient _http;
        public AppStateService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task Search(string value)
        {
            SearchInProgress = true;
            NotifyStateChanged();

            Dashboard.Patients = await _http.GetFromJsonAsync<Dictionary<string, Patient>>($"/api/practitioner/{value}");
            SearchInProgress = false;
            NotifyStateChanged();
        }

        public async Task AddToMonitor(Patient patient)
        {
            Monitor monitor = new Monitor(patient.Id, patient.Name);
            monitor.MeasurementList = await FetchMeasurement(monitor);
            Dashboard.RegisterMonitor(monitor);
            NotifyStateChanged();
        }

        public async Task<Measurement> FetchMeasurement(Monitor monitor)
        {
            var id = monitor.PatientId;
            var measurement = new Measurement()
            {
                //BloodPressureRecords = await _http.GetFromJsonAsync<List<BloodPressureRecord>>($"/api/patient/{id}/measurement/blood-pressure"),
                CholesterolRecords = await _http.GetFromJsonAsync<List<Record>>($"/api/patient/{id}/measurement/cholesterol")
            };
            return measurement;
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
