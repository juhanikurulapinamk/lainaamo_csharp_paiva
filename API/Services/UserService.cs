
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
        public async Task<AppUser?> GetByUserName(string username)
        {
            // FirstOrDefaultAsync palauttaa null, jos hakuehdolla ei löydy yhtään riviä tietokannasta
            // muuten sen rivin tiedot, joka niillä löytyy

            // FirstAsync puolestaan heittää poikkeuksen, jos hakuehdolla ei löydy osumia tietokannasta
            

            // EF Core luo tästä sql-kyselyn SELECT * FROM Users WHERE LOWER(UserName) = ? LIMIT 1;
            // jossa kysymysmerkin paikalle tulee username-muuttujan arvo


            // JUHANI juhani

            var user = await _repository.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
            return user;
        }

        public Task<string> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> Register(string username, string password)
        {
            var existingUser = await GetByUserName(username);
            if (existingUser != null)
            {
                throw new UserRegistrationException("username must be unique");
            }
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