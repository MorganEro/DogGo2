using DogGo2.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DogGo2.Models;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DogGo2.Controllers
{
    public class DogsController : Controller
    {
        private readonly IDogRepository _dogRepo;
        public DogsController(IDogRepository dogRepository)
        {
            _dogRepo = dogRepository;
        }

        [Authorize]
        // GET: DogsController
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);

            return View(dogs);
        }

        // GET: DogsController/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            if (dog == null)
            {
                return NotFound();
            }
            return View(dog);
        }
        [Authorize]
        // GET: DogsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                // update the dogs OwnerId to the current user's Id
                dog.OwnerId = GetCurrentUserId();

                _dogRepo.AddDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(dog);
            }
        }

        // GET: DogsController/Edit/5
        public ActionResult Edit(int id)
        {
            int O_Id = GetCurrentUserId();
            Dog dog = _dogRepo.GetDogById(id);
            if (dog.OwnerId != O_Id )
            {
                return NotFound();
            }
            return View(dog);
        }

        [Authorize]
        // POST: DogsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            int O_Id = GetCurrentUserId();           
                 if (dog.OwnerId == O_Id )
                {                
                    _dogRepo.UpdateDog(dog);
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }           
        }

        // GET: DogsController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            return View(dog);
        }

        // POST: DogsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepo.RemoveDog(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(dog);
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
