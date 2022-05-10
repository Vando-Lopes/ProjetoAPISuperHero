using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroApi.Data;
using SuperHeroApi.Models;

namespace SuperHeroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public SuperHeroController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> Get(int id)
        {
            var hero = await _dataContext.SuperHeroes.FindAsync(id);
            if (hero == null)
                return BadRequest("Hero not Found");
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<SuperHero>> AddHero(SuperHero hero)
        {
            _dataContext.SuperHeroes.Add(hero);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<SuperHero>> UpdateHero(SuperHero objectHero)
        {
            var dbHero = await _dataContext.SuperHeroes.FindAsync(objectHero.Id);
            if (dbHero == null)
                return BadRequest("Hero not Found");

            dbHero.Name = string.IsNullOrEmpty(objectHero.Name) ? dbHero.Name : objectHero.Name;
            dbHero.FirstName = string.IsNullOrEmpty(objectHero.FirstName) ? dbHero.FirstName : objectHero.FirstName;
            dbHero.LastName = string.IsNullOrEmpty(objectHero.LastName) ? dbHero.LastName : objectHero.LastName;
            dbHero.Place = string.IsNullOrEmpty(objectHero.Place) ? dbHero.Place : objectHero.Place;

            await _dataContext.SaveChangesAsync();

            return Ok(dbHero);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SuperHero>> DeleteHero(int id)
        {
            var dbHero = await _dataContext.SuperHeroes.FindAsync(id);
            if (dbHero == null)
                return BadRequest("Hero not Found");

            _dataContext.SuperHeroes.Remove(dbHero);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }
    }
}
