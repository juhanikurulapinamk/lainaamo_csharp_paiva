
using API.CustomExceptions;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{

    // kun nyt käytämme EF Corea, pystymme injektoimaan serviceen
    // suoraan DataContextin ja käyttää sitä repositoryna
    // DataContext on jo rekisteröity aiemmin käyttäen AddDbContext-metodia Program.cs-filussa
    public class UserService(DataContext _repository, IPasswordHasher<AppUser> _passwordHasher) : IUserService
    {
        public async Task<AppUser> Register(string username, string password)
        {
            var user = new AppUser
            {
                UserName = username,
                Role = "user",
                // Asetetaan väliaikainen arvo, koska property on required.
                // Oikea hash lasketaan heti seuraavalla rivillä.
                HashedPassword = ""
            };

            // HashPassword ottaa parametrikseen käyttäjän ja selkokielisen salasanan.
            // Se arpoo saltin itse, laskee hashin ja palauttaa merkkijonon,
            // johon on pakattu sekä algoritmin tunniste, salt että itse hash.
            user.HashedPassword = _passwordHasher.HashPassword(user, password);
            // tämä tekee käytännössä insertin Users-tietokantatauluun
            // koska olemme määrittäneet DbSetiksi Usersin, pystymme käyttämään sitä näin
            await _repository.Users.AddAsync(user);
            // insert pitää vahvistaa, jotta rivi oikeasti tallennetaan
            await _repository.SaveChangesAsync();

            return user;
        }
    }
}