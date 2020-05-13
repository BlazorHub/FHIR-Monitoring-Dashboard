﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.IRepositories;
using FHIR_FIT3077.IRepository;
using FHIR_FIT3077.Models;
using FHIR_FIT3077.Repository;
using FHIR_FIT3077.ViewModel;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace FHIR_FIT3077.Controllers
{
    public class PractitionerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            _cache.Refresh("Monitor");
            _cache.Refresh("Patient");
            return View();
        }

        private readonly IPractitionerRepository _practitioner;
        private readonly ICacheRepository _cache;

        public PractitionerController(IPractitionerRepository practitioner, ICacheRepository distributedCache)
        {
            _practitioner = practitioner;
            _cache = distributedCache;
        }

        [HttpPost]
        [ActionName("SubmitId")]
        public IActionResult SubmitId(PractitionerModel model)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("LoadPatient", new {id = model.Id});
            }

            return Index();
        }


        //This method load all unique patients from the server and store the list in cache
        public IActionResult LoadPatient(string id)
        {
            var patientModel = new PatientViewModel();
            if (_cache.ExistObject<Dictionary<string, PatientModel>>(id.ToString()) == true)
            {
                var patientsFromCache = _cache.GetObject<Dictionary<string, PatientModel>>(id.ToString());
            }
            else
            {
                patientModel.PatientList = _practitioner.GetTotalPatients(id);
                _cache.SetObject("Patients", patientModel.PatientList);
            }
            return View(patientModel);
        }

        
    }
}