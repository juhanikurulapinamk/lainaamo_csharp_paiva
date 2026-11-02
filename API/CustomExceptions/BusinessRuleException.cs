
namespace API.CustomExceptions
{
    // Tämä poikkeus tarkoittaa: pyyntö oli muodoltaan kelvollinen,
    // mutta operaatio ei ole sallittu sääntöjen takia
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException() { }

        public BusinessRuleException(string message) : base(message) { }
    }
}